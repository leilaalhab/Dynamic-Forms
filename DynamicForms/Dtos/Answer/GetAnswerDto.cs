using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Dtos.Answer
{
    public class GetAnswerDto
    {
        public int Id { get; set; }
        public int ProgressId { get; set; }
        public int InputId { get; set; }
        public string TextValue { get; set; } = string.Empty;
        public float NumberValue { get; set; }

    }
}