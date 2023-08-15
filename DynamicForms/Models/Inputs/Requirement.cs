using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class Requirement
    {
        public int Id { get; set; }
        public int InputId { get; set; }
        public double Value { get; set; }
        public ConditionType Type { get; set; }
    }
}