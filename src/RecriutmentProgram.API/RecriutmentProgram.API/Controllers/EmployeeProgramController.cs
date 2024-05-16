using Microsoft.AspNetCore.Mvc;
using RecruitmentProgram.Applications.Dtos;
using RecruitmentProgram.Applications.Interfaces;

namespace RecriutmentProgram.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProgramController : Controller
    {
        private readonly IEmployeeProgramRepository _employeeProgramRepository;

        public EmployeeProgramController(IEmployeeProgramRepository employeeProgramRepository)
        {
            _employeeProgramRepository = employeeProgramRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProgramOrUpdate(CreateEmployeeProgramDto input)
        {
            var result = await _employeeProgramRepository.CreateOrEditProgram(input);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult> GetProgramAndQuestions(string id)
        {
            var result = await _employeeProgramRepository.GetProgramAsync(id);
            return Ok(result);
        }
    }
}
