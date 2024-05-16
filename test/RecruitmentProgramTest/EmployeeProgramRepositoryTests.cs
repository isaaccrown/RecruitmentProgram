using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Moq;
using RecriutmentProgram.Infrastructure;
using RecruitmentProgram.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentProgramTest
{
    public class EmployeeProgramRepositoryTests
    {

        private readonly IConfiguration _configuration = Mock.Of<IConfiguration>();
        private readonly CosmosClient _client = Mock.Of<CosmosClient>();

        [Fact]
        public void ProgramShouldThrowExceptionWhenQuestionIsZero()
        {
            //Arrange
            var input = new CreateEmployeeProgramDto
            {
                ProgramTitle = "Create Employee Program",
                ProgramDescription = "Description",
                Questions = new List<CustomQuestionDto>()
            };
            var employeeProgram = new EmployeeProgramRepository(_client, _configuration);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await employeeProgram.CreateOrEditProgram(input));
        }
    }
}
