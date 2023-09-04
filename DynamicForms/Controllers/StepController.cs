using System;
using DynamicForms.Dtos.Step;
using DynamicForms.Filter;
using DynamicForms.Models;
using DynamicForms.Services.StepService;
using Microsoft.AspNetCore.Mvc;



namespace DynamicForms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StepController : ControllerBase
	{
        private readonly IStepService _StepService;

        public StepController(IStepService stepService)
        {
            _StepService = stepService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<PagedServiceResponse<GetStepDto>>> GetAllSteps([FromQuery] PaginationFilter filter)
        {
            var response = await _StepService.GetAllSteps(filter);
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
        public async Task<ActionResult<ServiceResponse<GetStepDto>>> GetStep(int Id)
        {
            var response = await _StepService.GetStep(Id);
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
        public async Task<ActionResult<ServiceResponse<GetStepDto>>> AddStep(AddStepDto newStep)
        {
            var response = await _StepService.AddStep(newStep);
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
        public async Task<ActionResult<ServiceResponse<GetStepDto>>> UpdateStep(UpdateStepDto updatedStep)
        {
            var response = await _StepService.UpdateStep(updatedStep);
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
        public async Task<ActionResult<ServiceResponse<int>>> DeleteStep(int Id)
        {
            var response = await _StepService.DeleteStep(Id);
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