using System;
using DynamicForms.Filter;

namespace DynamicForms.Services.InputService
{
	public interface IInputService
	{
        public Task<PagedServiceResponse<GetInputDto>> GetAllInputs(PaginationFilter filter);
        public Task<ServiceResponse<GetInputDto>> GetInput(int Id);
        public Task<ServiceResponse<GetInputDto>> AddInput(AddInputDto newInput);
        public Task<ServiceResponse<GetInputDto>> UpdateInput(UpdateInputDto updatedInput);
        public Task<ServiceResponse<int>> DeleteInput(int Id);
    }
}

