using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slay_the_Spire_Design
{
    public class Deck
    {
        // lists - discard, draw, hand, exhaust
        public List<Card> discardPile;
        public List<Card> handPile;
        public List<Card> drawPile;
        public List<Card> exhaustPile;

        private ICommand _uiCommand;

        public Deck()
        {
            drawPile = [];
            discardPile = [];
            handPile = [];
            exhaustPile = [];
            for (int i = 0; i < 4; i++)
            {
                // create strikes, defends
                var strike = new Card("Strike", 6, 0, 1);
                var defend = new Card("Defend", 0, 5, 1);
                drawPile.Add(strike);
                drawPile.Add(defend);
            }
            // create bash
            var bash = new Card("Bash", 8, 0, 2);
            var weaken = new Card("Weaken", 2, 0, 0);
            drawPile.Add(bash);
            drawPile.Add(weaken);
            drawPile.Add(weaken);
        }

        public void DrawCards()
        {
            // hard coding variables right now
            for (int i = 0; i < 5; i++)
            {
                if (drawPile.Count > 0)
                {
                    Card card = drawPile[^1];
                    drawPile.RemoveAt(drawPile.Count - 1);
                    handPile.Add(card);
                }
                else
                {
                    TransferPiles(discardPile, drawPile);
                    _uiCommand = new SimpleCommand("Shuffling deck...");
                    _uiCommand.Execute();
                    ShufflePile(drawPile);
                    i--;
                    continue;
                }
            }
        }

        public void PrintHand()
        {
            int count = 1;
            foreach (var card in handPile)
            {
                Console.WriteLine($"\t{count}. {card.Name}");
                count++;
            }
        }

        public void ShufflePile<Card>(List<Card> pile)
        {
            Random rng = new Random();
            int n = pile.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (pile[n], pile[k]) = (pile[k], pile[n]);
            }
        }

        public void TransferPiles(List<Card> original, List<Card> target)
        {
            while (original.Count > 0)
            {
                Card card = original[^1];
                original.RemoveAt(original.Count - 1);
                target.Add(card);
            }
        }
    }

    public class Card
    {
        public string Name;
        public int Damage;
        public int Block;
        public int Cost;
        public bool Exhaust;

        public Card(string Name, int Damage, int Block, int Cost)
        {
            this.Name = Name;
            this.Damage = Damage;
            this.Block = Block;
            this.Cost = Cost;
            Exhaust = false; // blanket item for now
        }
    }
}
