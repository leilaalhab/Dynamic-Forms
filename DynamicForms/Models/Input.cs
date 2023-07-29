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
		public string Name { get; set; } = String.Empty;
		public InputType InputType { get; set; }
		public List<Choice>? Choices { get; set; }

	}
}

