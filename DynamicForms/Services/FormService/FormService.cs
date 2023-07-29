using System;
using DynamicForms.Filter;

namespace DynamicForms.Services.FormService
{
	public class FormService : IFormService
	{
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FormService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedServiceResponse<GetFormDto>> GetAllForms(PaginationFilter filter)
        {
            var response = new PagedServiceResponse<GetFormDto>();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            try
            {
                response.TotalRecords = await _context.Forms.CountAsync();
                var pagedData = await _context.Forms.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize).ToListAsync();
                response.Data = _mapper.Map<List<GetFormDto>>(pagedData);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetFormDto>> GetForm(int Id)
        {
            var response = new ServiceResponse<GetFormDto>();
            try
            {
                var form = await _context.Forms.FirstOrDefaultAsync(c => c.Id == Id);

                if (form is null)
                {
                    response.Success = false;
                    response.Message = $"Form with id {Id} was not found.";
                    return response;
                }

                response.Data = _mapper.Map<GetFormDto>(form);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetFormDto>> AddForm(AddFormDto newForm)
        {
            var response = new ServiceResponse<GetFormDto>();
            try
            {
                var form = new Form
                {
                    Name = newForm.Name
                };

                await _context.Forms.AddAsync(form);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetFormDto>(form);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetFormDto>> UpdateForm(UpdateFormDto updatedForm)
        {
            var response = new ServiceResponse<GetFormDto>();
            try
            {
                var form = await _context.Forms.FirstOrDefaultAsync(c => c.Id == updatedForm.Id);

                if (form is null)
                {
                    response.Success = false;
                    response.Message = $"Form with id {updatedForm.Id} was not found.";
                    return response;
                }

                form.Name = updatedForm.Name;

                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetFormDto>(form);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteForm(int Id)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var form = await _context.Forms.FirstOrDefaultAsync(c => c.Id == Id);

                if (form is null)
                {
                    response.Success = false;
                    response.Message = $"Form with id {Id} was not found.";
                    return response;
                }

                _context.Forms.Remove(form);
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