using DynamicForms.Services.InputService;
using DynamicForms.Services.ConditionService;
using DynamicForms.Services.ProgressService;
using DynamicForms.Services.AnswerService;
using DynamicForms.Services.FormService;
using DynamicForms.Models.Answers;
using DynamicForms.Dtos.Progress;
using DynamicForms.Services.StepService;

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
        private readonly IMapper _mapper;

        public HandleFormService(IInputService inputService, IConditionService conditionService, IProgressService progressService, IFormService formService, IAnswerService answerService, IStepService stepService, IMapper mapper)
        {
            _InputService = inputService;
            _ConditionService = conditionService;
            _ProgressService = progressService;
            _FormService = formService;
            _AnswerService = answerService;
            _StepService = stepService;
            _mapper = mapper;
        }

        public bool DoesFormExist(int formId)
        {
            var form = _FormService.GetForm(formId).Result;
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
                    Input = c
                }
            ).ToArray();

            await SetValues(inputs, progress);
            return inputs;
        }

        public async Task<int?> GetPreviousStep(int stepId)
        {
            var Step = await _StepService.GetStep(stepId);

            if (Step.Data is not null)
                return Step.Data.NextStep;
            else
                return null;
        }

        public async Task<int?> GetNextStep(int stepId)
        {
            var Step = await _StepService.GetStep(stepId);

            if (Step.Data is not null)
                return Step.Data.NextStep;
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

        private async Task SetValues(InputWrapper[] inputs, Progress progress)
        {
            List<Answer>? answers = (await _AnswerService.GetAnswersWithProgressId(progress.Id)).Data;

            if (answers is null)
                return;

            foreach (var Answer in answers)
            {
                var input = inputs.FirstOrDefault(c => c.Input.Id == Answer.InputId);
                if (input is not null)
                {
                    input.Input.IsVisible = true;
                    input.Interacted = true;
                    if (Answer.GetType().Equals(typeof(TextAnswer)))
                        input.TextValue = ((TextAnswer)Answer).Value;
                    else if (Answer.GetType().Equals(typeof(DoubleAnswer)))
                        input.Value = ((DoubleAnswer)Answer).Value;
                    else
                        input.Value = ((IntegerAnswer)Answer).Value; // also for option id's
                }
            }
        }

        public List<InputWrapper>? GetUnchangedInputs(InputWrapper[] inputs)
        {
            var UnchangedInputs = inputs.Where(c => c.Interacted == false).ToList();
            List<InputWrapper> InvalidInputs = new();
            foreach (var input in UnchangedInputs)
            {
                var requirements = input.Input.Requirements;

                if (requirements is null)
                    return null;

                foreach (var Req in requirements)
                {
                    if (!CheckRequirement(Req, input.Value, input))
                    {
                        InvalidInputs.Add(input);
                        break;
                    }
                }
            }
            return InvalidInputs;
        }

        public async Task<List<Condition>> GetDependentInputConditions(int InputId)
        {
            return await _ConditionService.GetConditionsWithInput(InputId);
        }

        public bool CheckCondition(Requirement req, Request request, InputWrapper input)
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

        public GetInputDto ReturnResponse(InputWrapper input)
        {
            var response = new GetInputDto
            {
                Id = input.Input.Id,
                Order = input.Input.Order,
                Label = input.Input.Label,
                Placeholder = input.Input.Placeholder,
                Choices = _mapper.Map<List<GetChoiceDto>>(input.Input.Choices),
                Value = input.Value.ToString(),
            };

            if (input.Error != ErrorType.NoError)
            {
                response.errorType = input.Error;
                response.errorValue = input.ErrorValue.ToString();
            }

            return response;
        }

        public Response GenerateResponse(InputWrapper input, ResponseType responseType)
        {
            var res = new Response
            {
                Id = input.Input.Id,
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

        public bool IsInputValid(InputWrapper input, double requestValue, string requestText)
        {
            input.Interacted = true;
            double value;

            if (string.IsNullOrEmpty(requestText))
            {
                value = requestValue;
            }
            else
            {
                value = requestText.Length;
            }

            if (input.Input.Requirements is null)
                return true;


            foreach (var req in input.Input.Requirements)
            {
                if (!CheckRequirement(req, value, input))
                {
                    input.Value = value;
                    return false;
                }
                else
                {
                    input.Error = ErrorType.NoError;
                    input.Value = 0;
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

        public async Task SaveValues(InputWrapper[] inputs, int progressid)
        {

            var textAnswers = new List<TextAnswer>();
            var doubleAnswers = new List<DoubleAnswer>();
            var intAnswers = new List<IntegerAnswer>();

            foreach (var input in inputs)
            {
                if (input.Interacted == true)
                {
                    if (input.Input.InputType == InpType.Text)
                    {
                        textAnswers.Add(new TextAnswer
                        {
                            InputId = input.Input.Id,
                            ProgressId = progressid,
                            Value = input.TextValue
                        });
                    }
                    else if (input.Input.InputType == InpType.Float)
                    {
                        doubleAnswers.Add(new DoubleAnswer
                        {
                            InputId = input.Input.Id,
                            ProgressId = progressid,
                            Value = input.Value
                        });
                    }
                    else
                    {
                        intAnswers.Add(new IntegerAnswer
                        {
                            InputId = input.Input.Id,
                            ProgressId = progressid,
                            Value = (int)input.Value,
                        });
                    }
                }

            }
            await _AnswerService.AddAllAnswers(textAnswers, doubleAnswers, intAnswers);
        }
    }
}