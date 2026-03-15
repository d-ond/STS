namespace Slay_the_Spire_Design
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Player player = new Player("Player", 3);
            Dummy dummy = new Dummy();
            UI ui = new UI();
            ICommand _uiCommand;
            bool MainPhase = false;

            int count = 0;

            Console.WriteLine("Start combat");
            while (true)
            {
                // draw phase general area
                count++;
                Console.WriteLine($"\tStart turn {count}");
                Console.WriteLine("\tDraw cards...");
                player.PlayerDeck.DrawCards();
                player.Energy = player.MaxEnergy;
                MainPhase = true;

                // main phase general area
                while (MainPhase)
                {
                    Console.WriteLine($"Your move? (Energy: {player.Energy} / {player.MaxEnergy})");

                    ui.PrintHand(player.PlayerDeck.handPile);

                    bool success;
                    var result = Console.ReadLine();
                    success = int.TryParse(result, out int choiceIdx);

                    if (string.IsNullOrEmpty(result))
                    {
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                    }

                    if (result.ToLower().StartsWith('e'))
                    {
                        MainPhase = false;
                        break;
                    }

                    while (!success || choiceIdx < 1 || choiceIdx > player.PlayerDeck.handPile.Count)
                    {
                        Console.WriteLine("Invalid selection. Try again.");
                        result = Console.ReadLine();
                        success = int.TryParse(result, out choiceIdx);
                    }
                    if (player.PlayerDeck.handPile[choiceIdx - 1].Cost <= player.Energy)
                    {
                        CardCommand playCard = new CardCommand(player.PlayerDeck.handPile[choiceIdx - 1], player, dummy);
                        playCard.Execute();
                        var _card = player.PlayerDeck.handPile[choiceIdx - 1];
                        player.PlayerDeck.handPile.RemoveAt(choiceIdx - 1);
                        if (_card.Exhaust)
                        {
                            player.PlayerDeck.exhaustPile.Add(_card);
                        }
                        else
                        {
                            player.PlayerDeck.discardPile.Add(_card);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not enough energy. Try again.");
                    }
                }

                // end phase
                MainPhase = false;
                Console.WriteLine("\tDiscarding...");
                player.PlayerDeck.TransferPiles(player.PlayerDeck.handPile, player.PlayerDeck.discardPile);
                Console.WriteLine("\tEnd of turn");
                Console.WriteLine("=====================================");
            }
        }
    }
}
