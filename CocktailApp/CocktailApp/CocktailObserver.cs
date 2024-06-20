using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CocktailApp
{
    public class CocktailObserver : IObserver<Cocktail>
    {

        private List<string> words = new List<string>(); // Polje za čuvanje reči
         

        public void OnCompleted()
        {
            SortWord();
            Console.WriteLine(); // Prazan red između svakog koktela
            Console.WriteLine();
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

        public void SortWord()
        {
            Dictionary<string, int> WordCloud = new Dictionary<string, int>();  
            foreach (string word in words)
            {
                if (WordCloud.ContainsKey(word))
                {
                    WordCloud[word]++;
                }
                else
                {
                    WordCloud[word] = 1;
                }
            }
            var SortedWordCloud = WordCloud.OrderByDescending(entry => entry.Value);

            foreach (KeyValuePair<string, int> entry in SortedWordCloud)
            {
                Console.WriteLine($"{entry.Key} - {entry.Value}");
            }
            
        }
        
    }
}



