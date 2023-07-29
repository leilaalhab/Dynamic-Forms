using System;

namespace DynamicForms.Dtos.Form
{
	public class GetFormDto
	{
		
		public int Id { get; set; }
		public string Name { get; set; } = String.Empty;
		public List<GetStepDto>? Steps { get; set; }
	
	}
}

