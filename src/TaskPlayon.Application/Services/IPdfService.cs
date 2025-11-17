using PuppeteerSharp;
using RazorLight;
using TaskPlayon.Application.Enums;
using TaskPlayon.Application.Extensions;

namespace TaskPlayon.Application.Services;

public interface IPdfService
{
    Task<byte[]> GeneratePdfAsync<T>(string viewName, T model, string pageSize = "A4", PaperOrientation orientation = PaperOrientation.Portrait);
}
public class PdfService : IPdfService
{
    private readonly RazorLightEngine _engine;

    public PdfService()
    {
        var templatesPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(templatesPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<byte[]> GeneratePdfAsync<T>(string viewName, T model, string pageSize = "A4", PaperOrientation orientation = PaperOrientation.Portrait)
    {
        if (!viewName.EndsWith(".cshtml"))
            viewName += ".cshtml";
        string html = await _engine.CompileRenderAsync(viewName, model);
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        PdfOptions options = pageSize.ToPdfPaperFormat(orientation);
        return await page.PdfDataAsync(options);
    }
}