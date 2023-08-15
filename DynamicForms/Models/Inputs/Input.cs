using System;
namespace DynamicForms.Models
{
	public class Input
	{
		public int Id { get; set; }
		public Step? Step { get; set; }
		public int StepId { get; set; }
		public int Order { get; set; }
		public string? Label { get; set; }
		public string? Placeholder { get; set; }
		public string Name { get; set; } = string.Empty;
		public InpType InputType { get; set; }
		public bool IsVisible { get; set; }
		public List<Requirement>? Requirements { get; set; }
		public List<Choice>? Choices { get; set; }
		public double DefaultValue { get; set; }

	}
}

