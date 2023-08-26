using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class FormulaInputPaths
    {
        public required int FormId { get; set; }
        public InputPath[]? Paths { get; set; }
    }
}