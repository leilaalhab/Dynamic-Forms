using System;
using DynamicForms.Filter;

namespace DynamicForms.Services.FormService
{
	public interface IFormService
	{
        public Task<PagedServiceResponse<GetFormDto>> GetAllForms(PaginationFilter filter);
        public Task<ServiceResponse<GetFormDto>> GetForm(int Id);
        public Task<ServiceResponse<GetFormDto>> AddForm(AddFormDto newForm);
        public Task<ServiceResponse<GetFormDto>> UpdateForm(UpdateFormDto updatedForm);
        public Task<ServiceResponse<int>> DeleteForm(int Id);
	}
}

