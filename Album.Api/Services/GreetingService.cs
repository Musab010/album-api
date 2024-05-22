using System.Net;

namespace Album.Api.Services
{
    public class GreetingService
    {
        public string greeting(string name = "")
        {
            if(string.IsNullOrEmpty(name))
            {
                return "Hello World!";
            }
            else
            {
                return $"Hello {name} {Dns.GetHostName()}";
            }
        }
    }
}
