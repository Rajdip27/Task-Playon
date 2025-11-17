using PuppeteerSharp;
using PuppeteerSharp.Media;
using TaskPlayon.Application.Enums;

namespace TaskPlayon.Application.Extensions;

public static class PdfExtensions
{
    public static PdfOptions ToPdfPaperFormat(this string size, PaperOrientation orientation)
    {
        var options = new PdfOptions
        {
            MarginOptions = new MarginOptions
            {
                Top = "30px",
                Bottom = "30px",
                Left = "30px",
                Right = "30px"
            },
            Landscape = orientation == PaperOrientation.Landscape
        };

        switch (size.ToUpper())
        {
            case "A4": options.Width = "210mm"; options.Height = "297mm"; break;
            case "A5": options.Width = "148mm"; options.Height = "210mm"; break;
            case "A6": options.Width = "105mm"; options.Height = "148mm"; break;
            case "A7": options.Width = "74mm"; options.Height = "105mm"; break;
            case "A8": options.Width = "52mm"; options.Height = "74mm"; break;
            case "A9": options.Width = "37mm"; options.Height = "52mm"; break;
            default: throw new ArgumentException($"Unsupported size: {size}");
        }

        return options;
    }
}
