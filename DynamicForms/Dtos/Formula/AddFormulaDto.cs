using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Dtos.Formula
{
    public class AddFormulaDto
    {
        public int ParentId { get; set; }
        public required Element[] Formula { get; set; }
    }
}