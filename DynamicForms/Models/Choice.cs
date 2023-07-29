using System;
namespace DynamicForms.Models
{
	public class Choice
	{
        public int Id { get; set; }
        public Input? Input { get; set; }
		public int InputId { get; set; }
		public string? Label { get; set; }
	}
}

