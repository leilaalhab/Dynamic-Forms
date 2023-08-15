using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public class Element
    {
        public int InputId { get; set; }
        public double Value {get; set;}
        public NodeType Type { get; set; }
    }
}