using System;
using Azure;
using DynamicForms.Filter;
using DynamicForms.Services.ChoiceService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


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
        public async Task<ActionResult<ServiceResponse<GetChoiceDto>>> GetChoice(int Id)
        {
            var response = await _ChoiceService.GetChoice(Id);
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
        public async Task<ActionResult<ServiceResponse<GetChoiceDto>>> AddChoice(AddChoiceDto newChoice)
        {
            var response = await _ChoiceService.AddChoice(newChoice);
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
        public async Task<ActionResult<ServiceResponse<GetChoiceDto>>> UpdateChoice(UpdateChoiceDto updatedChoice)
        {
            var response = await _ChoiceService.UpdateChoice(updatedChoice);
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
        public async Task<ActionResult<ServiceResponse<int>>> DeleteChoice(int Id)
        {
            var response = await _ChoiceService.DeleteChoice(Id);
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