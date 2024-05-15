using RecriutmentProgram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentProgram.Applications.Dtos
{
    public class EmployeeApplicationDto
    {
        public string Id { get; set; }

        //[Required]
        public string ProgramId { get; set; }
        public ICollection<EmployeeAnswer> EmployeeAnswers { get; set; }
    }
}
