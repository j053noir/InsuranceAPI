using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Utils.Middlewares;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

#region Configure Database
string databaseConnectionString = configuration.GetConnectionString("MongoDatabase");

var client = new MongoClient(databaseConnectionString);

string databaseName = configuration["MongoDatabase:DatabaseName"];

IMongoDatabase database = client.GetDatabase(databaseName);

builder.Services.AddSingleton(database);

builder.Services.Configure<InsuranceAPI.Infrastructure.Models.MongoDatabaseSettings>
                                        (builder.Configuration.GetSection("MongoDatabase"));
#endregion

// Add automapper
builder.Services.AddAutoMapper(typeof(Program));

#region Repositories
builder.Services.AddSingleton<IClientsRepository, ClientsRepository>();
builder.Services.AddSingleton<IContactInformationRepository, ContactInformationRepository>();
builder.Services.AddSingleton<IInspectionsRepository, InspectionsRepository>();
builder.Services.AddSingleton<IInsurancePoliciesRepository, InsurancePoliciesRepository>();
builder.Services.AddSingleton<IPolicyPlansRepository, PolicyPlansRepository>();
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IVehiclesRepository, VehicleRepository>();
builder.Services.AddSingleton<IRefreshTokenRespository, RefreshTokenRepository>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMiddleware<JWTMiddleware>();

app.MapControllers();

app.Run();
