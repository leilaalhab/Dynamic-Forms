using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DynamicForms.Services.FormulaService;


namespace DynamicForms.Services
{
    public class HandleFormService : CompleteForm.CompleteFormBase
    {
        private readonly ILogger<HandleFormService> _logger;
        private readonly DataContext _context;
        private readonly IMongoCollection<FormulaTree> _FormulasCollection;
        protected InputWrapper[] inputs;

        public HandleFormService(ILogger<HandleFormService> logger, DataContext context, IOptions<DynamicFormsDatabaseSettings> dynamicFormsDbSettings)
        {
            var mongoClient = new MongoClient(dynamicFormsDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(dynamicFormsDbSettings.Value.DatabaseName);
            _FormulasCollection = mongoDatabase.GetCollection<FormulaTree>(dynamicFormsDbSettings.Value.DynamicFormsCollectionName);
            _logger = logger;
            _context = context;
        }

        public override async Task HandleForm(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            if (requestStream is null)
                return;

            try
            {
                var formRequest = context.RequestHeaders.FirstOrDefault(c => c.Key == "formid") ?? throw new Exception("No formId was found.");
                SetInputs(int.Parse(formRequest.Value));

                foreach (var input in inputs)
                {
                    if (input.Input.IsVisible)
                        await responseStream.WriteAsync(GenerateResponse(input, ResponseType.NewInput));
                }

                while (await requestStream.MoveNext())
                {
                    var request = requestStream.Current;
                    if (request.RequestType == RequestType.InputValue)
                    {
                        var currentInput = inputs.ElementAt(request.Id);
                        if (IsInputValid(currentInput, request.Value, request.TextValue))
                        {
                            await responseStream.WriteAsync(await SendPriceResponse(int.Parse(formRequest.Value), currentInput));

                            foreach (var condition in await GetDependentInputConditions(request.Id))
                            {
                                if (CheckCondition(condition.Requirement, request, currentInput))
                                    await responseStream.WriteAsync(GenerateResponse(FindInputWithId(condition.DependentInput), ResponseType.NewInput));
                            }
                        }
                        await responseStream.WriteAsync(GenerateResponse(currentInput, ResponseType.InputValidity));
                    }
                    else
                    {
                        await responseStream.WriteAsync(SubmitFormResponse(CheckAllInputsValidity(inputs)));
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public void SetInputs(int formId)
        {
            var step = _context.Steps.Include(c => c.Inputs).ThenInclude(i => i.Requirements).Where(c => c.FormId == formId).FirstOrDefault() ?? throw new Exception($"No steps belonging to FormID {formId} were found.");

            inputs = step.Inputs.Select(c => new InputWrapper
            {
                Input = c,
            }).ToArray();
        }


        private Stack<Node>? FindInputPath(Node root, Input input)
        {
            Stack<Node> nodes = new();
            
                do
                {
                        nodes.Push(root);

                    while (!IsLeaf(root))
                    {
                        root = root.Left;
                        nodes.Push(root);
                    }
                    

                    if (NodeMatchesInput(root, input))
                    {
                        return nodes;
                    }
                    else
                    {
                        var temp = nodes.Pop();

                        if (root.Right == temp)
                        {
                            nodes.Pop();
                        }
                        root = nodes.Peek().Right;
                    }

                } while (nodes.Count > 0);
            
            return null;
        }

        // private async Task<Node?> FindNodeOnPath(Stack<Node> nodes, Node root, Input input)
        // {
        //     await Task.Run(() =>
        //     {
        //         nodes.Push(root);

        //         while (!IsLeaf(root))
        //         {
        //             root = root.Left;
        //             nodes.Push(root);
        //         }

        //         if (NodeMatchesInput(root, input))
        //         {
        //             return null;
        //         }
        //         else
        //         {
        //             var temp = nodes.Pop();

        //             if (root.Right == temp)
        //             {
        //                 nodes.Pop();
        //             }
        //             return nodes.Peek().Right;
        //         }
        //     });
        //     return null;
        // }


        public async Task<Response> SendPriceResponse(int formId, InputWrapper currentInput)
        {
            var formula = await _FormulasCollection.Find(x => x.FormId == formId).FirstOrDefaultAsync();
            double val = CalculatePrice(formula.Root, currentInput.Input);

            return new Response
            {
                ResponseType = ResponseType.Price,
                Value = val,
            };
        }

        public double CalculatePrice(Node root, Input input)
        {
            Stack<Node>? nodes = FindInputPath(root, input);

            if (nodes is null)
            {
                return root.Value;
            }
            else
            {
                nodes.Pop();
                double temp = 0;

                foreach (var node in nodes)
                {
                    var operation = node;
                    var op1 = node.Left;
                    var op2 = node.Right;
                    TreeGenerator.Calculate(GetValue(op1), GetValue(op2), operation);
                    temp = node.Value;
                }

                return temp;

            }
        }

        private async Task<List<Condition>> GetDependentInputConditions(int InputId)
        {
            return await _context.Conditions.Include(c => c.Requirement).Where(r => r.Requirement.InputId == InputId).ToListAsync();
        }

        private Response SubmitFormResponse(bool FormComplete)
        {
            if (FormComplete)
                return new Response
                {
                    ResponseType = ResponseType.FormSubmitAccepted
                };
            else
                return new Response
                {
                    ResponseType = ResponseType.FormSubmitRejected
                };
        }

        private Response GenerateResponse(InputWrapper input, ResponseType responseType)
        {
            var res = new Response
            {
                Id = input.Input.Order - 1,
                Label = input.Input.Label,
                Placeholder = input.Input.Placeholder,
                InpType = input.Input.InputType,
                ErrorValue = input.ErrorValue,
                Value = input.Value,
                TextValue = input.TextValue,
                ResponseType = responseType,
            };
            if (input.Error != ErrorType.NoError)
            {
                res.Error = input.Error;
            }

            return res;
        }

        private bool CheckAllInputsValidity(InputWrapper[] inputs)
        {
            foreach (var input in inputs)
            {
                if (IsInputValid(input, input.Value, input.TextValue))
                    return false;
            }
            return true;
        }

        private bool CheckCondition(Requirement req, Request request, InputWrapper input)
        {
            if (string.IsNullOrEmpty(request.TextValue))
            {
                if (!CheckRequirement(req, request.Value, input))
                    return false;
            }
            else
            {
                if (!CheckRequirement(req, request.TextValue.Length, input))
                    return false;
            }

            input.Input.IsVisible = true;
            return true;
        }

        private bool IsInputValid(InputWrapper input, double requestValue, string requestText)
        {
            if (input.Input.Requirements is null)
                return true;

            double value;

            if (string.IsNullOrEmpty(requestText))
            {
                value = requestValue;
            }
            else
            {
                value = requestText.Length;
            }


            foreach (var req in input.Input.Requirements)
            {
                if (!CheckRequirement(req, value, input))
                {
                    input.Value = value;
                    return false;
                }
            }

            return true;
        }

        public static bool CheckRequirement(Requirement requirement, double value, InputWrapper input)
        {
            switch (requirement.Type)
            {
                case ConditionType.Equal:
                    if (value == requirement.Value)
                        return true;
                    input.Error = ErrorType.Equal;
                    input.ErrorValue = requirement.Value;
                    return false;

                case ConditionType.NotEqual:
                    if (value != requirement.Value)
                        return true;
                    input.Error = ErrorType.NotEqual;
                    input.ErrorValue = requirement.Value;
                    return false;

                case ConditionType.LessThan:
                    if (value < requirement.Value)
                        return true;
                    input.Error = ErrorType.LessThan;
                    input.ErrorValue = requirement.Value;
                    return false;

                case ConditionType.GreaterThan:
                    if (value > requirement.Value)
                        return true;
                    input.Error = ErrorType.GreaterThan;
                    input.ErrorValue = requirement.Value;
                    return false;
            }
            return false;
        }

        private double GetValue(Node? node)
        {
            if (node is null)
                throw new Exception("Formula calculation error.");

            if (node.Type == NodeType.Variable)
            {
                if (node.InputId is not null)
                    return FindInputWithId(node.InputId.Value).Value;
                else
                    throw new Exception("Node does not have InputId.");
            }
            else
                return node.Value;
        }

        private bool IsLeaf(Node node)
        {
            if (node.Left is null && node.Right is null)
                return true;
            return false;
        }

        public bool NodeMatchesInput(Node node, Input input)
        {
            if (node.Type == NodeType.Variable && node.InputId == input.Id)
                return true;
            return false;
        }

        private InputWrapper FindInputWithId(int InputId)
        {
            return inputs.FirstOrDefault(r => r.Input.Id == InputId) ?? throw new Exception($"Input with Id {InputId} was not found.");
        }
    }
}