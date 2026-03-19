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
        public List<EffectActionCommand> ActionQueue;
        public int turnCount = 0;
        public UI ui = new();
        public Player player;
        //public Dummy dummy;
        public bool MainPhase = false;
        public bool BattleActive = true;

        // enemy party
        public List<Enemy> enemyParty;
        public List<Enemy> aliveEnemyParty = [];

        public TurnManager(Player player, List<Enemy> enemyParty)
        {
            this.player = player;
            this.enemyParty = enemyParty;
            ActionQueue = [];
            foreach (var enemy in enemyParty)
            {
                aliveEnemyParty.Add(enemy);
            }
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
            foreach (Enemy enemy in aliveEnemyParty)
            {
                enemy.ChooseAction();
            }

            // tick down stats 
            player.TickStatusDown();
        }

        public void StartMainPhase()
        {
            // player picks cards until they end turn
            while (MainPhase)
            {
                ui.WriteMessage($"Your move? (Energy: {player.Energy} / {player.MaxEnergy})");
                foreach (var enemy in aliveEnemyParty)
                {
                    ui.ShowIntent(enemy._action.Intent);
                }

                ui.PrintHand(player.PlayerDeck.handPile);

                bool success;
                var result = Console.ReadLine();
                success = int.TryParse(result, out int choiceIdx);

                if (string.IsNullOrEmpty(result))
                {
                    ui.WriteMessage("Exiting...");
                    Environment.Exit(0);
                }

                // this is technically where the turn ends
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
                    EffectActionCommand playCard;
                    var tchoiceIdx = 1;

                    if (player.PlayerDeck.handPile[choiceIdx - 1].isSelfTargeted)
                    {
                        playCard = new(player.PlayerDeck.handPile[choiceIdx - 1], player, player);
                    }

                    else if (player.PlayerDeck.handPile[choiceIdx - 1].isTargetable)
                    {
                        ui.PrintEnemyParty(aliveEnemyParty);

                        bool tsuccess;
                        var tresult = Console.ReadLine();
                        tsuccess = int.TryParse(tresult, out tchoiceIdx);

                        while (!tsuccess || tchoiceIdx < 1 || tchoiceIdx > aliveEnemyParty.Count)
                        {
                            int aliveCount = aliveEnemyParty.Count;
                            if (aliveCount == 0)
                            {
                                BattleActive = false;
                                break;
                            }
                            if (aliveCount == 1)
                            {
                                tchoiceIdx = 1;
                                break;
                            }
                            ui.WriteMessage("Invalid selection. Try again.");
                            tresult = Console.ReadLine();
                            tsuccess = int.TryParse(tresult, out tchoiceIdx);
                        }
                        playCard = new(player.PlayerDeck.handPile[choiceIdx - 1], player, aliveEnemyParty[tchoiceIdx - 1]);
                    }
                    else
                    {
                        playCard = new(new Card("", 0, 0, 0, "", 0, 0, false, false), null, null); // just dont do anything itll crash anyways
                        Environment.Exit(0);
                    }

                    playCard.Execute();

                    // will need to be a check for enemy hit
                    BattleActive = !IsBattleOver();

                    if (!BattleActive) { MainPhase = false; break; }

                    player.Energy -= player.PlayerDeck.handPile[choiceIdx - 1].Cost;

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
            foreach (var enemy in aliveEnemyParty)
            {
                enemy.Block = 0;
            }

            foreach (var enemy in aliveEnemyParty)
            {
                EffectActionCommand enemyAction = new(enemy._action, enemy, player);
                enemyAction.Execute();
                BattleActive = !IsBattleOver();
                if (!BattleActive)
                {
                    break;
                }
            }

            if (!BattleActive)
            {
                return;
            }

            // tick down stats 
            foreach (var enemy in aliveEnemyParty)
            {
                enemy.TickStatusDown();
            }

            ui.WriteMessage("=====================================");
        }

        // rudimentary check - will need to instead be a notification for "hp modified" or the like
        public bool IsBattleOver()
        {
            if (player.isDead)
            {
                return true;
            }

            // TODO separate this into new function
            for (int i = 0; i < aliveEnemyParty.Count; i++)
            {
                if (aliveEnemyParty[i].isDead)
                {
                    aliveEnemyParty.RemoveAt(i);
                    i--;
                }
            }
            if (aliveEnemyParty.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
