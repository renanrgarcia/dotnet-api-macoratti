using WebApiPlayground.Clients;

namespace WebApiPlayground.Services;

public class NumbersService(INumbersClient _numbersClient)
{
    public int GetNumber()
    {
        return _numbersClient.GetNumber();
    }
}
