using System;
namespace DynamicForms.Models
{
	public class Step
	{
        public int Id { get; set; }
        public Form? Form { get; set; }
		public int FormId { get; set; }
		public int Order { get; set; }
		public string Label { get; set; } = string.Empty;
		public List<Input>? Inputs { get; set; }

	}
}

