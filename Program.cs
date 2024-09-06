using Example1.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Example1
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            // One time call to install Playwright for the first time
            //Microsoft.Playwright.Program.Main(["install"]);
            //return;


            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddTransient<ReportGenerator>();

            var serviceProvider = services.BuildServiceProvider();

            var reportGenerator = serviceProvider.GetRequiredService<ReportGenerator>();

            var file = await reportGenerator.GeneratePurchaseOrderAsync();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "PurchaseOrder.pdf");
            await File.WriteAllBytesAsync(path, file);

        }
    }
}
