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
                var strike = new Card("Strike", 6, 0, 1, "Deal 6 damage", 0, 0, true, false);
                var defend = new Card("Defend", 0, 5, 1, "Block 5 damage", 0, 0, false, true);
                drawPile.Add(strike);
                drawPile.Add(defend);
            }
            // create bash
            var bash = new Card("Bash", 8, 0, 2, "Deal 8 damage and apply 1 vulnerable", 1, 0, true, false);
            var weaken = new Card("Weaken", 2, 0, 0, "Deal 2 damage and apply 1 weak", 0, 1, true, false);
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
}
