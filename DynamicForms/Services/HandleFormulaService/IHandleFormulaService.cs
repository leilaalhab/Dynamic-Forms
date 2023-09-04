namespace DynamicForms.Services.HandleFormulaService
{
    public interface IHandleFormulaService
    {
        Task<FormulaTree> GetFormula(int formId);
        double EvaluateFormula(FormulaTree formula, InputWrapper[] inputs);
        Task<double> EvaluateFormula(FormulaTree formula, InputWrapper[] inputs, int inputId);
        Task<FormulaInputPaths> GetInputPaths(int formId);
    }
}