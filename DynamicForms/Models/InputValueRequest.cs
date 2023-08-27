using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class InputValueRequest
    {
        public int Index { get; set; }
        public string Value { get; set; } = string.Empty;
        public InpType InpType { get; set; }
    }
}