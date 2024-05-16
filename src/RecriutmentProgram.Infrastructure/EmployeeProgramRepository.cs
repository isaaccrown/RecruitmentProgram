using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using RecriutmentProgram.Domain.Entities;
using RecruitmentProgram.Applications.Dtos;
using RecruitmentProgram.Applications.Interfaces;

namespace RecriutmentProgram.Infrastructure
{

    public class EmployeeProgramRepository : IEmployeeProgramRepository
    {
        private readonly IConfiguration _configuration;

        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly Container _applicationContainer;

        public EmployeeProgramRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            _cosmosClient = cosmosClient;
            _configuration = configuration;
            var databaseName = _configuration["CosmosDb:DatabaseName"];
            var employeeEpplication = "EmployeeApplication";
            _applicationContainer = _cosmosClient.GetContainer(databaseName, employeeEpplication);
            var applicationProgram = "EmployeeProgram";
            _container = _cosmosClient.GetContainer(databaseName, applicationProgram);
        }

        public async Task<EmployeeProgram> CreateOrEditProgram(CreateEmployeeProgramDto programDto)
        {
            try
            {
                if (string.IsNullOrEmpty(programDto.Id))
                {
                    return await CreateProgram(programDto);
                }
                else
                {
                    return await UpdateProgram(programDto);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task<EmployeeProgram> UpdateProgram(CreateEmployeeProgramDto programDto)
        {
            var existingItem = await GetExistingProgram(programDto.Id);
            existingItem.ProgramTitle = programDto.ProgramTitle;
            existingItem.ProgramDescription = programDto.ProgramDescription;
            existingItem.Questions = MapQuestions(programDto.Questions);

            await _container.ReplaceItemAsync(existingItem, existingItem.Id, new PartitionKey(existingItem.Id));
            return existingItem;
        }

        private async Task<EmployeeProgram> CreateProgram(CreateEmployeeProgramDto programDto)
        {
            var program = new EmployeeProgram
            {
                ProgramTitle = programDto.ProgramTitle,
                ProgramDescription = programDto.ProgramDescription,
                Questions = MapQuestions(programDto.Questions)
            };

            var response = await _container.CreateItemAsync(program, new PartitionKey(program.Id));
            return response.Resource;
        }

        private async Task<EmployeeProgram> GetExistingProgram(string programId)
        {
            var result = await _container.ReadItemAsync<EmployeeProgram>(programId, new PartitionKey(programId));
            return result?.Resource ?? throw new Exception("Program does not exist");
        }

        private List<CustomQuestion> MapQuestions(List<CustomQuestionDto> questionDtos)
        {
            if (questionDtos == null)
                return null;

            return questionDtos.Select(x => new CustomQuestion
            {
                ProgramId = x.ProgramId,
                QuestionDescription = x.QuesttionDescription,
                FieldType = x.FieldType,
                Answer = x.Answer,
                IsHidden = x.IsHidden,
                IsInternal = x.IsInternal,
                IsDropDown = x.IsDropDown,
                IsRequired = x.IsRequired,
                DropdownOptions = MapOptions(x.DropdownOptions)
            }).ToList();
        }

        private ICollection<Options> MapOptions(List<OptionsDto> optionDtos)
        {
            if (optionDtos == null)
                return null;

            return optionDtos.Select(y => new Options
            {
                QuestionId = y.QuestionId,
                OptionName = y.OptionName
            }).ToList();
        }

        public async Task<CreateEmployeeProgramDto> GetProgramAsync(string id)
        {
            var response = await _container.ReadItemAsync<EmployeeProgram>(id, new PartitionKey(id));
            var resource = response.Resource;
            return new CreateEmployeeProgramDto
            {
                Id = resource.Id,
                ProgramTitle = resource.ProgramTitle,
                ProgramDescription = resource.ProgramDescription,
                Questions = resource.Questions.Select(x => new CustomQuestionDto
                {
                    Id = x.Id,
                    ProgramId = x.ProgramId,
                    QuesttionDescription = x.QuestionDescription,
                    FieldType = x.FieldType,
                    IsRequired = x.IsRequired,
                    IsInternal = x.IsInternal,
                    IsDropDown = x.IsDropDown,
                    IsHidden = x.IsHidden,
                    DropdownOptions = x.DropdownOptions.Select(y => new OptionsDto
                    {
                        QuestionId = y.QuestionId,
                        OptionName = y.OptionName
                    }).ToList()
                }).ToList()
            };
        }
    }
}
