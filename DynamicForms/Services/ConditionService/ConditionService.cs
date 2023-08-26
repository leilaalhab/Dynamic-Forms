using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Services.ConditionService
{
    public class ConditionService : IConditionService
    {

        private readonly DataContext _context;

        public ConditionService(DataContext context) {
            _context = context;
        }
        public async Task<List<Condition>> GetConditionsWithInput(int InputId)
        {
            return await _context.Conditions.Include(c => c.Requirement).Where(r => r.Requirement.InputId == InputId).ToListAsync();
        }
    }
}