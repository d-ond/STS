namespace Slay_the_Spire_Design
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Player player = new("Player", 3);
            Dummy dummy = new();

            UI ui = new();

            TurnManager turnManager = new(player, dummy);

            // general loop - will need to have the battle true or false depending on factors (outcomes)
            ui.WriteMessage("==== START COMBAT ====\n");
            turnManager.RunBattle();
            ui.WriteMessage("===== END COMBAT =====\n");
        }
    }
}
