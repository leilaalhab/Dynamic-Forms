using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Services.HandleFormulaService
{
    public interface IHandleFormulaService
    {
        Task<FormulaTree> GetFormula(int formId);
        double EvaluateFormula(FormulaTree formula, InputWrapper[] inputs);
        Task<double> EvaluateFormula(FormulaTree formula, InputWrapper[] inputs, int InputId);
        Task<FormulaInputPaths> GetInputPaths(int formId);

    }
}