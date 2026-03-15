using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Slay_the_Spire_Design
{
    public class TurnManager
    {
        public List<CardCommand> ActionQueue;
        public int turnCount = 0;
        public UI ui = new();
        public Player player;
        public Dummy dummy;
        public bool MainPhase = false;
        public bool BattleActive = true;

        public TurnManager(Player player, Dummy dummy)
        {
            this.player = player;
            this.dummy = dummy;
            ActionQueue = new List<CardCommand>();
        }

        public void RunBattle()
        {
            while (BattleActive)
            {
                StartTurn();
                if (!BattleActive) { break; }
                StartMainPhase();
                if (!BattleActive) { break; }
                StartEndPhase();
                if (!BattleActive) { break; }
                StartEnemyTurn();
                if (!BattleActive) { break; }
            }
        }

        public void StartTurn()
        {
            // reset energy, reset block, draw cards
            turnCount++;
            ui.WriteMessage($"\tStart turn {turnCount}\n");

            ui.WriteMessage("\tDraw cards...\n");

            player.PlayerDeck.DrawCards();
            player.Energy = player.MaxEnergy;
            player.Block = 0;
            MainPhase = true;

            // get the enemy action
            dummy.ChooseAction();
        }

        public void StartMainPhase()
        {
            // player picks cards until they end turn
            while (MainPhase)
            {
                ui.WriteMessage($"Your move? (Energy: {player.Energy} / {player.MaxEnergy})");
                ui.ShowIntent(dummy._action.Intent);

                ui.PrintHand(player.PlayerDeck.handPile);

                bool success;
                var result = Console.ReadLine();
                success = int.TryParse(result, out int choiceIdx);

                if (string.IsNullOrEmpty(result))
                {
                    ui.WriteMessage("Exiting...");
                    Environment.Exit(0);
                }

                if (result.ToLower().StartsWith('e'))
                {
                    MainPhase = false;
                    break;
                }

                while (!success || choiceIdx < 1 || choiceIdx > player.PlayerDeck.handPile.Count)
                {
                    ui.WriteMessage("Invalid selection. Try again.");
                    result = Console.ReadLine();
                    success = int.TryParse(result, out choiceIdx);
                }
                if (player.PlayerDeck.handPile[choiceIdx - 1].Cost <= player.Energy)
                {
                    CardCommand playCard = new(player.PlayerDeck.handPile[choiceIdx - 1], player, dummy);
                    playCard.Execute();

                    BattleActive = !IsBattleOver();
                    if (!BattleActive) { MainPhase = false; break; }

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
                    ui.WriteMessage("Not enough energy. Try again.");
                }
            }
        }

        public void StartEndPhase()
        {
            // discard remaining hand, trigger end-of-turn effects
            MainPhase = false;
            ui.WriteMessage("\tDiscarding...");
            player.PlayerDeck.TransferPiles(player.PlayerDeck.handPile, player.PlayerDeck.discardPile);
            ui.WriteMessage("\tEnd of turn");
        }

        public void StartEnemyTurn()
        {
            // enemy picks its move, resolves it through card commands
            ui.WriteMessage("\tEnemy turn...");
            dummy.Block = 0;
            EnemyCommand enemyAction = new(dummy._action, dummy, player);
            enemyAction.Execute();
            BattleActive = !IsBattleOver();

            ui.WriteMessage("=====================================");
        }

        // rudimentary check - will need to instead be a notification for "hp modified" or the like
        public bool IsBattleOver()
        {
            if (dummy.isDead || player.isDead)
            {
                return true;
            }
            return false;
        }
    }
}
