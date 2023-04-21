using DotNetA11.Services;
using DotNetA11_MLE.Context;
using DotNetA11_MLE.Dao;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetA11
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddFile("app.log");
            });

            // Concrete Services
            services.AddTransient<IMovieService, MovieService>();
            services.AddSingleton<IRepository, Repository>();
            services.AddDbContextFactory<MovieContext>();

            return services.BuildServiceProvider();
        }
    }
}
