using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;


namespace DynamicForms.Services
{
    public class HandleFormService : CompleteForm.CompleteFormBase
    {
        private readonly ILogger<HandleFormService> _logger;
        private readonly DataContext _context;
        public HandleFormService(ILogger<HandleFormService> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            Console.WriteLine("An instance of the class HandleFormService was created.");
        }
        public override async Task HandleForm(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            if (requestStream is null)
                return;

            try
            {
                var formRequest = context.RequestHeaders.FirstOrDefault(c => c.Key == "formid") ?? throw new Exception("No formId was found.");

                var step = await _context.Steps.Include(c => c.Inputs).ThenInclude(i => i.Requirements).Where(c => c.FormId == int.Parse(formRequest.Value)).FirstOrDefaultAsync() ?? throw new Exception($"No steps belonging to FormID {formRequest.Value} were found.");
                

                var inputs = step.Inputs.Select(c => new Inp
                {
                    Input = c,
                }).ToArray() ?? throw new Exception("Step contains no inputs.");

                

                for (int i = 0; i < inputs.Count(); i++)
                {
                    var input = inputs.ElementAt(i);

                    if (input.Input.IsVisible)
                    {
                        var res = GenerateResponse(input, ResponseType.NewInput);
                        //if (input.Input.Choices != null)
                        //res.Choices.Add(input.Input.Choices.Select(r => r.Label));
                        await responseStream.WriteAsync(res);
                    }
                }

                while (await requestStream.MoveNext())
                {   if (requestStream.Current.RequestType == RequestType.InputValue) {
                    int inpId = requestStream.Current.Id;
                    //var currentInput = inputs.FirstOrDefault(c => c.Input.Id == inpId) ?? throw new Exception($"No Input with Id {inpId} was found.");
                    var currentInput = inputs.ElementAt(inpId);

                    if (CheckInput(currentInput, requestStream.Current.Value, requestStream.Current.TextValue))
                    {
                        var conds = currentInput.Input.Requirements;
                        var conditions = await _context.Conditions.Include(c => c.Requirement).Where(r => r.Requirement.InputId == inpId).ToListAsync();
                        foreach (var condition in conditions)
                        {
                            if (CheckCondition(condition.Requirement, requestStream.Current, currentInput))
                            {
                                var input = inputs.FirstOrDefault(r => r.Input.Id == condition.DependentInput) ?? throw new Exception($"No Input with Id {inpId} was found.");

                                var res = GenerateResponse(input, ResponseType.NewInput);
                                //if (res.Choices != null)
                                // res.Choices.Add(input.Input.Choices.Select(r => r.Label));
                                await responseStream.WriteAsync(res);
                            }
                        }
                    }
                    await responseStream.WriteAsync(GenerateResponse(currentInput, ResponseType.InputValidity));
                } else {
                    if (CheckInputValidity(inputs)) {
                        await responseStream.WriteAsync(new Response {
                            ResponseType = ResponseType.FormSubmitAccepted
                        });
                    } else {
                        await responseStream.WriteAsync(new Response {
                            ResponseType = ResponseType.FormSubmitRejected
                        });
                    }
                }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private bool CheckInputValidity(Inp[] inputs) {
            foreach (var input in inputs)
            {
                if(CheckInput(input, input.Value, input.TextValue))
                    return false;
            }
            return true;
        }

        private Response GenerateResponse(Inp input, ResponseType responseType)
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

        private bool CheckCondition(Requirement req, Request request, Inp input)
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
            return true;
        }

        private bool CheckInput(Inp input, double requestValue, string requestText)
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

        private bool CheckRequirement(Requirement requirement, double value, Inp input)
        {
            input.ErrorValue = requirement.Value;
            switch (requirement.Type)
            {
                case ConditionType.Equal:
                    if (value == requirement.Value)
                        return true;
                    input.Error = ErrorType.Equal;
                    return false;

                case ConditionType.NotEqual:
                    if (value != requirement.Value)
                        return true;
                    input.Error = ErrorType.NotEqual;
                    return false;

                case ConditionType.LessThan:
                    if (value < requirement.Value)
                        return true;
                    input.Error = ErrorType.LessThan;
                    return false;

                case ConditionType.GreaterThan:
                    if (value > requirement.Value)
                        return true;
                    input.Error = ErrorType.GreaterThan;
                    return false;
            }
            return false;
        }
    }

    public class Inp
    {
        public required Input Input { get; set; }
        public ErrorType Error { get; set; } = ErrorType.NoError;
        public double ErrorValue { get; set; }
        public double Value { get; set; }
        public string TextValue { get; set; } = string.Empty;
        public int Index { get; set; }
        
    }
}