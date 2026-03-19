using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace PersonManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var provider = App.BuildDiContainer();
            App.EnsureAndSeedDatabase(provider);
            var mediator = provider.GetRequiredService<IMediator>();
            var menu = new ConsoleMenu(mediator);
            await menu.RunAsync();
        }
    }
}
