using DynamicForms.Models.Answers;
using Formpackage;
using InputValueRequest = Formpackage.InputValueRequest;

namespace DynamicForms.Services.HandleFormService
{
    public interface IHandleFormService
    {
        Task<bool>  DoesFormExist(int formId);
        Task<Progress> GetProgress(int formId, int progressId);
        List<InputWrapper>? CheckUnchangedInputs(InputWrapper[] inputs);
        bool IsInputValueValid(InputWrapper input, double? requestValue, string? requestText);
        public bool CheckCondition(List<Requirement> req, int dependentInput, InputWrapper[] inputs);
        Task<List<Condition>> GetDependentInputConditions(int InputId);
        InputInvalidResponse GenerateInputValidityResponse(InputWrapper input);
        InputResponse GenerateInputResponse(InputWrapper input);
        Task<int?> GetNextStep(int stepId);
        Task<int?> GetPreviousStep(int stepId);
        Task SaveValues(InputWrapper[] inputs, int progressid);
        Task<List<Answer>?> GetAnswers(int progressId);
        Task<List<Input>> GetFormInputs(Progress progress);
        InputandValueResponse GenerateInputAndValueResponse(InputWrapper input);
        public bool ConditionsMet(InputWrapper input);
    }
}