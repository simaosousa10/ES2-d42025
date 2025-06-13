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
                                t.Span("Relatório Geral de Lojas").FontSize(20).FontColor("#3064fc").Bold();
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

                page.Content().Element(content =>
                {
                    content.Column(col =>
                    {
                        foreach (var store in stores)
                        {
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
                                    c.Item().PaddingTop(10).LineHorizontal(1);
                                });
                            });
                        }
                    });
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("DealRadar ").Style(TextStyle.Default.Size(9).Bold());
                    t.Span("- Relatório PDF gerado automaticamente.").Style(TextStyle.Default.Size(9).Italic());
                });
            });
        }).GeneratePdf();
    }

    public byte[] GenerateStoreSpecificReportPdf(string userName, Store store, List<Product> products)
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
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Element(header =>
                {
                    header.Row(row =>
                    {
                        if (logoBytes != null)
                            row.ConstantItem(120).Image(logoBytes);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignRight().Text(t =>
                            {
                                t.Span("Relatório da Loja").FontSize(20).FontColor("#3064fc").Bold();
                            });

                            col.Item().AlignRight().Text($"Gerado por: {userName}").FontSize(10);
                            col.Item().AlignRight().Text($"Data: {currentDate}").FontSize(10);
                        });
                    });
                });

                page.Content().Element(content =>
                {
                    content.Column(col =>
                    {
                        col.Item().Text(store.Name).FontSize(16).Bold();
                        col.Item().Text($"Localização: {store.Location}")
                            .Italic().FontSize(12).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);

                        col.Item().Height(10);

                        if (products != null && products.Any())
                        {
                            foreach (var p in products)
                            {
                                var price = p.Prices.OrderByDescending(p => p.Date).FirstOrDefault()?.Value;

                                col.Item().BorderBottom(1).PaddingVertical(5).Row(row =>
                                {
                                    row.RelativeItem().Column(inner =>
                                    {
                                        inner.Item().Text(p.Name).Bold();
                                        inner.Item().Text(p.Description)
                                            .FontColor(QuestPDF.Helpers.Colors.Grey.Darken2).FontSize(10);
                                    });

                                    row.ConstantItem(80).AlignRight().AlignMiddle().Text($"{price?.ToString("0.00")} €")
                                        .FontColor(QuestPDF.Helpers.Colors.Blue.Medium).SemiBold().FontSize(11);
                                });
                            }
                        }
                        else
                        {
                            col.Item().Text("Sem produtos registados.")
                                .Italic().FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                        }
                    });
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("DealRadar ").Style(TextStyle.Default.Size(9).Bold());
                    t.Span("- Relatório PDF gerado automaticamente.").Style(TextStyle.Default.Size(9).Italic());
                });
            });
        }).GeneratePdf();
    }

  public byte[] GenerateProductReportPdf(string userName, Product product)
{
    QuestPDF.Settings.License = LicenseType.Community;

    var logoPath = Path.Combine(_env.WebRootPath, "images", "LogoEmAzul.png");
    var logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;
    var currentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

    var latestPrices = product.Prices?
        .Where(p => p.Store != null)
        .GroupBy(p => p.StoreID)
        .Select(g => g.OrderByDescending(p => p.Date).First())
        .OrderBy(p => p.Value)
        .ToList() ?? new();

    return Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(12));

            // Cabeçalho
            page.Header().Element(header =>
            {
                header.Row(row =>
                {
                    if (logoBytes != null)
                        row.ConstantItem(100).Image(logoBytes);

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignRight().Text("Relatório do Produto")
                            .FontSize(20).FontColor("#3064fc").Bold();
                        col.Item().AlignRight().Text($"Gerado por: {userName}").FontSize(10);
                        col.Item().AlignRight().Text($"Data: {currentDate}").FontSize(10);
                    });
                });
            });

            // Conteúdo
            page.Content().Element(content =>
            {
                content.Column(col =>
                {
                    // Info do produto
                    col.Item().PaddingBottom(10).Text(product.Name).FontSize(18).Bold();
                    col.Item().PaddingBottom(5).Text(product.Description).FontSize(12);
                    col.Item().PaddingBottom(20).Text($"Categoria: {product.Category?.Name}")
                        .FontSize(11).FontColor(Colors.Grey.Darken2).Italic();

                    // Secção de preços
                    if (latestPrices.Any())
                    {
                        col.Item().Text("Preços mais recentes por loja:")
                            .FontSize(14).Bold().FontColor(Colors.Black);

                        col.Item().PaddingVertical(10).PaddingHorizontal(8).Background(Colors.Grey.Lighten4).Column(priceCol =>
                        {
                            foreach (var price in latestPrices)
                            {
                                priceCol.Item().PaddingVertical(6).Row(row =>
                                {
                                    row.RelativeItem().Text($"{price.Store?.Name} ({price.Date:dd/MM/yyyy})")
                                        .FontSize(11);
                                    row.ConstantItem(90).AlignRight().Text($"{price.Value:0.00} €")
                                        .FontColor(Colors.Blue.Medium).FontSize(12).Bold();
                                });

                                priceCol.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
                            }
                        });
                    }
                    else
                    {
                        col.Item().PaddingTop(15).Text("Este produto não tem preços registados.")
                            .Italic().FontColor(Colors.Grey.Darken2);
                    }
                });
            });

            // Rodapé
            page.Footer().AlignCenter().Text(t =>
            {
                t.Span("DealRadar ").Bold().FontSize(9);
                t.Span("- Relatório PDF gerado automaticamente.").Italic().FontSize(9);
            });
        });
    }).GeneratePdf();
}

}
