using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Services
{
    public class MyService : IMyService
    {
        public string Greeting(string name)
        {
            return $"Welcome, {name} \n\n {DateTime.UtcNow}";
        }
    }
}