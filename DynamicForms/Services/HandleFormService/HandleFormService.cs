using DynamicForms.Services.InputService;
using DynamicForms.Services.ConditionService;
using DynamicForms.Services.ProgressService;
using DynamicForms.Services.AnswerService;
using DynamicForms.Services.FormService;
using DynamicForms.Models.Answers;
using DynamicForms.Dtos.Progress;
using DynamicForms.Services.StepService;
using Formpackage;

namespace DynamicForms.Services.HandleFormService
{
    public class HandleFormService : IHandleFormService
    {
        private readonly IInputService _InputService;
        private readonly IConditionService _ConditionService;
        private readonly IProgressService _ProgressService;
        private readonly IFormService _FormService;
        private readonly IAnswerService _AnswerService;
        private readonly IStepService _StepService;

        public HandleFormService(IInputService inputService, IConditionService conditionService, IProgressService progressService, IFormService formService, IAnswerService answerService, IStepService stepService)
        {
            _InputService = inputService;
            _ConditionService = conditionService;
            _ProgressService = progressService;
            _FormService = formService;
            _AnswerService = answerService;
            _StepService = stepService;
        }

        public async Task<bool> DoesFormExist(int formId)
        {
            var form = (await _FormService.GetForm(formId));
            if (form.Success)
                return true;
            return false;
        }

        public async Task<InputWrapper[]> SetInputs(Progress progress)
        {
            // get the inputs belonging to the form
            // get the progress belonging to this progress id -- if progress does not exist, create new progress
            // set values for the inputs

            var inputsUnwrapped = (await _InputService.GetAllInputsWithStepId(progress.StepId)).Data ?? throw new Exception("Form has no inputs.");
            var inputs = inputsUnwrapped.Select(
                c => new InputWrapper
                {
                    Input = c,
                    Index = c.Order,
                }
            ).ToArray();

            await SetValues(inputs, progress);
            return inputs;
        }

        public async Task<int?> GetPreviousStep(int stepId)
        {
            var step = await _StepService.GetStep(stepId);

            if (step.Data is not null)
                return step.Data.NextStep;
            else
                return null;
        }

        public InputInvalidResponse GenerateInputValidityResponse(InputWrapper input)
        {

            var response = new InputInvalidResponse();
            response.Index = input.Index;
            if (input.Error is not null)
            {
                response.Error = input.Error.Value;
                response.ErrorValue = input.ErrorValue;
            }

            if (input.TextValue is not null)
                response.TextValue = input.TextValue;
            if (input.Value is not null)
                response.NumValue = input.Value.Value;

            return response;
        }

        public InputResponse GenerateInputResponse(InputWrapper input)
        {
            var response = new InputResponse
            {
                Index = input.Index,
                InputType = input.Input.InputType,
                Label = input.Input.Label,
                Placeholder = input.Input.Placeholder,
            };
            if (input.Input.Choices is not null)
                response.Choices.AddRange(MapChoices(input.Input.Choices));

            return response;
        }

        public InputandValueResponse GenerateInputAndValueResponse(InputWrapper input)
        {
            var response = new InputandValueResponse
            {
                Index = input.Index,
                InputType = input.Input.InputType,
                Label = input.Input.Label,
                Placeholder = input.Input.Placeholder,
            };
            if (input.Input.Choices is not null)
                response.Choices.AddRange(MapChoices(input.Input.Choices));

            if (input.Error is not null)
            {
                response.Error = input.Error.Value;
                response.ErrorValue = input.ErrorValue;
            }

            if (input.TextValue is not null)
                response.TextValue = input.TextValue;
            if (input.Value is not null)
                response.NumValue = input.Value.Value;

            return response;
        }

        private List<SendChoice> MapChoices(List<Choice> choices)
        {
            var mappedChoices = new List<SendChoice>();
            foreach (var choice in choices)
            {
                mappedChoices.Add(new SendChoice { Id = choice.Id, Label = choice.Label });
            }

            return mappedChoices;
        }

        public async Task<int?> GetNextStep(int stepId)
        {
            var step = await _StepService.GetStep(stepId);

            if (step.Data is not null)
                return step.Data.NextStep;
            else
                return null;
        }

        public async Task<Progress> GetProgress(int formId, int progressId)
        {
            var progress = await _ProgressService.GetProgress(progressId);

            if (!progress.Success)
                progress = await _ProgressService.AddProgress(new AddProgressDto { FormId = formId });

            if (progress.Data is not null)
                return progress.Data;
            else
                throw new Exception("Internal Server error. Progress not found.");

        }

        public async Task<List<Answer>?> GetAnswers(int progressId)
        {
            return (await _AnswerService.GetAnswersWithProgressId(progressId)).Data;
        }
        private async Task SetValues(InputWrapper[] inputs, Progress progress)
        {
            List<Answer>? answers = (await _AnswerService.GetAnswersWithProgressId(progress.Id)).Data;

            if (answers is null)
                return;

            foreach (var answer in answers)
            {
                var input = inputs.FirstOrDefault(c => c.Input.Id == answer.InputId);
                if (input is not null)
                {
                    input.Input.IsVisible = true;
                    input.Interacted = true;
                    if (answer.GetType().Equals(typeof(TextAnswer)))
                        input.TextValue = ((TextAnswer)answer).Value;
                    else if (answer.GetType().Equals(typeof(DoubleAnswer)))
                        input.Value = ((DoubleAnswer)answer).Value;
                    else
                        input.Value = ((IntegerAnswer)answer).Value;
                }
            }
        }
        
        public List<InputWrapper>? CheckUnchangedInputs(InputWrapper[] inputs)
        {
            var unchangedInputs = inputs.Where(c => c.Interacted == false).ToList();
            List<InputWrapper> invalidInputs = new();
            foreach (var input in unchangedInputs)
            {
                var requirements = input.Input.Requirements;

                if (requirements is null)
                    return null;

                foreach (var req in requirements)
                {
                    if (!CheckRequirement(req, input.Value.Value, input))
                    {
                        invalidInputs.Add(input);
                        break;
                    }
                }
            }
            return invalidInputs;
        }

        public async Task<List<Condition>> GetDependentInputConditions(int inputId)
        {
            return await _ConditionService.GetConditionsWithInput(inputId);
        }

        public bool CheckCondition(List<Requirement> req, int dependentInput, InputWrapper[] inputs)
        {
            foreach (var requirement in req)
            {
                var input = FindInputWithId(inputs, requirement.InputId);
                if (input.Value is not null)
                {
                    if (!CheckRequirement(requirement, input.Value.Value, input))
                        return false;
                }
                else
                {
                    if (!CheckRequirement(requirement, input.TextValue.Length, input))
                        return false;
                } 
            }
   
            return true;
        }

        public bool IsInputValueValid(InputWrapper input, double? requestValue, string? requestText)
        {
            input.Interacted = true;

            if (input.Input.Requirements is null)
                return true;

            double value;
            if (!string.IsNullOrEmpty(requestText))
                value = requestText.Length;
            else
                value = requestValue.Value;

            foreach (var req in input.Input.Requirements)
            {
                if (!CheckRequirement(req, value, input))
                {
                    input.Value = value;
                    return false;
                }
            }

            input.Error = null;
            input.Value = null;
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

        public async Task SaveValues(InputWrapper[] inputs, int progressId)
        {

            var textAnswers = new List<TextAnswer>();
            var doubleAnswers = new List<DoubleAnswer>();
            var intAnswers = new List<IntegerAnswer>();

            foreach (var input in inputs)
            {
                if (input.Interacted)
                {
                    if (input.Input.InputType == InpType.Text)
                    {
                        textAnswers.Add(new TextAnswer
                        {
                            InputId = input.Input.Id,
                            ProgressId = progressId,
                            Value = input.TextValue
                        });
                    }
                    else if (input.Input.InputType == InpType.Float)
                    {
                        doubleAnswers.Add(new DoubleAnswer
                        {
                            InputId = input.Input.Id,
                            ProgressId = progressId,
                            Value = input.Value.Value
                        });
                    }
                    else
                    {
                        intAnswers.Add(new IntegerAnswer
                        {
                            InputId = input.Input.Id,
                            ProgressId = progressId,
                            Value = (int)input.Value,
                        });
                    }
                }

            }
            await _AnswerService.AddAllAnswers(textAnswers, doubleAnswers, intAnswers);
        }

        public async Task<List<Input>> GetFormInputs(Progress progress)
        {
            return (await _InputService.GetAllInputsWithStepId(progress.StepId)).Data ?? throw new Exception("Form has no inputs.");
        }
        
        private InputWrapper FindInputWithId(InputWrapper[] inputs, int inputId)
        {
            return inputs.FirstOrDefault(r => r.Input.Id == inputId) ??
                   throw new Exception($"Input with Id {inputId} was not found.");
        }
    }

}
