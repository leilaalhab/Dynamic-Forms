using System;

namespace DynamicForms.Dtos.Step
{
	public class GetStepDto
	{
        public int Id { get; set; }
        public int Order { get; set; }
        public string Label { get; set; } = string.Empty;
        
    }
}