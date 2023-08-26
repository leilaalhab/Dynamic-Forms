using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Dtos.Progress;

namespace DynamicForms.Services.ProgressService
{
    public class ProgressService : IProgressService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProgressService(DataContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<Progress>> AddProgress(AddProgressDto newProgress)
        {        
            // set the progress stepid to the first step of the form and return that progress 
     

            var response = new ServiceResponse<Progress>();
            try
            {
                var step = await _context.Steps.OrderBy(c => c.Order).FirstOrDefaultAsync(c => c.FormId == newProgress.FormId);

                if (step is null)
                {
                    response.Success = false;
                    response.Message = $"Form with id {newProgress.FormId} was not found.";
                    return response;
                }

                var progress = new Progress
                {
                    StepId = step.Id
                };

                await _context.Progresses.AddAsync(progress);
                await _context.SaveChangesAsync();
                response.Data = progress;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<Progress>> GetProgress(int Id)
        {
            var response = new ServiceResponse<Progress>();
            try
            {
                var progress = await _context.Progresses.FirstOrDefaultAsync(c => c.Id == Id);

                if (progress is null)
                {
                    response.Success = false;
                    response.Message = $"progress with id {Id} was not found.";
                    return response;
                }

                response.Data = progress;
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