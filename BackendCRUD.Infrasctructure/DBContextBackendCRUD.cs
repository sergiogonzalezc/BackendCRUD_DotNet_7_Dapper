//using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using BackendCRUD.Infraestructure.Repository;
using BackendCRUD.Application.Model;

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
        public DBContextBackendCRUD(DbContextOptions opciones) : base(opciones)
        {

            _connString = Database.GetDbConnection().ConnectionString ?? String.Empty;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connString);
            }
        }
    }
}