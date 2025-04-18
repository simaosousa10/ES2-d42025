using ESIID42025.Data;
using ESIID42025.Models;

public class ReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> ExportarRelatorioAsync(GeneratedReport report)
    {
        return await report.GerarAsync(_context);
    }
}