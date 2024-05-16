using RecruitmentProgram.Applications.Dtos;

namespace RecruitmentProgram.Applications.Interfaces
{
    public interface IEmployeeApplicationRepository
    {
        Task<EmployeeApplicationDto> Apply(string programId, EmployeeApplicationDto employeeApplicationDto);
        Task<EmployeeApplicationDto> UpdateApplication(EmployeeApplicationDto input);
    }
}
