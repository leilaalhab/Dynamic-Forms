using System;

namespace DynamicForms.Dtos.Input
{
	public class GetInputDto
	{
        public int Id { get; set; }
        public int stepId { get; set; }
        public string? Label { get; set; }
        public string? Placeholder { get; set; }
        public int Order { get; set; }
        public InputType InputType { get; set; }
        public List<GetChoiceDto>? Choices { get; set; }
    }
}

