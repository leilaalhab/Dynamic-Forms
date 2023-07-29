using System;
using DynamicForms.Filter;

namespace DynamicForms.Services.ChoiceService
{
	public interface IChoiceService
	{
        public Task<PagedServiceResponse<GetChoiceDto>> GetAllChoices(PaginationFilter filter);
		public Task<ServiceResponse<GetChoiceDto>> GetChoice(int Id);
        public Task<ServiceResponse<GetChoiceDto>> AddChoice(AddChoiceDto newChoice);
        public Task<ServiceResponse<GetChoiceDto>> UpdateChoice(UpdateChoiceDto updatedChoice);
        public Task<ServiceResponse<int>> DeleteChoice(int Id);
    }
}

