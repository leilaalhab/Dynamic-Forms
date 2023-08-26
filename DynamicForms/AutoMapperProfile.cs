//using System;
namespace DynamicForms
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<AddChoiceDto, Choice>();
			CreateMap<Choice, GetChoiceDto>();

            CreateMap<Input, GetInputDto>();
			CreateMap<AddInputDto, Input>();

            CreateMap<Form, GetFormDto>();
            CreateMap<AddFormDto, Form>();

            CreateMap<Step, GetStepDto>();
            CreateMap<AddStepDto, Step>();

			CreateMap<FormulaTree, GetFormulaDto>();
        }
	}
}

