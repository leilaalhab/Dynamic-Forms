using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Repositories.ConditionRepo
{
    public interface IConditionRepo
    {
       public Task<List<Condition>> GetAllConditions(int Id);

    }
}