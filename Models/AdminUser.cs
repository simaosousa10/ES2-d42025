namespace ESIID42025.Models;

public class AdminUser : User, IAdmin
{
    public void GerarRelatorioEspecial()
    {
        // Lógica de geração de relatório só para admins
        Console.WriteLine($"Admin {Name} gerou relatório especial.");
    }
}