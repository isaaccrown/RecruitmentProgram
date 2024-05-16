using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Moq;
using RecriutmentProgram.Domain.Entities;
using RecriutmentProgram.Infrastructure;
using RecruitmentProgram.Applications.Dtos;

namespace RecruitmentProgramTest
{
    public class EmployeeApplicationRepositoryTests
    {
        private readonly IConfiguration _configuration = Mock.Of<IConfiguration>();
        private readonly CosmosClient _client = Mock.Of<CosmosClient>();


        [Fact]
        public void ApplyApplication_Should_Throw_Exception_When_Program_Id_Is_Empty()
        {
            //Arrange
            var input = new EmployeeApplicationDto
            {
                ProgramId = null,
                EmployeeAnswers =
                [
                    new()
                    {
                        QuestionDescription = "Favourite colour",
                        Answer = "Blue"
                    }
                ]
            };
            var applicationRepository = new EmployeeApplicationRepository(_client, _configuration);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await applicationRepository.Apply(input.ProgramId, input));
        }

    }
}