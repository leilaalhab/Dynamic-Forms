using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Services.HandleFormService
{
    public interface IHandleFormService
    {
        bool DoesFormExist(int formId);
        Task<Progress> GetProgress(int formId, int progressId);
        Task<InputWrapper[]> SetInputs(Progress progress);
        List<InputWrapper>? GetUnchangedInputs(InputWrapper[] inputs);
        bool IsInputValid(InputWrapper input, double requestValue, string requestText);
        bool CheckCondition(Requirement req, Request request, InputWrapper input);
        Task<List<Condition>> GetDependentInputConditions(int InputId);
        Response GenerateResponse(InputWrapper input, ResponseType responseType);
        GetInputDto ReturnResponse(InputWrapper input);
        Task<int?> GetNextStep(int stepId);
        Task<int?> GetPreviousStep(int stepId);
        Task SaveValues(InputWrapper[] inputs, int progressid);
    }
}