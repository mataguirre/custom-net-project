using API.Definitions.Conventions;
using API.Definitions.Repositories;
using API.Domain;
using API.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddControllers();
            services.AddDbContext<FitnessDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("Fitness_DB")));
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<FitnessDataSeedContributor>();
            services.AddScoped<FitnessDbContext>();
            services.AddControllersWithViews(options =>
            {
                options.Conventions.Add(new CustomControllerModelConvention());
            });
            //services.AddAutoMapper(typeof(FitnessAutoMapperProfile));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Task.Run(async () =>
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var seedContributor = serviceScope.ServiceProvider.GetRequiredService<FitnessDataSeedContributor>();
                    await seedContributor.SeedAsync();
                }
            }).Wait();
        }
    }
}
