﻿using System;
using DynamicForms.Filter;

namespace DynamicForms.Services.StepService
{
	public interface IStepService
	{
        public Task<PagedServiceResponse<GetStepDto>> GetAllSteps(PaginationFilter filter);
        public Task<ServiceResponse<Step>> GetStep(int Id);
        public Task<ServiceResponse<Step>> GetStepWithForm(int formId, int order);
        public Task<ServiceResponse<GetStepDto>> AddStep(AddStepDto newStep);
        public Task<ServiceResponse<GetStepDto>> UpdateStep(UpdateStepDto updatedStep);
        public Task<ServiceResponse<int>> DeleteStep(int Id);
    }
}