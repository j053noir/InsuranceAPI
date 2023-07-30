using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Infrastructure.Services;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using InsuranceAPI.Utils.Middlewares;
using Microsoft.OpenApi.Models;
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

#region Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Insurance API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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
