using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Dtos.Progress;

namespace DynamicForms.Services.ProgressService
{
    public interface IProgressService
    {
        public Task<ServiceResponse<Progress>> GetProgress(int Id);
        public Task<ServiceResponse<Progress>> AddProgress(AddProgressDto newProgress);
    }
}