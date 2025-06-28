using WebApiPlayground.Clients;

namespace WebApiPlayground.Services;

public class NumbersService(NumbersClient _numbersClient)
{
    private readonly int _number = new Random().Next(1, 100);
    public int GetNumber()
    {
        return _numbersClient.GetNumber();
    }
}
