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



// var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
//var servidorbd = Environment.GetEnvironmentVariable("MSSQL_SA_SERVER") ?? @"THEKONES-PC\\SQLEXPRESS";
//var puerto = Environment.GetEnvironmentVariable("MSSQL_SA_PORT") ?? @"1433";
//var basedatos = Environment.GetEnvironmentVariable("DB_HOST");
//var user = Environment.GetEnvironmentVariable("DB_NAME");
//var contrasenna = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

//var CONN = $"Server={servidorbd},{puerto};Initial Catalog={basedatos};User ID={user};Password={contrasenna}";

//ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", $"VARIABLES DE ENTORNO {CONN}");

//builder.Services.AddDbContext<DBContextBackendCRUD>(
//                    options =>
//                    {
//                        var connectionString = builder.Configuration.GetConnectionString("stringConnection");
//                        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", $"===> CONN [{connectionString}]...");

//                        if (!builder.Environment.IsDevelopment())
//                        {
//                            ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", $"===> PRODUCCION MODE!");

//                            // var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
//                            var servidorbd = Environment.GetEnvironmentVariable("MSSQL_SA_SERVER") ?? @"THEKONES-PC\\SQLEXPRESS";
//                            var puerto = Environment.GetEnvironmentVariable("MSSQL_SA_PORT") ?? @"1433";
//                            var basedatos = Environment.GetEnvironmentVariable("DB_HOST");
//                            var user = Environment.GetEnvironmentVariable("DB_NAME");
//                            var contrasenna = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

//                            connectionString = $"Server={servidorbd},{puerto};Initial Catalog={basedatos};User ID={user};Password={contrasenna};TrustServerCertificate=true";

//                        }
//                        else
//                            ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", $"===> DEBUG MODE!");


//                        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", $"===> CONEXION [{connectionString}]...");

//                        options.UseSqlServer(connectionString);                      

//                    });

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
    //Solo habilitado en producción
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


app.UseCors("localhost");
app.UseAuthentication();
app.UseAuthorization();

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


//app.UseExceptionHandler("/error");
app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));

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

//builder.Services.AddDbContext<DockerComposeDemoDbContext>(
//                       options =>
//                       {
//                           var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//                           if (!builder.Environment.IsDevelopment())
//                           {
//                               // var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
//var servidorbd = Environment.GetEnvironmentVariable("MSSQL_SA_SERVER") ?? @"THEKONES-PC\\SQLEXPRESS";
//var puerto = Environment.GetEnvironmentVariable("MSSQL_SA_PORT") ?? @"1433";
//var basedatos = Environment.GetEnvironmentVariable("DB_HOST");
//var user = Environment.GetEnvironmentVariable("DB_NAME");
//var contraseña = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

//connectionString = $"Server={servidorbd},{puerto};Initial Catalog={basedatos};User ID={user};Password={contraseña};TrustServerCertificate=true";


//                           }
//                           options.UseSqlServer(connectionString);

//                       });

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<DockerComposeDemoDbContext>();
//    db.Database.Migrate();
//}



ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "===== INICIO API [BackendCRUD.Minimal.Api] =====");

app.Run();
