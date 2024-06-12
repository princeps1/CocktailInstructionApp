using System;
using System.Collections.Generic;
using System.Linq;

namespace CocktailApp
{
    public class CocktailObserver : IObserver<Cocktail>
    {
        private readonly string name;
        private List<string> words = new List<string>(); // Polje za čuvanje reči

        public CocktailObserver(string name)
        {
            this.name = name;
        }

        public void OnCompleted()
        {
            foreach (var word in words)
            {
                Console.WriteLine(word);
            }
            Console.WriteLine(); // Prazan red između svakog koktela
            Console.WriteLine(); // Prazan red između svakog koktela
            Console.WriteLine("Svi kokteli su uspesno pribavljeni");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.ToString());
        }

        public void OnNext(Cocktail cocktail)
        {
            char[] separators = { ' ', ',', '.', '\n' };
            // Dodaj reči u postojeću listu
            words.AddRange(cocktail.Instructions.Split(separators, StringSplitOptions.RemoveEmptyEntries));

            Console.WriteLine($"Name: {cocktail.Name}");
            Console.WriteLine($"Instructions: {cocktail.Instructions}");
            Console.WriteLine($"Glass Type: {cocktail.GlassType}");
            Console.WriteLine(); // Prazan red između svakog koktela
        }

        // Metoda za dobijanje svih reči
        public List<string> GetAllWords()
        {
            return words;
        }
    }
}



