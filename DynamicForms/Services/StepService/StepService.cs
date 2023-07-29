using System;
using DynamicForms.Filter;
using DynamicForms.Models;

namespace DynamicForms.Services.StepService
{
	public class StepService : IStepService
	{
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public StepService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedServiceResponse<GetStepDto>> GetAllSteps(PaginationFilter filter)
        {
            var response = new PagedServiceResponse<GetStepDto>();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            try
            {
                response.TotalRecords = await _context.Steps.CountAsync();
                var pagedData = await _context.Steps.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize).ToListAsync();
                response.Data = _mapper.Map<List<GetStepDto>>(pagedData);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetStepDto>> GetStep(int Id)
        {
            var response = new ServiceResponse<GetStepDto>();
            try
            {
                var step = await _context.Steps.FirstOrDefaultAsync(c => c.Id == Id);

                if (step is null)
                {
                    response.Success = false;
                    response.Message = $"Step with id {Id} was not found.";
                    return response;
                }

                response.Data = _mapper.Map<GetStepDto>(step);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetStepDto>> AddStep(AddStepDto newStep)
        {
            var response = new ServiceResponse<GetStepDto>();
            try
            {
                var form = await _context.Forms.Include(c => c.Steps).FirstOrDefaultAsync(c => c.Id == newStep.FormId);

                if (form is null)
                {
                    response.Success = false;
                    response.Message = $"Form with id {newStep.FormId} was not found.";
                    return response;
                }

                var step = new Step
                {
                    Label = newStep.Label,
                    FormId = newStep.FormId,
                };

                if (form.Steps is null)
                {
                    step.Order = 1;
                }
                else
                {
                    step.Order = form.Steps.Count + 1;
                }


                await _context.Steps.AddAsync(step);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetStepDto>(step);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetStepDto>> UpdateStep(UpdateStepDto updatedStep)
        {
            var response = new ServiceResponse<GetStepDto>();
            try
            {
                var step = await _context.Steps.FirstOrDefaultAsync(c => c.Id == updatedStep.Id);

                if (step is null)
                {
                    response.Success = false;
                    response.Message = $"Step with id {updatedStep.Id} was not found.";
                    return response;
                }

                step.Label = updatedStep.Label;
                if (step.Order < updatedStep.Order)
                {
                    await _context.Inputs.OrderBy(input => input.Order).Skip(step.Order).Take(updatedStep.Order - step.Order).ExecuteUpdateAsync(c => c.SetProperty(e => e.Order, e => e.Order - 1));
                    step.Order = updatedStep.Order;

                }
                else
                {
                    await _context.Inputs.OrderBy(input => input.Order).Skip(updatedStep.Order - 1).Take(step.Order - updatedStep.Order).ExecuteUpdateAsync(c => c.SetProperty(e => e.Order, e => e.Order + 1));
                    step.Order = updatedStep.Order;
                }

                await _context.Steps.Skip(updatedStep.Order).ExecuteUpdateAsync(c => c.SetProperty(e => e.Order, e => e.Order + 1));
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetStepDto>(step);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteStep(int Id)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var step = await _context.Steps.FirstOrDefaultAsync(c => c.Id == Id);

                if (step is null)
                {
                    response.Success = false;
                    response.Message = $"Step with id {Id} was not found.";
                    return response;
                }

                _context.Steps.Remove(step);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}