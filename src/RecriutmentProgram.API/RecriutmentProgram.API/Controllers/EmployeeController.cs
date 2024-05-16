using Microsoft.AspNetCore.Mvc;
using RecruitmentProgram.Applications.Dtos;
using RecruitmentProgram.Applications.Interfaces;

namespace RecriutmentProgram.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeApplicationRepository _employeeApplicationRepository;

        public EmployeeController(IEmployeeApplicationRepository employeeApplicationRepository)
        {
            _employeeApplicationRepository = employeeApplicationRepository;
        }


        [HttpPost]
        public async Task<ActionResult> Apply(EmployeeApplicationDto input)
        {
            var result = await _employeeApplicationRepository.Apply(input.ProgramId, input);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateApplication(EmployeeApplicationDto input)
        {
            var result = await _employeeApplicationRepository.UpdateApplication(input);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public ActionResult GetEmployeeApplicationById(Guid id)
        {
            return Ok();
        }
    }
}
