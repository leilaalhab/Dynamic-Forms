using System;
namespace DynamicForms.Dtos.Step
{
	public class UpdateStepDto
	{
		public int Id { get; set; }
		public int Order { get; set; }
		public string Label { get; set; } = String.Empty;
	}
}

