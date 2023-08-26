using System;
namespace DynamicForms.Dtos.Input
{
	public class UpdateInputDto
	{
		public int Id { get; set; }
		public int StepId { get; set; }
		public int Order { get; set; }
		public InpType Type { get; set; }
		public string Label { get; set; } = String.Empty;
		public string Placeholder { get; set; } = String.Empty;
	}
}

