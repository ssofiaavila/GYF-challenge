using System;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Datos.Database
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }

        private const string SERVER_NAME = "SERVER_NAME";
        private static readonly string ConnectionString = $"Server={SERVER_NAME};Database=gyf_challenge;Trusted_Connection=True;TrustServerCertificate=True;";

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>()
                .HasKey(p => p.Id); 

            modelBuilder.Entity<Producto>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); 
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
