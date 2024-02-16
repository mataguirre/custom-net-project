using API.Domain;
using API.Domain.Ejercicios;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace API.EntityFrameworkCore
{
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Ejercicio> Ejercicios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure your own tables/entities inside here */

            builder.Entity<Ejercicio>(a =>
            {
                a.ToTable(FitnessConsts.DbTablePrefix + "Ejercicios", FitnessConsts.DbSchema);
                a.HasKey(a => a.Id);
                a.Property(a => a.Title).IsRequired();
            });
        }
    }
}
