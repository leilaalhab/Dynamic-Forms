using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models.Answers
{
    public class TextAnswer : Answer
    {
        public string Value { get; set; } = string.Empty;
    }
}