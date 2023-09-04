using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class Condition
    {
        public int Id { get; set; }
        public required List<Requirement> Requirements { get; set; }
        public int DependentInput { get; set; }
        
    }
}