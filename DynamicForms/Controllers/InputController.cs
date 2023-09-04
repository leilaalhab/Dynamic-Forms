using System;
using DynamicForms.Filter;
using DynamicForms.Services.InputService;
using Microsoft.AspNetCore.Mvc;


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
            switch (response.StatusCode)
            {
                case 200: return Ok(response);
                case 201: return Created("", response);
                case 400: return BadRequest(response);
                case 404: return NotFound(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<GetInputDto>>> GetInput(int Id)
        {
            var response = await _InputService.GetInput(Id);
            switch (response.StatusCode)
            {
                case 200: return Ok(response);
                case 201: return Created("", response);
                case 400: return BadRequest(response);
                case 404: return NotFound(response);
            }

            return BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetInputDto>>> AddInput(AddInputDto newInput)
        {
            var response = await _InputService.AddInput(newInput);
            switch (response.StatusCode)
            {
                case 200: return Ok(response);
                case 201: return Created("", response);
                case 400: return BadRequest(response);
                case 404: return NotFound(response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetInputDto>>> UpdateInput(UpdateInputDto updatedInput)
        {
            var response = await _InputService.UpdateInput(updatedInput);
            switch (response.StatusCode)
            {
                case 200: return Ok(response);
                case 201: return Created("", response);
                case 400: return BadRequest(response);
                case 404: return NotFound(response);
            }

            return BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteInput(int Id)
        {
            var response = await _InputService.DeleteInput(Id);
            switch (response.StatusCode)
            {
                case 200: return Ok(response);
                case 201: return Created("", response);
                case 400: return BadRequest(response);
                case 404: return NotFound(response);
            }

            return BadRequest(response);
        }


    }
}