using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class InputWrapper
    {
        public required Input Input { get; set; }
        public ErrorType Error { get; set; } = ErrorType.NoError;
        public double ErrorValue { get; set; }
        public double Value { get; set; }
        public string TextValue { get; set; } = string.Empty;
        public int Index { get; set; }
        public bool Interacted { get; set; } = false;
    }
}