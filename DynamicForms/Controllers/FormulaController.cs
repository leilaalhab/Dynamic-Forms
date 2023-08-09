using System;
using DynamicForms.Filter;
using DynamicForms.Services.FormulaService;

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
        public async Task<ActionResult<ServiceResponse<GetFormulaDto>>> AddFormula(AddFormulaDto newFormula)
        {
            var response = await _FormulaService.AddFormula(newFormula);
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
        public async Task<ActionResult<ServiceResponse<GetFormulaDto>>> UpdateFormula(UpdateFormulaDto updatedFormula)
        {
            var response = await _FormulaService.UpdateFormula(updatedFormula);
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
        public async Task<ActionResult<ServiceResponse<int>>> DeleteFormula(string Id)
        {
            var response = await _FormulaService.DeleteFormula(Id);
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