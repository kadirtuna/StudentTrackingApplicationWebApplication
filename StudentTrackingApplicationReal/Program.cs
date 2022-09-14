using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentTrackingApplicationBackEnd.Controllers;
using StudentTrackingApplicationBackEnd.DIExtensions;
using StudentTrackingApplicationReal.Shared.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Add CORS Security Feature
builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("https://localhost:7110").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
//builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header using he Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//To add the JWT Authenticate to our project, the code below has to be added to Program.cs file.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
           ValidateIssuer = false,
           ValidateAudience = false
       };
    });

//Dependency Injection(DI) for DbContext
builder.Services.AddDbContext<Context>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("StudentTrackingApplicationReal")));

//Remove the feature of cascading delete for Relationships between Attendance and Student
IServiceCollection services = new ServiceCollection();
services.AddControllers();
services.AddRepositories();

var app = builder.Build();


//To use an action in another controller
services.AddMvc().AddControllersAsServices();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

//app.MapStudentEndpoints();

app.Run();
