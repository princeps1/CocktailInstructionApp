using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CocktailApp
{
    public class Program
    {
        static HttpListener listener = new HttpListener();
        static async Task Main(string[] args)
        {
            listener.Prefixes.Add($"http://localhost:5050/");
            Console.WriteLine("Server je startovan...\n\n");
            listener.Start();

            var stream = new CocktailStream();
            var observer = new CocktailObserver("cao");

            stream.Subscribe( observer );

            while(true ) 
            {
                HttpListenerContext context = listener.GetContext();
                _= Task.Run(() => stream.SearchAsync(context));
            }
        }
        //http://localhost:5050/Margarita
        //http://localhost:5050/asd
    }
}
