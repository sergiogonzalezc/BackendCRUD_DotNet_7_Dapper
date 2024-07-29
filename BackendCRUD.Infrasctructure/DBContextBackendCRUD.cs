//using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using BackendCRUD.Infraestructure.Repository;
using BackendCRUD.Application.Model;
using System.Configuration;
using Microsoft.Extensions.Options;
using BackendCRUD.Common;

namespace BackendCRUD.Infraestructure
{
    public class DBContextBackendCRUD : DbContext
    {
        private string _connString { get; set; }

        public DbSet<MemberEF> Member { get; set; }
        public DbSet<MemberTypesEF> MemberType { get; set; }
        public DbSet<RoleTypeEF> RoleType { get; set; }
        public DbSet<MemberTagEF> MemberTag { get; set; }
        public DbSet<TagEF> Tag { get; set; }


        private DBContextBackendCRUD()
        {
            _connString = String.Empty;
        }

        public DBContextBackendCRUD(string cadenaConexion)
        {
            _connString = cadenaConexion;
        }
        public DBContextBackendCRUD(DbContextOptions<DBContextBackendCRUD> dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                _connString = Database.GetDbConnection().ConnectionString ?? String.Empty;

                //// var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
                //var servidorbd = Environment.GetEnvironmentVariable("DB_SERVER_HOST") ?? @"THEKONES-PC\\SQLEXPRESS";
                //var puerto = Environment.GetEnvironmentVariable("DB_SERVER_PORT") ?? @"1433";
                //var basedatos = Environment.GetEnvironmentVariable("DB_NAME");
                //var user = Environment.GetEnvironmentVariable("DB_USER");
                //var contrasenna = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

                //_connString = $"Server={servidorbd},1433;Initial Catalog={basedatos};User ID={user};Password={contrasenna};TrustServerCertificate=true";
                                
                //var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

                //ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "OnConfiguring", $"Se setea BD [{_connString}]");

                //if (databaseCreator != null)
                //{
                //    if (!databaseCreator.CanConnect())
                //    {
                //        databaseCreator.Create(); // crea la BD vacías en el sql server de Docker
                //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "BD creada OK!!");
                //    }
                //    else
                //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "BD ya existe!");

                //    if (!databaseCreator.HasTables())
                //    {
                //        databaseCreator.CreateTables(); // crea la BD vacías en el sql server de Docker
                //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "Tablas creadas OK!!");
                //    }
                //    else
                //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "Tablas ya existen!");

                //}

                //ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "OnConfiguring", $"Fin BD!");

            }
            catch (Exception ex)
            {
                ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, ex, "DBContextBackendCRUD", "Error inicio!");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (!optionsBuilder.IsConfigured)
                {
                    // var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
                    var servidorbd = Environment.GetEnvironmentVariable("DB_SERVER_HOST") ?? @"THEKONES-PC\\SQLEXPRESS";
                    var puerto = Environment.GetEnvironmentVariable("DB_SERVER_PORT") ?? @"1433";
                    var basedatos = Environment.GetEnvironmentVariable("DB_NAME");
                    var user = Environment.GetEnvironmentVariable("DB_USER");
                    var contrasenna = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

                    _connString = $"Server={servidorbd},{puerto};Initial Catalog={basedatos};User ID={user};Password={contrasenna};TrustServerCertificate=true";

                    optionsBuilder.UseSqlServer(_connString);

                    //var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

                    //ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "OnConfiguring", $"Se setea BD [{_connString}]");

                    //if (databaseCreator != null)
                    //{
                    //    if (!databaseCreator.CanConnect())
                    //    {
                    //        databaseCreator.Create(); // crea la BD vacías en el sql server de Docker
                    //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "BD creada OK!!");
                    //    }
                    //    else
                    //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "BD ya existe!");

                    //    if (!databaseCreator.HasTables())
                    //    {
                    //        databaseCreator.CreateTables(); // crea la BD vacías en el sql server de Docker
                    //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "Tablas creadas OK!!");
                    //    }
                    //    else
                    //        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "INICIO_API", "Tablas ya existen!");

                    //}

                    //ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Info, "OnConfiguring", $"Fin BD!");

                }
            }
            catch (Exception ex)
            {
                ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, ex, "OnConfiguring", "Error inicio!");
            }

        }
    }
}