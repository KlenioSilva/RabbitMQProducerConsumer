using Mazzatech.Domain.EntitiesModels;
using Microsoft.EntityFrameworkCore;

namespace Mazzatech.Data.EF
{
    public class AuthenticateContext : DbContext
    {
        public DbSet<UserEntityModel> Users { get; set; }
        public DbSet<IssuerSecretKeyEntityModel> IssuerSecretKeys { get; set; }
        public DbSet<IssuerUserSecretKeyTokenEntityModel> IssuerUserSecretKeyTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Projetos\\Mazzatech\\Sln.Mazzatech.ProtocolRobot\\SqLite\\DbAuthenticate.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntityModel>()
                .ToTable("TbUser");

            modelBuilder.Entity<IssuerSecretKeyEntityModel>()
                .ToTable("TbIssuerSecretKey");

            modelBuilder.Entity<IssuerUserSecretKeyTokenEntityModel>()
                .ToTable("TbIssuerUserSecretKeyToken");

            // Configuração para impedir UsersNames duplicados
            modelBuilder.Entity<UserEntityModel>().HasIndex(p => p.UserName).IsUnique();
            // Configuração para impedir UserName e e-mail duplicados
            modelBuilder.Entity<UserEntityModel>().HasIndex(p => new { p.UserName, p.Email }).IsUnique();
            
            // Configuração para issuer (emissor) e chave duplicados
            modelBuilder.Entity<IssuerSecretKeyEntityModel>().HasIndex(p => new { p.Issuer, p.SecretKey }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
