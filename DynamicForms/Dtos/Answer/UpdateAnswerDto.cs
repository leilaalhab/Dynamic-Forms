using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Dtos.Answer
{
    public class UpdateAnswerDto
    {
        public int Id { get; set; }
        public int InputId { get; set; }
        public float NumberValue { get; set; }
        public string TextValue { get; set; } = string.Empty;
    }
}