using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using RecriutmentProgram.Domain.Entities;
using RecruitmentProgram.Applications.Dtos;
using RecruitmentProgram.Applications.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecriutmentProgram.Infrastructure
{
    public class EmployeeApplicationRepository : IEmployeeApplicationRepository
    {

        private readonly CosmosClient _cosmosClient;
        private readonly IConfiguration _configuration;
        private readonly Container _container;
        private readonly Container _applicationContainer;
        public EmployeeApplicationRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            _cosmosClient = cosmosClient;
            _configuration = configuration;
            var databaseName = _configuration["CosmosDb:DatabaseName"];
            var employeeEpplication = "EmployeeApplication";
            _applicationContainer = _cosmosClient.GetContainer(databaseName, employeeEpplication);
            var applicationProgram = "EmployeeProgram";
            _container = _cosmosClient.GetContainer(databaseName, applicationProgram);
        }

        public async Task<EmployeeApplicationDto> Apply(string programId, EmployeeApplicationDto employeeApplication)
        {
            var response = new EmployeeApplicationDto();
            var result = await _container.ReadItemAsync<EmployeeProgram>(programId, new PartitionKey())
                ?? throw new Exception("c");

            var employeeProgram = result.Resource;

            if (employeeProgram != null)
            {
                var application = new EmployeeApplication
                {
                    ProgramId = employeeProgram.Id,
                    EmployeeAnswers = employeeApplication.EmployeeAnswers.Select(x => new EmployeeAnswer
                    {
                        QuestionId = x.QuestionId,
                        QuestionDescription = x.QuestionDescription,
                        Answer = x.Answer
                    }).ToList()
                };
                var addResult = await _applicationContainer.CreateItemAsync(application, new PartitionKey(application.Id));
                var entry = addResult.Resource;

                response = new EmployeeApplicationDto
                {
                    Id = entry.Id,
                    ProgramId = entry.ProgramId,
                    EmployeeAnswers = entry.EmployeeAnswers
                };
            }
            return response;
        }
        public async Task<EmployeeApplicationDto> UpdateApplication(EmployeeApplicationDto input)
        {
            if (string.IsNullOrEmpty(input?.Id))
                throw new ArgumentException("Id cannot be null or empty", nameof(input.Id));

            var result = await _applicationContainer.ReadItemAsync<EmployeeApplication>(input.Id, new PartitionKey(input.Id));
            var existingApplication = result.Resource;

            if (existingApplication == null)
                throw new Exception("Existing application not found");

            existingApplication.EmployeeAnswers = input.EmployeeAnswers;

            var entry = await _applicationContainer.ReplaceItemAsync<EmployeeApplication>(existingApplication, input.Id, new PartitionKey(input.Id));
            var output = entry.Resource;

            return new EmployeeApplicationDto
            {
                Id = output.Id,
                ProgramId = output.ProgramId,
                EmployeeAnswers = output.EmployeeAnswers
            };
        }

    }
}
