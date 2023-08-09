using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Dtos.Formula
{
    public class GetFormulaDto
    {
        public string Id { get; set; } = string.Empty;
        public required Node Root {get; set;}
    }
}