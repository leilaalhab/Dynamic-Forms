using System;
using DynamicForms.Models;

namespace DynamicForms.Dtos.Input
{
	public class AddInputDto
	{
        public int StepId { get; set; }
        public string? Label { get; set; }
        public string? Placeholder { get; set; }
        public InputType InputType { get; set; }
	}
}

