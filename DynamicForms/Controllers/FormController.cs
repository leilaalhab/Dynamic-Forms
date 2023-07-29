using System;
using DynamicForms.Filter;
using DynamicForms.Services.FormService;
using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class FormController : ControllerBase
	{

        private readonly IFormService _FormService;

        public FormController(IFormService FormService)
        {
            _FormService = FormService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<PagedServiceResponse<GetFormDto>>> GetAllForms([FromQuery] PaginationFilter filter)
        {
            var response = await _FormService.GetAllForms(filter);
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
        public async Task<ActionResult<ServiceResponse<GetFormDto>>> GetForm(int Id)
        {
            var response = await _FormService.GetForm(Id);
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
        public async Task<ActionResult<ServiceResponse<GetFormDto>>> AddForm(AddFormDto newForm)
        {
            var response = await _FormService.AddForm(newForm);
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
        public async Task<ActionResult<ServiceResponse<GetFormDto>>> UpdateForm(UpdateFormDto updatedForm)
        {
            var response = await _FormService.UpdateForm(updatedForm);
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
        public async Task<ActionResult<ServiceResponse<int>>> DeleteForm(int Id)
        {
            var response = await _FormService.DeleteForm(Id);
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