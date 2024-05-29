using System;
using System.Net;
using Xunit;
using Album.Api.Services;

namespace Album.Api
{
    public class UnitTest1
    {
        
        [Fact]
        public void greetingWithName()
        {
            var gs = new GreetingService();
            string name = "Musab";
            Assert.Equal("Hello " + name + " " +Dns.GetHostName() + " v2", gs.greeting(name));
        }
        
        [Fact]
        public void greetingWithEmptyString()
        {
            var gs = new GreetingService();
            string name = string.Empty;
            Assert.Equal("Hello World! v2", gs.greeting(name));
        }

        [Fact]
        public void greetingWithNull()
        {
            var gs = new GreetingService();
            string name = null;
            Assert.Equal("Hello World! v2", gs.greeting(name));
        }

        [Fact]
        public void greetingWithSpace()
        {
            var gs = new GreetingService();
            string name = "";
            Assert.Equal("Hello World! v2", gs.greeting(name));
        }


    }
}
