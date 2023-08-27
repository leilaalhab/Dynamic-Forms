using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using DynamicForms.Services.HandleFormService;
using DynamicForms.Services.HandleFormulaService;

namespace DynamicForms.Hubs
{
    public class FormHub : Hub
    {
        private readonly IHandleFormService _HandleFormService;
        private readonly IHandleFormulaService _HandleFormulaService;

        public FormHub(IHandleFormService handleFormService, IHandleFormulaService handleFormulaService)
        {
            _HandleFormService = handleFormService;
            _HandleFormulaService = handleFormulaService;
        }
        // public functions that are called by the client
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("--> Connection Established " + Context.ConnectionId);
            Clients.Caller.SendAsync("RecieveConnID", Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public async Task CheckForm(int formId, int progressId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                bool formExists = _HandleFormService.DoesFormExist(formId);
                if (formExists)
                {
                    if (!Context.Items.ContainsKey("formid"))
                        Context.Items.Add("formid", formId);
                    response.Data = formExists;
                    await InitialSendInputs(progressId);
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            await Clients.Caller.SendAsync("RecieveFormStatus", response);
        }

        public async Task CheckInputValueValidity(JsonElement InputValueRequest)
        {
            var response = new ServiceResponse<Response>();
            try
            {
                var req = InputValueRequest.Deserialize<InputValueRequest>();

                if (req is null)
                    return;

                Request request = new()
                {
                    Id = req.Index,
                    InpType = req.InpType,
                    Value = int.Parse(req.Value),
                    TextValue = req.Value
                };


                var currentInput = GetInputs()[request.Id];

                if (_HandleFormService.IsInputValid(currentInput, request.Value, request.TextValue))
                    await SendDependentInputs(currentInput, request);

                await SendInputValidityResponse(currentInput);
                await CalculatePrice(currentInput.Input.Id);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
        }

        public async Task SubmitForm()
        {
            var response = new ServiceResponse<Response>();
            try
            {
                await CheckUnchangedInputs();
                await _HandleFormService.SaveValues(GetInputs(), GetProgress().Id);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            await Clients.Caller.SendAsync("RecieveSubmitCompletionStatus", response);
        }

        public async Task NextStep()
        {
            var response = new ServiceResponse<Response>();
            try
            {
                if (await CheckUnchangedInputs())
                {
                    Progress progress = GetProgress();
                    var nextStep = _HandleFormService.GetNextStep(progress.StepId);
                    if (nextStep is null)
                    {
                        await SubmitForm();
                        return;
                    }

                    progress.StepId = nextStep.Id;
                    await _HandleFormService.SetInputs(progress);
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            await Clients.Caller.SendAsync("ChangeStepStatus", response);
        }

        public async Task PreviousStep()
        {
            var response = new ServiceResponse<Response>();

            try
            {
                if (await CheckUnchangedInputs())
                {
                    Progress progress = GetProgress();
                    var previousStep = _HandleFormService.GetPreviousStep(progress.StepId);
                    if (previousStep is null)
                        return;

                    progress.StepId = previousStep.Id;
                    await _HandleFormService.SetInputs(progress);
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            await Clients.Caller.SendAsync("ChangeStepStatus", response);
        }

        //logic
        private async Task<bool> CheckUnchangedInputs()
        {
            var UnchangedInputs = _HandleFormService.GetUnchangedInputs(GetInputs());

            if (UnchangedInputs is null)
                return true;

            foreach (var InvalidInput in UnchangedInputs)
            {
                Response response = _HandleFormService.GenerateResponse(InvalidInput, ResponseType.InputValidity);
                await Clients.Caller.SendAsync("recieveInput", JsonSerializer.Serialize(response, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull }));
            }
            return false;
        }

        private async Task InitialSendInputs(int progressId)
        {
            var progress = await _HandleFormService.GetProgress(GetFormId(), progressId);
            var inputs = await _HandleFormService.SetInputs(progress); // sets the inputs that belong to this form -- if progress existed, values are included
            Context.Items.Add("Inputs", inputs);
            Context.Items.Add("progress", progress);
            foreach (var input in inputs)
            {
                if (input.Input.IsVisible)
                {
                    var temp = _HandleFormService.ReturnResponse(input);
                    var responseJSON = JsonSerializer.Serialize(temp, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                    await Clients.Caller.SendAsync("RecieveInput", responseJSON);
                }
            }

            await InitialCalculatePrice();
        }

        private async Task InitialCalculatePrice()
        {
            FormulaTree formula = await _HandleFormulaService.GetFormula(GetFormId());
            Context.Items.Add("formula", formula);
            FormulaInputPaths formulaInputPaths = await _HandleFormulaService.GetInputPaths(GetFormId());

            if (formulaInputPaths.Paths is not null)
            {

                foreach (var Path in formulaInputPaths.Paths)
                {
                    FindInputWithId(Path.InputId).Path = Path.Path;
                }

            }

            var value = _HandleFormulaService.EvaluateFormula(formula, GetInputs());
            await SendPriceResponse(value);
        }

        private async Task SendPriceResponse(double value)
        {
            var response = new Response
            {
                ResponseType = ResponseType.Price,
                Value = value,
            };
            var responseJSON = JsonSerializer.Serialize(response, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
            await Clients.Caller.SendAsync("recievePrice", responseJSON);
        }

        private async Task CalculatePrice(int InputId)
        {
            var formula = GetFormula();
            var value = await _HandleFormulaService.EvaluateFormula(formula, GetInputs(), InputId);

            await SendPriceResponse(value);
        }

        private async Task SendDependentInputs(InputWrapper currentInput, Request request)
        {
            foreach (var condition in await _HandleFormService.GetDependentInputConditions(currentInput.Input.Id))
            {
                if (_HandleFormService.CheckCondition(condition.Requirement, request, currentInput))
                {
                    var Input = FindInputWithId(condition.DependentInput);
                    if (!Input.Input.IsVisible)
                    {
                        var response = _HandleFormService.GenerateResponse(Input, ResponseType.NewInput);
                        await Clients.Caller.SendAsync("receiveinput", JsonSerializer.Serialize(response));
                    }
                }
            }
        }

        private async Task SendInputValidityResponse(InputWrapper currentInput)
        {
            Response response = _HandleFormService.GenerateResponse(currentInput, ResponseType.InputValidity);
            await Clients.Caller.SendAsync("recieveinput", JsonSerializer.Serialize(response));
        }

        private int GetFormId()
        {
            Context.Items.TryGetValue("formid", out object? formId);
            if (formId is not null)
                return (int)formId;
            else
                throw new Exception("FormId has not been initialized.");
        }

        private Progress GetProgress()
        {
            Context.Items.TryGetValue("progress", out object? progress);
            if (progress is not null)
                return (Progress)progress;
            else
                throw new Exception("Progress has not been initialized.");
        }

        private FormulaTree GetFormula()
        {
            Context.Items.TryGetValue("formula", out object? formula);
            if (formula is not null)
                return (FormulaTree)formula;
            else
                throw new Exception("Formula has not been initialized.");
        }
        private InputWrapper[] GetInputs()
        {
            Context.Items.TryGetValue("Inputs", out object? inputs);
            if (inputs is not null)
                return (InputWrapper[])inputs;
            else
                throw new Exception("inputs have not been initialized.");
        }

        private InputWrapper FindInputWithId(int InputId)
        {
            return GetInputs().FirstOrDefault(r => r.Input.Id == InputId) ?? throw new Exception($"Input with Id {InputId} was not found.");
        }
    }
}