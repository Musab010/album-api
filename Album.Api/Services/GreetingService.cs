using System.Net;

namespace Album.Api.Services
{
    public class GreetingService
    {
        public string greeting(string name = "")
        {
            
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Hello World";
        }

        return $"Hello {name}";

        }
    }
}
