using DynamicForms.Filter;

namespace DynamicForms.Services.ChoiceService
{
    public class ChoiceService : IChoiceService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ChoiceService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedServiceResponse<GetChoiceDto>> GetAllChoices(PaginationFilter filter)
        {
            var response = new PagedServiceResponse<GetChoiceDto>();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            try
            {
                response.TotalRecords = await _context.Choices.CountAsync();
                var pagedData = await _context.Choices.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize).ToListAsync();
                response.Data = _mapper.Map<List<GetChoiceDto>>(pagedData);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetChoiceDto>> GetChoice(int Id)
        {
            var response = new ServiceResponse<GetChoiceDto>();
            try
            {
                var choice = await _context.Choices.FirstOrDefaultAsync(c => c.Id == Id);

                if (choice is null)
                {
                    response.Success = false;
                    response.Message = $"Choice with id {Id} was not found.";
                    return response;
                }

                response.Data = _mapper.Map<GetChoiceDto>(choice);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetChoiceDto>> AddChoice(AddChoiceDto newChoice)
        {
            var response = new ServiceResponse<GetChoiceDto>();
            try
            {
                var input = await _context.Inputs.FirstOrDefaultAsync(c => c.Id == newChoice.InputId);

                if (input is null)
                {
                    response.Success = false;
                    response.Message = $"Input with id {newChoice.InputId} was not found.";
                    return response;
                }

                var choice = new Choice
                {
                    Label = newChoice.Label
                };

                await _context.Choices.AddAsync(choice);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetChoiceDto>(choice);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetChoiceDto>> UpdateChoice(UpdateChoiceDto updatedChoice)
        {
            var response = new ServiceResponse<GetChoiceDto>();
            try
            {
                var choice = await _context.Choices.FirstOrDefaultAsync(c => c.Id == updatedChoice.Id);

                if (choice is null)
                {
                    response.Success = false;
                    response.Message = $"Choice with id {updatedChoice.Id} was not found.";
                    return response;
                }

                choice.Label = updatedChoice.Label;
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetChoiceDto>(choice);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<int>> DeleteChoice(int Id)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var choice = await _context.Choices.FirstOrDefaultAsync(c => c.Id == Id);

                if (choice is null)
                {
                    response.Success = false;
                    response.Message = $"Choice with id {Id} was not found.";
                    return response;
                }

                _context.Choices.Remove(choice);
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