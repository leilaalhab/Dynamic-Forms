using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models.Answers
{
    public abstract class Answer
    {
        public int Id { get; set; }
        public int InputId { get; set; }
        public int ProgressId { get; set; }
        
    }
}