using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

[assembly: ASNA.QSys.Expo.Model.ExpoModelAssembly()] // Mark this assembly as containing Display Page Models.

namespace SunFarmSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<SunFarmSite.Startup>();
                });
    }
}

