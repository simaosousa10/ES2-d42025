using ESIID42025.Models;

namespace ESIID42025.Services;

public class UserActionService
{
    public void ExecutarAcao(User user)
    {
        if (user is IAdmin admin)
        {
            admin.GerarRelatorioEspecial();
        }
        else if (user is IPriceRegistable registador)
        {
            registador.RegistarPreco(new Price { Value = 5 });
        }
        else
        {
            Console.WriteLine("Utilizador sem permissões especiais.");
        }
    }
}