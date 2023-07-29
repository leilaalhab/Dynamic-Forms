using System;
namespace DynamicForms.Models
{
	public class Form
	{
		public int Id { get; set; }
		public string Name { get; set; } = String.Empty;
		public List<Step>? Steps { get; set; }
		//public List<Answer> Answers { get; set; }
	}
}

