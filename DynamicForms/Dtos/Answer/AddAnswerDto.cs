using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Dtos.Answer
{
    public class AddAnswerDto
    {
        public int InputId { get; set; }
        public int ProgressId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}