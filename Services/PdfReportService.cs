using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ESIID42025.Models;
using Microsoft.AspNetCore.Hosting;

namespace ESIID42025.Services;

public class PdfReportService
{
    private readonly IWebHostEnvironment _env;

    public PdfReportService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public byte[] GenerateStoreReportPdf(string userName, List<Store> stores, Dictionary<int, Dictionary<string, int>> categoryCounts)
{
    QuestPDF.Settings.License = LicenseType.Community;

    var logoPath = Path.Combine(_env.WebRootPath, "images", "LogoEmAzul.png");
    var logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;
    var currentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

    return Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.A4);

            // HEADER
            page.Header().Element(header =>
            {
                header.Row(row =>
                {
                    if (logoBytes != null)
                    {
                        row.ConstantItem(120).Image(logoBytes);
                    }

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text(t =>
                        {
                            t.Span("Relatório Geral de Lojas")
                             .FontSize(20)
                             .FontColor("#3064fc")
                             .Bold();
                        });

                        col.Item().AlignRight().Text(t =>
                        {
                            t.Span($"Gerado por: {userName}").Style(TextStyle.Default.Size(10));
                        });

                        col.Item().AlignRight().Text(t =>
                        {
                            t.Span($"Data: {currentDate}").Style(TextStyle.Default.Size(10));
                        });
                    });
                });
            });

            // CONTEÚDO
            page.Content().Element(content =>
            {
                content.Column(col =>
                {
                    foreach (var store in stores)
                    {
                        // Força o bloco completo a manter-se na mesma página
                        col.Item().PaddingVertical(20).Element(e => e.ShowEntire()).Element(e =>
                        {
                            e.Column(c =>
                            {
                                c.Item().Text(t =>
                                {
                                    t.Span(store.Name).Style(TextStyle.Default.Size(14).Bold());
                                });

                                c.Item().Text(t =>
                                {
                                    t.Span($"Localização: {store.Location}").Style(TextStyle.Default.Size(10).Italic());
                                });

                                if (categoryCounts.TryGetValue(store.ID, out var categories) && categories.Any())
                                {
                                    c.Item().Text(t =>
                                    {
                                        t.Span("Categorias:").Style(TextStyle.Default.Size(11).Bold());
                                    });

                                    foreach (var cat in categories)
                                    {
                                        c.Item().Text(t =>
                                        {
                                            t.Span($"• {cat.Key} ({cat.Value})").Style(TextStyle.Default.Size(10));
                                        });
                                    }
                                }
                                else
                                {
                                    c.Item().Text(t =>
                                    {
                                        t.Span("Sem produtos registados.").Style(TextStyle.Default.Size(10).Italic());
                                    });
                                }

                                // LINHA separadora
                                c.Item().PaddingTop(10).LineHorizontal(1);
                            });
                        });
                    }
                });
            });

            // FOOTER
            page.Footer().AlignCenter().Text(t =>
            {
                t.Span("DealRadar ").Style(TextStyle.Default.Size(9).Bold());
                t.Span("- Relatório PDF gerado automaticamente.").Style(TextStyle.Default.Size(9).Italic());
            });
        });
    }).GeneratePdf();
}

}
