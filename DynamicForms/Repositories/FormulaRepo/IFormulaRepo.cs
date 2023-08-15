
namespace DynamicForms.Repositories.FormulaRepo
{
    public interface IFormulaRepo
    {
        public Task<FormulaTree> GetFormulaTree(string _Id);
        public Task<FormulaTree> GetFormulaWithFormId(int Id);
    }
}