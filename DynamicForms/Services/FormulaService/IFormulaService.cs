using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Filter;


namespace DynamicForms.Services.FormulaService
{
    public interface IFormulaService
    {
        public Task<ServiceResponse<GetFormulaDto>> GetFormula(string Id);
        public Task<ServiceResponse<FormulaTree>> GetFormulaWithFormId(int FormId);
        public Task<ServiceResponse<GetFormulaDto>> AddFormula(AddFormulaDto newFormula);
        public Task<ServiceResponse<GetFormulaDto>> UpdateFormula(UpdateFormulaDto updatedFormula);
        public Task<ServiceResponse<int>> DeleteFormula(string Id);
        Task AddInputPaths(FormulaTree tree);
        Task<FormulaInputPaths> GetInputPaths(int formId);
    }
}