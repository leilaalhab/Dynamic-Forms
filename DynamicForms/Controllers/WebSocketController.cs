using System.Net.WebSockets;
using DynamicForms.Models.Answers;
using DynamicForms.Services.HandleFormService;
using DynamicForms.Services.HandleFormulaService;
using Formpackage;
using Google.Protobuf;

namespace DynamicForms.Controllers;

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
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            Console.WriteLine("WebSocket Connected");
            socket = webSocket;

            while (socket.State == WebSocketState.Open)
            {
                await HandleRequest();
            }
        }
        else
        {
            Console.WriteLine("Unexpected error.");
        }
    }

    private async Task HandleRequest()
    {
        var buffer = new byte[3485];
        var receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var data = buffer.Skip(1).Take(receiveResult.Count - 1).ToArray();

        switch ((RequestType)buffer[0])
        {
            case RequestType.StartForm:
            {
                var request = StartFormRequest.Parser.ParseFrom(data);
                await StartForm(request);
                break;
            }
            case RequestType.InputValue:
            {
                var request = InputValueRequest.Parser.ParseFrom(data);
                await CheckInputValueValidity(request);
                break;
            }
            case RequestType.NextStep:
            {
                await NextStep();
                break;
            }
            case RequestType.PreviousStep:
            {
                await PreviousStep();
                break;
            }
            default:
            {
                await SendMessageAsync(false, "Invalid Request Type. Internal Error.");
                break;
            }
        }
    }
    
    public async Task StartForm(StartFormRequest request)
    {
        try
        {
            var formExists = await _HandleFormService.DoesFormExist(request.FormId);
            if (formExists)
            {
                await SendMessageAsync(true, "Form is Valid.");
                await InitialSendInputs(request.ProgressId);
                await InitialCalculatePrice();
            }
        }
        catch (Exception e)
        {
            await SendMessageAsync(false, e.Message);
        }
    }


    private async Task SendInputs()
    {
        List<Input> unwrappedInputs = await _HandleFormService.GetFormInputs(Progress);
        List<Answer>? answers = await _HandleFormService.GetAnswers(Progress.Id);
        List<InputWrapper> inputs = new();

        if (answers is null)
        {
            foreach (var input in unwrappedInputs)
            {
                var wrapper = new InputWrapper { Input = input, Index = input.Order - 1, Value = input.DefaultValue };
                inputs.Add(wrapper);
                if (input.IsVisible)
                {
                    var response = _HandleFormService.GenerateInputResponse(wrapper);
                    var buffer = SerializeProto(ResponseType.Input, response);
                    await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                }
            }
        }
        else
        {
            foreach (var input in unwrappedInputs)
            {
                var wrapper = new InputWrapper { Input = input, Index = input.Order - 1};
                var answer = answers.Find(c => c.InputId == input.Id);

                if (answer is null)
                {
                    wrapper.Value = input.DefaultValue;
                    if (input.IsVisible)
                    {
                        var response = _HandleFormService.GenerateInputResponse(wrapper);
                        var buffer = SerializeProto(ResponseType.Input, response);
                        await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                }
                else
                {
                    wrapper.Input.IsVisible = true;
                    wrapper.Interacted = true;
                    AssignValue(wrapper, answer);
                    _HandleFormService.IsInputValueValid(wrapper, wrapper.Value, wrapper.TextValue);
                    
                    var response = _HandleFormService.GenerateInputAndValueResponse(wrapper);
                    var buffer = SerializeProto(ResponseType.Input, response);
                    await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                }

                inputs.Add(wrapper);
            }
        }

        Inputs = inputs.ToArray();
    }

    private void AssignValue(InputWrapper wrapper, Answer answer)
    {
        if (answer.GetType().Equals(typeof(TextAnswer)))
            wrapper.TextValue = ((TextAnswer)answer).Value;
        else if (answer.GetType().Equals(typeof(DoubleAnswer)))
            wrapper.Value = ((DoubleAnswer)answer).Value;
        else
            wrapper.Value = ((IntegerAnswer)answer).Value;
    }

    private async Task InitialSendInputs(int progressId)
    {
        try
        {
            Progress = await _HandleFormService.GetProgress(FormId, progressId);
            await SendInputs();
        }
        catch (Exception e)
        {
            await SendMessageAsync(false, e.Message);
        }
    }

    private async Task InitialCalculatePrice()
    {
        Formula = await _HandleFormulaService.GetFormula(FormId);
        await SetPaths();

        var price = _HandleFormulaService.EvaluateFormula(Formula, Inputs);
        await socket.SendAsync(SendPriceResponse(price), WebSocketMessageType.Binary, true, CancellationToken.None);
    }

    private async Task SetPaths()
    {
        var formulaInputPaths = await _HandleFormulaService.GetInputPaths(FormId);

        if (formulaInputPaths.Paths is not null)
        {
            foreach (var path in formulaInputPaths.Paths)
            {
                var input = Inputs.FirstOrDefault(c => c.Input.Id == path.InputId);
                if (input is not null)
                    input.Path = path.Path;
            }
        }
    }
    
    public async Task CheckInputValueValidity(InputValueRequest request)
    {
        try
        {
            var currentInput = Inputs[request.Index];

            if (_HandleFormService.IsInputValueValid(currentInput, request.NumValue, request.TextValue))
                _ = SendDependentInputs(currentInput);

            _ = SendInputValidityResponse(currentInput);
            _ = CalculatePrice(currentInput.Input.Id);
        }
        catch (Exception e)
        {
            await SendMessageAsync(false, "Error occurred when checking input validity. Message : " + e);
        }
    }


    public async Task NextStep()
    {
        if (await CheckUnchangedInputs())
        {
            await _HandleFormService.SaveValues(Inputs, Progress.Id);
            var nextStep = await _HandleFormService.GetNextStep(Progress.StepId);

            if (nextStep is null)
            {
                await SendMessageAsync(true, "Form Successfully submitted.");
                return;
            }

            Progress.StepId = nextStep.Value;
        }
    }

    public async Task PreviousStep()
    {
        var previousStep = await _HandleFormService.GetPreviousStep(Progress.StepId);
        if (previousStep is null)
        {
            await SendMessageAsync(false, "This is the first step.");
            return;
        }
        else
        {
            await SendMessageAsync(true, "Step change valid.");
            Progress.StepId = previousStep.Value;
        }

        _ = _HandleFormService.SaveValues(Inputs, Progress.Id);
        _ = SendInputs();
    }

    private async Task<bool> CheckUnchangedInputs()
    {
        var unchangedInputs = _HandleFormService.CheckUnchangedInputs(Inputs);

        if (unchangedInputs is null)
            return true;

        foreach (var invalidInput in unchangedInputs)
        {
            var response = _HandleFormService.GenerateInputValidityResponse(invalidInput);
            var buffer = SerializeProto(ResponseType.Message, response);
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        return false;
    }
    
    private async Task CalculatePrice(int inputId)
    {
        var value = await _HandleFormulaService.EvaluateFormula(Formula, Inputs, inputId);
        await socket.SendAsync(SendPriceResponse(value), WebSocketMessageType.Binary, true, CancellationToken.None);
    }
    
    private async Task SendDependentInputs(InputWrapper currentInput)
    {
        foreach (var condition in await _HandleFormService.GetDependentInputConditions(currentInput.Input.Id))
        {
            if (_HandleFormService.CheckCondition(condition.Requirements, condition.DependentInput, Inputs))
            {
                var input = FindInputWithId(condition.DependentInput);
                if (!input.Input.IsVisible)
                {
                    input.Input.IsVisible = true;
                    var response = _HandleFormService.GenerateInputResponse(input);
                    var buffer = SerializeProto(ResponseType.Input, response);
                    await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                }
            }
        }
    }

    private byte[] SendPriceResponse(double value)
    {
        var response = new PriceResponse
        {
            Price = value,
        };
        var buffer = SerializeProto(ResponseType.Price, response);
        return buffer;
    }

    private async Task SendInputValidityResponse(InputWrapper currentInput)
    {
        var response = _HandleFormService.GenerateInputValidityResponse(currentInput);
        var buffer = SerializeProto(ResponseType.InputInvalid, response);
        await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
    }


    private InputWrapper FindInputWithId(int inputId)
    {
        return Inputs.FirstOrDefault(r => r.Input.Id == inputId) ??
               throw new Exception($"Input with Id {inputId} was not found.");
    }

    private async Task SendMessageAsync(bool success, string message)
    {
        var response = new ValidityResponse { Message = message, Valid = success };
        var buffer = SerializeProto(ResponseType.Message, response);
        await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
    }

    private static byte[] SerializeProto(ResponseType type, Object obj)
    {
        byte[] method = new byte[1];

        byte[] buffer;
        int size;
        switch (type)
        {
            case ResponseType.Message:
            {
                method[0] = (int)ResponseType.Message;
                size = ((ValidityResponse)obj).CalculateSize();
                buffer = new byte[size];
                var temp = new CodedOutputStream(buffer);
                ((ValidityResponse)obj).WriteTo(temp);
                break;
            }
            case ResponseType.Input:
            {
                method[0] = (int)ResponseType.Input;
                size = ((InputResponse)obj).CalculateSize();
                buffer = new byte[size];
                var temp = new CodedOutputStream(buffer);
                ((InputResponse)obj).WriteTo(temp);
                break;
            }
            case ResponseType.StepChangeValid:
            {
                method[0] = (int)ResponseType.StepChangeValid;
                size = ((ValidityResponse)obj).CalculateSize();
                buffer = new byte[size];
                var temp = new CodedOutputStream(buffer);
                ((ValidityResponse)obj).WriteTo(temp);
                break;
            }
            case ResponseType.Price:
            {
                method[0] = (int)ResponseType.Price;
                size = ((PriceResponse)obj).CalculateSize();
                buffer = new byte[size];
                var temp = new CodedOutputStream(buffer);
                ((PriceResponse)obj).WriteTo(temp);
                break;
            }
            case ResponseType.InputInvalid:
            {
                method[0] = (int)ResponseType.InputInvalid;
                size = ((InputInvalidResponse)obj).CalculateSize();
                buffer = new byte[size];
                var stream = new CodedOutputStream(buffer);
                ((InputInvalidResponse)obj).WriteTo(stream);
                break;
            }
            default:
            {
                method[0] = (int)ResponseType.Message;
                ((ValidityResponse)obj).Message = "Invalid Response Type.";
                size = ((ValidityResponse)obj).CalculateSize();
                buffer = new byte[size];
                var stream = new CodedOutputStream(buffer);
                ((PriceResponse)obj).WriteTo(stream);
                break;
            }
        }

        var result = new byte[1 + size];
        Buffer.BlockCopy(method, 0, result, 0, 1);
        Buffer.BlockCopy(buffer, 0, result, 1, buffer.Length);
        return result;
    }
}