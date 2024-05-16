using Microsoft.Azure.Cosmos;
using RecriutmentProgram.Infrastructure;
using RecruitmentProgram.Applications.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployeeApplicationRepository, EmployeeApplicationRepository>();
builder.Services.AddScoped<IEmployeeProgramRepository, EmployeeProgramRepository>();

var configuration = builder.Configuration;

builder.Services.AddSingleton((provider) =>
{
    var endpointUri = configuration["CosmosDb:EndpointUri"];
    var primaryKey = configuration["CosmosDb:PrimaryKey"];
    var databaseName = configuration["CosmosDb:DatabaseName"];

    var cosmosClientOpions = new CosmosClientOptions
    {
        ApplicationName = databaseName,
    };

    var loggerFactory = LoggerFactory.Create(option =>
    {
        option.AddConsole();
    });
    var cosmosClient = new CosmosClient(endpointUri, primaryKey, cosmosClientOpions);
    cosmosClient.ClientOptions.ConnectionMode = ConnectionMode.Direct;
    return cosmosClient;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
