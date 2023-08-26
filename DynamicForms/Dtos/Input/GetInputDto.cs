using System;

namespace DynamicForms.Dtos.Input
{
	public class GetInputDto
	{
        public int Id { get; set; }
        public string? Label { get; set; }
        public string? Placeholder { get; set; }
        public int Order { get; set; }
        public InpType InputType { get; set; }
        public List<GetChoiceDto>? Choices { get; set; }
        public string Value {get; set;} = "0";
        public ErrorType? errorType {get; set;}
        public string? errorValue {get; set;} 
    }
}

