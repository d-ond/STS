using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Slay_the_Spire_Design
{
    public class Character
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public string Name { get; set; }
        public bool isDead;
        public int Block { get; set; }
        public int Vulnerable { get; set; }
        public int Energy { get; set; }
        public int Strength { get; set; }
        public int Weak { get; set; }

        public Character()
        {
            isDead = false;
        }

        public void ModifyHP(int amount)
        {
            if (Vulnerable > 0)
            {
                amount = (int) (amount * 1.5);
            }

            // if block is above 0, the amount will take that first
            if (Block > 0)
            {
                if (amount > Block)
                {
                    amount -= Block;
                    Block = 0;
                }
                else
                {
                    ModifyBlock(-amount);
                    return;
                }
            }

            if (amount == 0)
            {
                return;
            }

            // damages HP. simply adjusting here for ease of use
            HP -= amount;
            HP = Math.Min(MaxHP, HP);
            if (HP <= 0)
            {
                HP = 0;
                isDead = true;
            }
        }

        public void ModifyBlock(int amount)
        {
            Block += amount;
            if (Block <= 0)
            {
                Block = 0;
            }
        }

        public int DamageDealt(int amount)
        {
            // realistically this is not correct
            if (Weak > 0)
            {
                amount = (int)(amount * 0.75);
            }
            if (Strength > 0)
            {
                amount = (int)(amount * 1.25);
            }
            return amount;
        }

        public void TickStatusDown()
        {
            if (isDead)
            {
                return;
            }
            if (Vulnerable > 0)
            {
                Vulnerable -= 1;
            }
            if (Weak > 0)
            {
                Weak -= 1;
            }
        }
    }

    public class Player : Character
    {
        public Deck PlayerDeck { get; set; }
        public int MaxEnergy = 3;

        public Player(string name, int energy)
        {
            HP = 80;
            MaxHP = HP;
            Name = name;
            Energy = energy;
            PlayerDeck = new Deck();
        }
    }

    public class Enemy : Character
    {
        public List<EnemyAction> Actions;
        public EnemyAction _action { get; set; }
        public void ChooseAction()
        {
            Random random = new Random();
            int choice = random.Next(0, Actions.Count);
            _action = Actions[choice];
        }
    }

    public class Dummy : Enemy
    {
        public Dummy()
        {
            HP = 100;
            MaxHP = HP;
            Name = "Dummy";

            // overly complicated way of assigning odds of 2/5, 2/5, 1/5 to the actions. Likely better to assign an intent percentage.
            Actions = [];
            for (int i = 0; i < 2; i++)
            {
                EnemyAction damage = new EnemyAction("Attack", 7, 0, ["Damage"]);
                EnemyAction block = new EnemyAction("Block", 0, 4, ["Block"]);
                EnemyAction both = new EnemyAction("Attack and Block", 3, 3,["Damage", "Block"]);
                Actions.Add(damage);
                Actions.Add(block);
                Actions.Add(both);
            }
            Actions.RemoveAt(Actions.Count - 1);
        }
    }
}
