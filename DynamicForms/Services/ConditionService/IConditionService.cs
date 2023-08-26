using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Services.ConditionService
{
    public interface IConditionService
    {
        public Task<List<Condition>> GetConditionsWithInput(int InputId);
    }
}