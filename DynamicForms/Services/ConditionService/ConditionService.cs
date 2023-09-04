
namespace DynamicForms.Services.ConditionService
{
    public class ConditionService : IConditionService
    {

        private readonly DataContext _context;

        public ConditionService(DataContext context) {
            _context = context;
        }
        public async Task<List<Condition>> GetConditionsWithInput(int inputId)
        {
            return  await _context.Conditions.Where(c => c.Requirements.Any(r => r.InputId == inputId)).ToListAsync();

        }
    }
}