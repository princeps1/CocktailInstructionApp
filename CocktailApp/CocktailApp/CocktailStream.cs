using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CocktailApp
{
    public class CocktailStream : IObservable<Cocktail>
    {
        const string Url = "https://www.thecocktaildb.com/api/json/v1/1/search.php?s=";
        private Subject<Cocktail> cocktailsSubject;

        private readonly HttpClient client;

        public CocktailStream()
        {
            cocktailsSubject = new Subject<Cocktail>();
            client = new HttpClient();
            //
        }

        public async Task SearchAsync(HttpListenerContext context)
        {
            var request = context.Request;
            string cocktailName = request.Url.Segments.Last();

            try
            {
                string UrlForSend = Url + cocktailName;

                var response = await client.GetAsync(UrlForSend);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();


                var jsonResponse = JObject.Parse(content);
                var cocktailsJson = jsonResponse["drinks"];
                //Console.WriteLine(cocktailsJson);//prikazivanje json format podataka
                if(cocktailsJson.HasValues == false) 
                {
                    throw new Exception("Ne postoji ovo pice u ponudi\n\n");
                }

                List<Cocktail> lista = new List<Cocktail>();
                lista.AddRange
                (
                   cocktailsJson.Select(cocktail => new Cocktail
                   {
                       Name = (string)cocktail["strDrink"],
                       Instructions = (string)cocktail["strInstructions"],
                       GlassType = (string)cocktail["strGlass"] 
                   })
                );

                foreach (var cocktail in lista)
                {
                    cocktailsSubject.OnNext(cocktail);
                }
                cocktailsSubject.OnCompleted();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}"); // Ispisivanje poruke iz izuzetka
                cocktailsSubject.OnError(ex);
            }
        }

        public IDisposable Subscribe(IObserver<Cocktail> observer)
        {
            return cocktailsSubject
                .SubscribeOn(ThreadPoolScheduler.Instance)//pretplata se izvrsava asinhrono,glavna nit se ne blokira
                .ObserveOn(ThreadPoolScheduler.Instance)//observable emituje asinhrono,obavestenja koja observer prima bice isporucena preko worker niti
                .Subscribe(observer);
        }
    }
}
