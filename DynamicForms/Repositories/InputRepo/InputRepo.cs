using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Filter;

namespace DynamicForms.Repositories.InputRepo
{
    public class InputRepo : IInputRepo
    {
        private readonly DataContext _context;

        public InputRepo(DataContext context)
        {
            _context = context;
        }
        public Task<Input> AddInput(AddInputDto newInput)
        {
            throw new NotImplementedException();
        }

        public void DeleteInput(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Input>> GetAllInputs(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Input?> GetInput(int Id)
        {
            return await _context.Inputs.Include(c => c.Choices).FirstOrDefaultAsync(c => c.Id == Id);

        }

        public Task<Input> UpdateInput(UpdateInputDto updatedInput)
        {
            throw new NotImplementedException();
        }
    }
}