using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slay_the_Spire_Design
{
    public class TurnManager
    {
        public List<CardCommand> ActionQueue;
        public int count = 1;
        public bool battleActive = false;

        public TurnManager()
        {
            Character player = new Character();
            ActionQueue = new List<CardCommand>();
        }

        public void StartTurn()
        {
            // reset energy, reset block, draw cards
        }

        public void MainPhase()
        {
            // player picks cards until they end turn
        }

        public void EndPlayerTurn()
        {
            // discard remaining hand, trigger end-of-turn effects
        }

        public void EnemyTurn()
        {
            // enemy picks its move, resolves it through card commands
        }
    }
}
