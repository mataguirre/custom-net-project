using API.Domain;
using API.Domain.Ejercicios;
using Microsoft.EntityFrameworkCore;

namespace API.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure your own tables/entities inside here */
        }
    }
}
