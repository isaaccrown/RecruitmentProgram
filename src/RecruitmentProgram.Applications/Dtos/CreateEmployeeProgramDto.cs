using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentProgram.Applications.Dtos
{
    public class CreateEmployeeProgramDto
    {
        public string Id { get; set; }

        public string ProgramTitle { get; set; }

        public string ProgramDescription { get; set; }

        public List<CustomQuestionDto> Questions { get; set; }
    }
}
