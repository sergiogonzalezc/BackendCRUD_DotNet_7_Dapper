using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Services;
using BackendCRUD.Common;
using BackendCRUD.Infraestructure;
using BackendCRUD.Infraestructure.Repository;
using BackendCRUD.Minimal.Api.Endpoints;
using BackendCRUD.Minimal.Api.Model;
using Carter;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using FluentValidation;
using System.Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;
using BackendCRUD.Application.Behaviors;
using BackendCRUD.Application.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

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
                            "http://localhost:3003",
                            "http://localhost:12000",
                            "http://localhost:8080",
                            "http://localhost:8081",
                            "http://localhost:8082",
                            "http://localhost:5000",
                            "http://localhost:25471",
                            "http://localhost:57344",
                            "http://localhost:57343",
                            "http://localhost:80"
                            )
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
        //.WithExposedHeaders("content-disposition");
    });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ErrorHandlingFilterAttribute>();
});

foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly)
                                        .AddOpenBehavior(typeof(ValidationBehavior<,>))
                                        .AddOpenBehavior(typeof(LoggingBehavior<,>))
    );

    builder.Services.AddValidatorsFromAssembly(assembly);
}
//builder.Services.AddMediatR(config =>
//{
//    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
//    //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
//    //config.AddOpenBehavior(typeof(LoggingBehavior<,>));
//});



builder.Services.AddCarter();

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
});

builder.Services.AddOptions();

//Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks();
//    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();


// Ensure database is created during application startup
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<DBContextBackendCRUD>();
//    await dbContext.Database.EnsureCreatedAsync();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //Solo habilitado en producci�n
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseHsts();
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

//app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("localhost");
app.UseAuthentication();
app.UseAuthorization();

// SGC - Asigna la version del Assembly al Log4Net
Assembly thisApp = Assembly.GetExecutingAssembly();

AssemblyName name = new AssemblyName(thisApp.FullName);

// Identifica la versi�n del ensamblado
GlobalDiagnosticsContext.Set("VersionApp", name.Version.ToString());

// Identifica el nombre del ensamblado para poder identificar el ejecutable
GlobalDiagnosticsContext.Set("AppName", name.Name);

// Obtiene el process ID
Process currentProcess = Process.GetCurrentProcess();

GlobalDiagnosticsContext.Set("ProcessID", "PID " + currentProcess.Id.ToString());


//app.UseExceptionHandler("/error");
//app.UseExceptionHandler(exceptionHandlerApp
//    => exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));

app.Map("/error", () =>
                {
                    ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Error, "INICIO_API", "An Error Occurred...!!");

                    throw new InvalidOperationException("An Error Occurred...");
                });

app.UseRouting();
app.UseResponseCaching();
app.UseHttpsRedirection();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.MapCarter();
app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "===== INICIO API [BackendCRUD.Minimal.Api] =====");

app.Run();
