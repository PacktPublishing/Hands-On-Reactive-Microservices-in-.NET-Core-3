using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ReactiveMicroservices.Sample.Customers.Web
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) => config
                    .AddJsonFile("appsettings.json", true)
                    .AddYamlFile("configs/messaging", true, reloadOnChange:true)
                    .AddJsonFile("secrets/customerservice.settings.json", true, true))
                .UseStartup<Startup>();
    }
}
