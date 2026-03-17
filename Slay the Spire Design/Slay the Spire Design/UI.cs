using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Slay_the_Spire_Design
{
    public class UI
    {
        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void PrintHand(List<Card> pile)
        {
            Console.WriteLine("Your hand...");
            int count = 1;
            foreach (var card in pile)
            {
                Console.WriteLine($"\t{count}. {card.Name}: {card.Description}");
                count++;
            }
            Console.WriteLine();
        }

        public void ShowIntent(string[] intents)
        {
            Console.Write("Enemy intends to: ");
            for (int i = 0; i < intents.Length; i++)
            {
                Console.Write(intents[i]);
                if (i < intents.Length - 1)
                {
                    Console.Write(", ");
                }
                else
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        public void PrintCharacterStats(Character character)
        {
            Console.Write($"\n{character.Name}:\n\tHP: {character.HP} / {character.MaxHP}\n\tBlock: {character.Block}\n\n");
        }
    }
}
