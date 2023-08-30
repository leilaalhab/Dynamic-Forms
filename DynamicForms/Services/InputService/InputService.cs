using System;
using System.Collections.Generic;
using DynamicForms.Filter;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Services.InputService
{
    public class InputService : IInputService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public InputService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<Input>>> GetAllInputsWithStepId(int StepId)
        {
            var response = new ServiceResponse<List<Input>>();
            try {
                    var Inputs = await _context.Inputs.Include(c => c.Choices).Include(c => c.Requirements).Where(i => i.StepId == StepId).ToListAsync();
                    response.Data = Inputs;
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetInputDto>>> GetAllInputs()
        {
            var response = new ServiceResponse<List<GetInputDto>>();
            //var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            try
            {
                //var list = await _context.Inputs.ToListAsync();
                //response.TotalRecords = list.Count;
                //int skip = (validFilter.PageNumber - 1) * validFilter.PageSize;
                //var pagedData = list.GetRange(skip, skip + validFilter.PageSize);

                //response.TotalRecords = await _context.Inputs.CountAsync();
                //var pagedData = await _context.Inputs.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                //  .Take(validFilter.PageSize).ToListAsync();
                //response.Data = _mapper.Map<List<GetInputDto>>(pagedData);
               
                //var pagedInputs = await _context.Inputs.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                //.Take(validFilter.PageSize).Select(c => new {total_records = _context.Inputs.Count(), t = c }).ToListAsync();
                //response.TotalRecords = pagedInputs.ElementAt(0).total_records;
                var pagedInputs = await _context.Inputs.ToListAsync();
                //List<Input> inputs = pagedInputs.Select(b => b.t).ToList();
                response.Data = _mapper.Map<List<GetInputDto>>(pagedInputs);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetInputDto>> GetInput(int Id)
        {
            var response = new ServiceResponse<GetInputDto>();
            try
            {
                var input = await _context.Inputs.Include(c => c.Choices).FirstOrDefaultAsync(c => c.Id == Id);

                if (input is null)
                {
                    response.Success = false;
                    response.Message = $"Input with id {Id} was not found.";
                    return response;
                }

                response.Data = _mapper.Map<GetInputDto>(input);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetInputDto>> AddInput(AddInputDto newInput)
        {
            var response = new ServiceResponse<GetInputDto>();
            try
            {
                var step = await _context.Steps.Include(c => c.Inputs).FirstOrDefaultAsync(c => c.Id == newInput.StepId);

                if (step is null)
                {
                    response.Success = false;
                    response.Message = $"Step with id {newInput.StepId} was not found.";
                    return response;
                }

                var input = new Input
                {
                    Label = newInput.Label,
                    //InputType = newInput.InputType,
                    Placeholder = newInput.Placeholder,
                    Step = step,
                    StepId = newInput.StepId,
                };

                if (step.Inputs is null)
                {
                    input.Order = 1;
                }
                else
                {
                    input.Order = step.Inputs.Count + 1;
                }

                await _context.Inputs.AddAsync(input);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetInputDto>(input);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetInputDto>> UpdateInput(UpdateInputDto updatedInput)
        {
            var response = new ServiceResponse<GetInputDto>();
            try
            {
                var input = await _context.Inputs.FirstOrDefaultAsync(c => c.Id == updatedInput.Id);

                if (input is null)
                {
                    response.Success = false;
                    response.Message = $"Input with id {updatedInput.Id} was not found.";
                    return response;
                }

                input.Label = updatedInput.Label;
                input.Placeholder = updatedInput.Placeholder;
                //input.InputType = updatedInput.Type;

                if  (input.Order < updatedInput.Order)
                {
                    await _context.Inputs.OrderBy(input => input.Order).Skip(input.Order).Take(updatedInput.Order - input.Order).ExecuteUpdateAsync(c => c.SetProperty(e => e.Order, e => e.Order - 1));
                    input.Order = updatedInput.Order;

                } else
                {
                    await _context.Inputs.OrderBy(input => input.Order).Skip(updatedInput.Order - 1).Take(input.Order - updatedInput.Order).ExecuteUpdateAsync(c => c.SetProperty(e => e.Order, e => e.Order + 1));
                    input.Order = updatedInput.Order;
                }
               
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetInputDto>(input);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteInput(int Id)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var input = await _context.Inputs.FirstOrDefaultAsync(c => c.Id == Id);

                if (input is null)
                {
                    response.Success = false;
                    response.Message = $"Input with id {Id} was not found.";
                    return response;
                }

                _context.Inputs.Remove(input);
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