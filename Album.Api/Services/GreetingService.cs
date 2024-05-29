using System.Net;

namespace Album.Api.Services
{
    public class GreetingService
    {
        public string greeting(string name = "")
        {

        if(string.IsNullOrEmpty(name))
            {
                return "Hello World! v2";
            }
            else
            {
                return $"Hello {name} {Dns.GetHostName()} v2";
            }

        }
    }
}
