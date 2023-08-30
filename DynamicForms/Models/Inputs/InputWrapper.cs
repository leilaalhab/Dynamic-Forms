using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Formpackage;

namespace DynamicForms.Models
{
    public class InputWrapper
    {
        public required Input Input { get; set; }
        public ErrorType? Error { get; set; }
        public double ErrorValue { get; set; }
        public double? Value { get; set; }
        public string? TextValue { get; set; }
        public int Index { get; set; }
        public bool Interacted { get; set; } = false;
        public bool[]? Path {get; set;}
    }
}