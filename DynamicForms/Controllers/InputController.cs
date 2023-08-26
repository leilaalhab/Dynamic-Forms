using System;
using DynamicForms.Filter;
using DynamicForms.Services.InputService;

namespace DynamicForms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class InputController : ControllerBase
	{
        private readonly IInputService _InputService;

        public InputController(IInputService inputService)
        {
            _InputService = inputService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<PagedServiceResponse<GetInputDto>>> GetAllInputs([FromQuery] PaginationFilter filter)
        {
            var response = await _InputService.GetAllInputs();
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
        public async Task<ActionResult<ServiceResponse<GetInputDto>>> GetInput(int Id)
        {
            var response = await _InputService.GetInput(Id);
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
        public async Task<ActionResult<ServiceResponse<GetInputDto>>> AddInput(AddInputDto newInput)
        {
            var response = await _InputService.AddInput(newInput);
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
        public async Task<ActionResult<ServiceResponse<GetInputDto>>> UpdateInput(UpdateInputDto updatedInput)
        {
            var response = await _InputService.UpdateInput(updatedInput);
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
        public async Task<ActionResult<ServiceResponse<int>>> DeleteInput(int Id)
        {
            var response = await _InputService.DeleteInput(Id);
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