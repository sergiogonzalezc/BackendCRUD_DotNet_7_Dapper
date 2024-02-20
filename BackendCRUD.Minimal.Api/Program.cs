using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Services;
using BackendCRUD.Common;
using BackendCRUD.Infraestructure.Repository;
using BackendCRUD.Minimal.Api.Endpoints;
using BackendCRUD.Minimal.Api.Model;
using Carter;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NLog;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen();


//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();



string envName = builder.Environment.EnvironmentName;

if (envName != "Development" && envName != "qa")
{
    builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();
}

// Add services to the container.
builder.Services.AddScoped<IMemberApplication, MembersApplication>();
builder.Services.AddScoped<IMemberRepository, MemberEFRepository>();
builder.Services.AddScoped(typeof(IMemberApplication), typeof(MembersApplication));
builder.Services.AddScoped(typeof(IMemberRepository), typeof(MemberEFRepository));

builder.Services.AddCors(options =>
{
    options.AddPolicy("localhost", builder =>
    {
        builder.WithOrigins("http://localhost:3000",
                            "http://localhost:3001",
                            "http://localhost:3002",
                            "http://localhost:3003")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithExposedHeaders("content-disposition");
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ErrorHandlingFilterAttribute>();
});

//builder.Services.AddMediatR(typeof(Program).Assembly);
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
});

builder.Services.AddOptions();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //Solo habilitado en producción
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.UseHsts();
}

// define culture spanish CL
var cultureInfo = new CultureInfo("es-CL");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultureInfo),
    SupportedCultures = new List<CultureInfo>
                {
                    cultureInfo,
                },
    SupportedUICultures = new List<CultureInfo>
                {
                    cultureInfo,
                }
});

app.UseCors("localhost");
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler("/error");
app.UseRouting();
app.UseResponseCaching();
app.UseHttpsRedirection();
app.MapControllers();

app.MapCarter();

// SGC - Asigna la version del Assembly al Log4Net
Assembly thisApp = Assembly.GetExecutingAssembly();

AssemblyName name = new AssemblyName(thisApp.FullName);

// Identifica la versión del ensamblado
GlobalDiagnosticsContext.Set("VersionApp", name.Version.ToString());

// Identifica el nombre del ensamblado para poder identificar el ejecutable
GlobalDiagnosticsContext.Set("AppName", name.Name);

// Obtiene el process ID
Process currentProcess = Process.GetCurrentProcess();

GlobalDiagnosticsContext.Set("ProcessID", "PID " + currentProcess.Id.ToString());

ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "===== INICIO API [BackendCRUD.Api] =====");

app.Run();
