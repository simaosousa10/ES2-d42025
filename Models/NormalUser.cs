namespace ESIID42025.Models;

public class NormalUser : User, IPriceRegistable
{
    public void RegistarPreco(Price preco)
    {
        Prices.Add(preco);
        Console.WriteLine($"Utilizador {Name} registou o preço: {preco.Value}");
    }
}