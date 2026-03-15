using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Character()
        {
            isDead = false;
        }

        public void ModifyHP(int amount)
        {
            // block does nothing right now with HP calcs
            HP += amount;
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

        public void PrintCharacterStats()
        {
            Console.Write($"{Name}:\nHP: {HP} / {MaxHP}\nBlock: {Block}\n");
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

    public class Dummy : Character
    {
        public Dummy()
        {
            HP = 100;
            MaxHP = HP;
            Name = "Dummy";
        }
    }
}
