using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Slay_the_Spire_Design
{
    public class Effects
    {
        public string Name;
        public int Damage;
        public int Block;
        public int Vulnerable;
        public int Weak;

        public Effects(string Name, int Damage, int Block)
        {
            this.Name = Name;
            this.Damage = Damage;
            this.Block = Block;
        }
    }

    public class Card : Effects
    {
        public int Cost;
        public bool Exhaust;
        public string Description;

        public Card(string Name, int Damage, int Block, int Cost, string Description, int Vulnerable, int Weak) : base(Name, Damage, Block)
        {
            this.Name = Name;
            this.Damage = Damage;
            this.Block = Block;
            this.Cost = Cost;
            this.Description = Description;
            this.Vulnerable = Vulnerable;
            this.Weak = Weak;
            Exhaust = false; // blanket item for now
        }
    }

    public class Action : Effects
    {
        public bool isStatus;
        public int Heal;
        public string[] Intent;

        public Action(string Name, int Damage, int Block, string[] Intent) : base(Name, Damage, Block)
        {
            this.Name = Name;
            this.Damage = Damage;
            this.Block = Block;
            this.Intent = Intent;
        }
    }

    // consolidate the above two
    public class EffectActionCommand : ICommand
    {
        public Effects _effect;
        public Character _user;
        public Character _target;
        public UI _ui = new();

        public EffectActionCommand(Effects effect, Character user, Character target)
        {
            _effect = effect;
            _user = user;
            _target = target;
        }

        public void Execute()
        {
            _ui.WriteMessage($"\n{_user.Name} uses {_effect.Name}");

            if (_effect.Damage != 0)
            {
                _target.ModifyHP(_effect.Damage);
                _ui.WriteMessage($"\t{_target.Name} took {_effect.Damage} damage.");
                _ui.PrintCharacterStats(_target);
            }

            if (_effect.Block != 0)
            {
                _user.ModifyBlock(_effect.Block);
                _ui.WriteMessage($"\t{_user.Name} adds {_effect.Block} block.");
                _ui.PrintCharacterStats(_user);
            }

            if (_effect.Vulnerable != 0)
            {
                _target.Vulnerable = _effect.Vulnerable;
                _ui.WriteMessage($"\t{_target.Name} receives {_effect.Vulnerable} vulnerable.");
            }

            if (_effect.Weak != 0)
            {
                _target.Weak = _effect.Weak;
                _ui.WriteMessage($"\t{_target.Name} receives {_effect.Weak} weak.");
            }
        }
    }
}
