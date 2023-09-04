using System;
using DynamicForms.Filter;
using DynamicForms.Services.FormulaService;
using Microsoft.AspNetCore.Mvc;


namespace DynamicForms.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FormulaController : ControllerBase
	{
        private readonly IFormulaService _FormulaService;

        public FormulaController(IFormulaService FormulaService)
        {
            _FormulaService = FormulaService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<GetFormulaDto>>> GetFormula(string Id)
        {
            var response = await _FormulaService.GetFormula(Id);
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
        public async Task<ActionResult<ServiceResponse<GetFormulaDto>>> AddFormula(AddFormulaDto newFormula)
        {
            var response = await _FormulaService.AddFormula(newFormula);
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
        public async Task<ActionResult<ServiceResponse<GetFormulaDto>>> UpdateFormula(UpdateFormulaDto updatedFormula)
        {
            var response = await _FormulaService.UpdateFormula(updatedFormula);
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
        public async Task<ActionResult<ServiceResponse<int>>> DeleteFormula(string Id)
        {
            var response = await _FormulaService.DeleteFormula(Id);
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