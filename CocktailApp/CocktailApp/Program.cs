using System;
using System.Net;
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
            var observer = new CocktailObserver();

            var subscription = stream.Subscribe( observer );


            HttpListenerContext context = await listener.GetContextAsync();
            await stream.SearchAsync(context);

            Console.ReadLine();
            subscription.Dispose();
            listener.Stop();
        }
        //http://localhost:5050/Margarita
        //http://localhost:5050/Blue
        //http://localhost:5050/asd
    }
}
