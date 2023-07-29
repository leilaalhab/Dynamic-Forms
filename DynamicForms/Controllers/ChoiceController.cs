using System;
using DynamicForms.Filter;
using DynamicForms.Services.ChoiceService;

namespace DynamicForms.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class ChoiceController : ControllerBase
	{
		private readonly IChoiceService _ChoiceService;

		public ChoiceController(IChoiceService choiceService)
		{
			_ChoiceService = choiceService;
		}

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<PagedServiceResponse<GetChoiceDto>>> GetAllChoices([FromQuery] PaginationFilter filter)
        {
            var response = await _ChoiceService.GetAllChoices(filter);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<GetChoiceDto>>> GetChoice(int Id)
        {
            var response = await _ChoiceService.GetChoice(Id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetChoiceDto>>> AddChoice(AddChoiceDto newChoice)
        {
            var response = await _ChoiceService.AddChoice(newChoice);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetChoiceDto>>> UpdateChoice(UpdateChoiceDto updatedChoice)
        {
            var response = await _ChoiceService.UpdateChoice(updatedChoice);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteChoice(int Id)
        {
            var response = await _ChoiceService.DeleteChoice(Id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}