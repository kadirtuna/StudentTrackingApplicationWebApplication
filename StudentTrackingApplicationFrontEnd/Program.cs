global using StudentTrackingApplicationReal.Shared.Models;
global using StudentTrackingApplicationBackEnd.DTO;
global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
using StudentTrackingApplicationFrontEnd.Services.TeacherService;
using StudentTrackingApplicationFrontEnd.Services.ManagerService;
using StudentTrackingApplicationFrontEnd.Services.StudentService;
using StudentTrackingApplicationFrontEnd.Services.CourseService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudentTrackingApplicationFrontEnd;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7105") });
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

