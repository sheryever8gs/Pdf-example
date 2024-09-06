using Example1.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example1
{
    public class ReportGenerator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;

        public ReportGenerator(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = loggerFactory;
        }

        public async Task<byte[]> GeneratePurchaseOrderAsync()
        {
            var html = await GenerateReportHtmlAsync();
            var file = await GeneratePdfReportAsync(html);
            return file;
        }

        public async Task<string> GenerateReportHtmlAsync()
        {
            await using var htmlRenderer = new HtmlRenderer(_serviceProvider, _loggerFactory);

            var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "Model", PurchaseOrder.GetFake() }
                };

                var parameters = ParameterView.FromDictionary(dictionary);
                var output = await htmlRenderer.RenderComponentAsync<PurchaseOrderReport>(parameters);
                return output.ToHtmlString();
            });

            return html;
        }

        public async Task<byte[]> GeneratePdfReportAsync(string html)
        {

            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            var tempDirectory = Path.GetTempPath();
            var filePath = $"{tempDirectory}/{Guid.NewGuid()}.pdf";
            await page.PdfAsync(new PagePdfOptions
            {
                Format = "A4",
                Path = filePath
            });

            await page.CloseAsync();

            if (File.Exists(filePath))
            {
                return await File.ReadAllBytesAsync(filePath);
            }
            else
            {
                throw new Exception("PDF report generation failed");
            }
        }
        
    }
}
