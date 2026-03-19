using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using PersonManager.Repositories;
using PersonManager.Data;
using PersonManager.Services;
using PersonManager.Handlers;

namespace PersonManager
{
    public static class App
    {
        public static ServiceProvider BuildDiContainer()
        {
            var services = new ServiceCollection();
            services.AddLogging();
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("Data Source=app.db"));
                services.AddScoped<IPersonRepository, PersonRepository>();
                services.AddScoped<PersonManager.UnitOfWork.IUnitOfWork, PersonManager.UnitOfWork.UnitOfWork>();
                services.AddScoped<IPersonService, PersonService>();
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
