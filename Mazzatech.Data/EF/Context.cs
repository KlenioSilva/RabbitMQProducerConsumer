using Mazzatech.Domain.EntitiesModels;
using Microsoft.EntityFrameworkCore;

namespace Mazzatech.Data.EF
{
    public class Context : DbContext
    {
        public DbSet<ProtocoloEntityModel> Protocolos { get; set; }
        public DbSet<DbErrorLoggerEntityModel> DbErrorsLoggers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Projetos\\Mazzatech\\Sln.Mazzatech.ProtocolRobot\\SqLite\\DbProtocolo.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProtocoloEntityModel>()
                .ToTable("TbProtocolo");

            modelBuilder.Entity<DbErrorLoggerEntityModel>()
                .ToTable("TbErrorLogger");

            // Configuração para impedir protocolos duplicados
            modelBuilder.Entity<ProtocoloEntityModel>().HasIndex(p => p.Protocolo).IsUnique();

            // Configuração para impedir CPF e RG com mesmo número de via repetido
            modelBuilder.Entity<ProtocoloEntityModel>().HasIndex(p => new { p.CPF, p.Via }).IsUnique();
            modelBuilder.Entity<ProtocoloEntityModel>().HasIndex(p => new { p.RG, p.Via }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

