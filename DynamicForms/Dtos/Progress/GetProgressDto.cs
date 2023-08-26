using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Dtos.Progress
{
    public class GetProgressDto
    {
        public int Id { get; set; }
        public GetFormDto formData { get; set; }
    }
}