using RecriutmentProgram.Domain.Entities;
using RecruitmentProgram.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentProgram.Applications.Interfaces
{
    public interface IEmployeeProgramRepository
    {
        Task<EmployeeProgram> CreateOrEditProgram(CreateEmployeeProgramDto programDto);
        Task<CreateEmployeeProgramDto> GetProgramAsync(string id);
    }
}
