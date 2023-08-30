using System.Net.WebSockets;
using DynamicForms.Services.HandleFormService;
using DynamicForms.Services.HandleFormulaService;
using Formpackage;
using Google.Protobuf;
using InputValueRequest = Formpackage.InputValueRequest;

namespace DynamicForms.Controllers
{
    public class WebSocketController : ControllerBase
    {
        private readonly IHandleFormService _HandleFormService;
        private readonly IHandleFormulaService _HandleFormulaService;
        private int FormId;
        private InputWrapper[] Inputs;
        private Progress Progress;
        private FormulaTree Formula;
        private WebSocket socket;

        public WebSocketController(IHandleFormService handleFormService, IHandleFormulaService handleFormulaService)
        {
            _HandleFormService = handleFormService;
            _HandleFormulaService = handleFormulaService;
        }

        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("WebSocket Connected");
                socket = webSocket;
                await HandleRequest();
            }
            else
            {
                Console.WriteLine("Unexpected error.");
            }
        }

        private async Task HandleRequest()
        {
            while (socket.State == WebSocketState.Open)
            {
                var buffer = new byte[1024];
                var receiveResult =
                    await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var data = buffer.Skip(1).Take(receiveResult.Count - 1).ToArray();

                switch ((RequestType)buffer[0])
                {
                    case RequestType.StartForm:
                        await StartForm(data);
                        break;
                    case RequestType.InputValue:
                        await CheckInputValueValidity(data);
                        break;
                    case RequestType.NextStep:
                        await NextStep();
                        break;
                    case RequestType.PreviousStep:
                        await PreviousStep();
                        break;
                    case RequestType.SubmitForm:
                        await SubmitForm();
                        break;
                    default:
                        await socket.SendAsync(InvalidRequestType("Invalid Request Type. Internal Error."),
                            WebSocketMessageType.Binary, false,
                            CancellationToken.None);
                        break;
                }
            }
        }

        public async Task SubmitForm()
        {
            await CheckUnchangedInputs();
            await _HandleFormService.SaveValues(Inputs, Progress.Id);
        }

        public async Task NextStep()
        {
            if (await CheckUnchangedInputs())
            {
                var nextStep = await _HandleFormService.GetNextStep(Progress.StepId);

                if (nextStep is null)
                {
                    await SubmitForm();
                    return;
                }

                Progress.StepId = nextStep.Value;
                await _HandleFormService.SetInputs(Progress);
            }
        }

        public async Task PreviousStep()
        {
            if (await CheckUnchangedInputs())
            {
                var previousStep = await _HandleFormService.GetPreviousStep(Progress.StepId);
                if (previousStep is null)
                    return;

                Progress.StepId = previousStep.Value;
                await _HandleFormService.SetInputs(Progress);
            }
        }

        private async Task<bool> CheckUnchangedInputs()
        {
            var UnchangedInputs = _HandleFormService.GetUnchangedInputs(Inputs);

            if (UnchangedInputs is null)
                return true;

            foreach (var InvalidInput in UnchangedInputs)
            {
                var response = _HandleFormService.GenerateInputResponse(InvalidInput);
                byte[] buffer = serializeProto(ResponseType.FormValid, response);
                await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
            }

            return false;
        }


        public async Task CheckInputValueValidity(byte[] data)
        {
            try
            {
                var request = InputValueRequest.Parser.ParseFrom(data);

                var currentInput = Inputs[request.Index - 1];

                if (_HandleFormService.IsInputValid(currentInput, request.NumValue, request.TextValue))
                    await SendDependentInputs(currentInput, request);

                await SendInputValidityResponse(currentInput);
                //await CalculatePrice(currentInput.Input.Id);
            }
            catch (Exception e)
            {
                await socket.SendAsync(InvalidRequestType("Error occurred when checking input validity. Message : " + e), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }


        private byte[] InvalidRequestType(string message)
        {
            var response = new ValidityResponse
            {
                Valid = false,
                Message = message
            };
            var buffer = serializeProto(ResponseType.FormValid, response);
            return buffer;
        }

        public async Task StartForm(byte[] buffer)
        {
            var request = StartFormRequest.Parser.ParseFrom(buffer);
            Console.WriteLine(request.FormId);
            await CheckForm(request.FormId, request.ProgressId);
        }

        public async Task CheckForm(int formId, int progressId)
        {
            ValidityResponse response = new();
            try
            {
                bool formExists = _HandleFormService.DoesFormExist(formId);
                if (formExists)
                {
                    FormId = formId;
                    response.Valid = formExists;
                    await InitialSendInputs(progressId);
                }
            }
            catch (Exception e)
            {
                response.Valid = false;
                response.Message = e.Message;
            }

            byte[] buffer = serializeProto(ResponseType.FormValid, response);
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        private byte[] serializeProto(ResponseType type, Object obj)
        {
            byte[] method = new byte[1];
            
            byte[] buffer;
            int size;
            switch (type)
            {
                case ResponseType.FormValid:
                {
                    method[0] = (int)ResponseType.FormValid;
                    size = ((ValidityResponse)obj).CalculateSize();
                    buffer = new byte[size];
                    CodedOutputStream temp = new CodedOutputStream(buffer);
                    ((ValidityResponse)obj).WriteTo(temp);
                    break;
                }
                case ResponseType.Input:
                {
                    method[0] = (int)ResponseType.Input;
                    size = ((InputResponse)obj).CalculateSize();
                    buffer = new byte[size];
                    CodedOutputStream temp = new CodedOutputStream(buffer);
                    ((InputResponse)obj).WriteTo(temp);
                    break;
                }
                case ResponseType.StepChangeValid:
                {
                    method[0] = (int)ResponseType.StepChangeValid;
                    size = ((ValidityResponse)obj).CalculateSize();
                    buffer = new byte[size];
                    CodedOutputStream temp = new CodedOutputStream(buffer);
                    ((ValidityResponse)obj).WriteTo(temp);
                    break;
                }
                case ResponseType.Price:
                {
                    method[0] = (int)ResponseType.Price;
                    size = ((PriceResponse)obj).CalculateSize();
                    buffer = new byte[size];
                    CodedOutputStream temp = new CodedOutputStream(buffer);
                    ((PriceResponse)obj).WriteTo(temp);
                    break;
                }
                case ResponseType.InputInvalid:
                {
                    method[0] = (int)ResponseType.InputInvalid;
                    size = ((InputInvalidResponse)obj).CalculateSize();
                    buffer = new byte[size];
                    CodedOutputStream stream = new CodedOutputStream(buffer);
                    ((InputInvalidResponse)obj).WriteTo(stream);
                    break;
                }
                default:
                {
                    method[0] = 1;
                    ((ValidityResponse)obj).Message = "Invalid Response Type.";
                    size = ((ValidityResponse)obj).CalculateSize();
                    buffer = new byte[size];
                    CodedOutputStream temp = new CodedOutputStream(buffer);
                    ((PriceResponse)obj).WriteTo(temp);
                    break;
                }
            }

            var result = new byte[1 + size];
            Buffer.BlockCopy(method, 0, result, 0, 1);
            Buffer.BlockCopy(buffer, 0, result, 1, buffer.Length);
            return result;
        }

        private async Task InitialSendInputs(int progressId)
        {
            try
            {
                var progress = await _HandleFormService.GetProgress(FormId, progressId);
                // sets the inputs that belong to this form - if progress existed, values are included
                var inputs = await _HandleFormService.SetInputs(progress);
                Inputs = inputs;
                Progress = progress;

                foreach (var input in inputs)
                {
                    if (input.Input.IsVisible)
                    {
                        var temp = _HandleFormService.GenerateInputResponse(input);
                        var buffer = serializeProto(ResponseType.Input, temp);
                        await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                        if (input.Error is not null)
                        {
                            var err = _HandleFormService.GenerateInputValidityResponse(input);
                            var buff = serializeProto(ResponseType.InputInvalid, temp);
                            await socket.SendAsync(buff, WebSocketMessageType.Binary, true, CancellationToken.None);

                        }
                            
                    }
                }

                await InitialCalculatePrice();
            }
            catch (Exception e)
            {
                await socket.SendAsync(InvalidRequestType(e.Message), WebSocketMessageType.Binary, true,
                    CancellationToken.None);
            }
        }

        private async Task InitialCalculatePrice()
        {
            FormulaTree formula = await _HandleFormulaService.GetFormula(FormId);
            Formula = formula;
            FormulaInputPaths formulaInputPaths = await _HandleFormulaService.GetInputPaths(FormId);

            if (formulaInputPaths.Paths is not null)
            {
                foreach (var Path in formulaInputPaths.Paths)
                {
                    Inputs.FirstOrDefault(c => c.Input.Id == Path.InputId).Path = Path.Path;
                }
            }

            var value = _HandleFormulaService.EvaluateFormula(formula, Inputs);
            await socket.SendAsync(SendPriceResponse(value), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        private byte[] SendPriceResponse(double value)
        {
            var response = new PriceResponse
            {
                Price = value,
            };
            var buffer = serializeProto(ResponseType.Price, response);
            
            return buffer;
        }

        private async Task CalculatePrice(int InputId)
        {
            var value = await _HandleFormulaService.EvaluateFormula(Formula, Inputs, InputId);
            await socket.SendAsync(SendPriceResponse(value), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        private async Task SendDependentInputs(InputWrapper currentInput, InputValueRequest request)
        {
            foreach (var condition in await _HandleFormService.GetDependentInputConditions(currentInput.Input.Id))
            {
                if (_HandleFormService.CheckCondition(condition.Requirement, request, currentInput))
                {
                    var input = FindInputWithId(condition.DependentInput);
                    if (!input.Input.IsVisible)
                    {
                        var response = _HandleFormService.GenerateInputResponse(input);
                        var buffer = serializeProto(ResponseType.Input, response);
                        await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                }
            }
        }

        private async Task SendInputValidityResponse(InputWrapper currentInput)
        {
            var response = _HandleFormService.GenerateInputValidityResponse(currentInput);
            var buffer = serializeProto(ResponseType.InputInvalid, response);
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        private InputWrapper FindInputWithId(int InputId)
        {
            return Inputs.FirstOrDefault(r => r.Input.Id == InputId) ??
                   throw new Exception($"Input with Id {InputId} was not found.");
        }
    }
}