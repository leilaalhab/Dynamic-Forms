

using DynamicForms.Filter;

namespace DynamicForms.Repositories.InputRepo
{
    public interface IInputRepo
    {
        public Task<List<Input>> GetAllInputs(PaginationFilter filter);
        public Task<Input> GetInput(int Id);
        public Task<Input> AddInput(AddInputDto newInput);
        public Task<Input?> UpdateInput(UpdateInputDto updatedInput);
        public void DeleteInput(int Id);
    }
}