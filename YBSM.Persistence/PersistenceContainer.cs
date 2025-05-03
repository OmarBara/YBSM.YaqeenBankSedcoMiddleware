using Core.Application.Repositories;
using HRM.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oracle.EntityFrameworkCore;


namespace Infrastructure.Persistence
{
    public static class PersistenceContainer
    {
        public static IServiceCollection AdPersistenceRegistration(this IServiceCollection services,
            IConfiguration configuration)
        {
            /*services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default"), opt =>
                    opt.EnableRetryOnFailure(5,
                        TimeSpan.FromSeconds(30),
                        null)));*/

           /* services.AddDbContext<AppDbContext>(options =>
                options.UseOracle(configuration.GetConnectionString("Default"), opt =>
                    opt.EnableRetryOnFailure(5,
                        TimeSpan.FromSeconds(30),
                        null)));*/

            services.AddDbContext<AppDbContext>(options =>
               options.UseOracle(configuration.GetConnectionString("DefaultOracle"), opt =>
                   opt.ExecutionStrategy(dependencies => new OracleRetryingExecutionStrategy(dependencies)))); // Replace EnableRetryOnFailure with OracleRetryingExecutionStrategy


            services.AddScoped<IUnitOfWork, UnitOfWork>();

           /* using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
                if (!dbContext.Database.CanConnect())
                {
                    throw new Exception("Unable to connect to the Oracle database. Please check the connection string and database availability.");
                }
            }*/

            return services;
        }
    }
}