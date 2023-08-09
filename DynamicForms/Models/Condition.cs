using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class Condition
    {
        public int Id { get; set; }
        public Requirement Requirement { get; set; }
        public int RequirementId { get; set; }
        public int DependentInput { get; set; }
        
    }
}