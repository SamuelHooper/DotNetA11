using DotNetA11.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetA11
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var startup = new Startup();
                var serviceProvider = startup.ConfigureServices();
                var service = serviceProvider.GetService<IMovieService>();

                service?.Invoke();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}