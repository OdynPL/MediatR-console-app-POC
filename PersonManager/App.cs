using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using PersonManager.Repositories;
using PersonManager.Data;
using PersonManager.Services;
using PersonManager.Handlers;
using Microsoft.Extensions.Configuration;

namespace PersonManager
{
    public static class App
    {
        public static ServiceProvider BuildDiContainer()
        {
            var services = new ServiceCollection();
            services.AddLogging();

            // Wczytaj konfigurację z appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("Default");
            services.AddSingleton(configuration);
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<UnitOfWork.IUnitOfWork>(provider =>
                new UnitOfWork.UnitOfWork(
                    provider.GetRequiredService<AppDbContext>(),
                    provider.GetRequiredService<IPersonRepository>(),
                    provider.GetRequiredService<IAddressRepository>(),
                    provider.GetRequiredService<ICompanyRepository>(),
                    provider.GetRequiredService<IProjectRepository>()
                ));

            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePersonCommandHandler).Assembly));
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddAutoMapper(typeof(MappingProfile));
            // Rejestracja walidatorów FluentValidation
            services.AddValidatorsFromAssembly(typeof(App).Assembly);
            // Pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
            return services.BuildServiceProvider();
        }

        public static void EnsureAndSeedDatabase(ServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                DbSeeder.Seed(db);
            }
        }
    }
}
