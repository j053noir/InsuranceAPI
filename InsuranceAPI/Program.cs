using InsuranceAPI.Infrastructure.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

#region Configure Database
string databaseConnectionString = configuration.GetConnectionString("MongoDatabase");

var client = new MongoClient(databaseConnectionString);

string databaseName = configuration["MongoDatabase:DatabaseName"];

IMongoDatabase database = client.GetDatabase(databaseName);

builder.Services.AddSingleton(database);

builder.Services.Configure<InsuranceAPI.Infrastructure.Models.MongoDatabaseSettings>
                                        (builder.Configuration.GetSection("MongoDatabase"));
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

app.UseAuthorization();

app.MapControllers();

app.Run();
