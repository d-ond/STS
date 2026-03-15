using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slay_the_Spire_Design
{
    // commands for making choices
    public interface ICommand
    {
        void Execute();
    }

    public class SimpleCommand : ICommand
    {
        public UI _ui = new();
        private string _payload = string.Empty;

        public SimpleCommand(string payload)
        {
            _payload = payload;
        }

        public void Execute()
        {
            _ui.WriteMessage($"Executing command: {_payload}");
        }
    }

    public class Receiver
    {
        public UI _ui = new();

        public void DoSomething(string a)
        {
            _ui.WriteMessage($"Doing {a}");
        }

        public void DoSomethingElse(string b)
        {
            _ui.WriteMessage($"Doing {b}");
        }
    }

    public class ComplexCommand : ICommand
    {
        private Receiver _receiver;
        public UI _ui = new();

        public string _a;
        public string _b;

        public ComplexCommand(Receiver receiver, string a, string b)
        {
            this._receiver = receiver;
            _a = a; _b = b;
        }

        public void Execute()
        {
            _ui.WriteMessage("Complex command usage here.");
            _receiver.DoSomething(_a);
            _receiver.DoSomethingElse(_b);
        }
    }

    public class CardCommand : ICommand
    {
        public Card _card;
        public Character _user;
        public Character _target;
        public UI _ui = new();

        public CardCommand(Card card, Character user, Character target)
        {
            _card = card;
            _user = user;
            _target = target;
        }

        public void Execute()
        {
            _ui.WriteMessage($"\n{_user.Name} uses {_card.Name}");
            _user.Energy -= _card.Cost;
            if (_card.Block != 0)
            {
                _user.ModifyBlock(_card.Block);
                _ui.WriteMessage($"\t{_user.Name} receives {_card.Block} block");
                _ui.PrintCharacterStats(_user);
            }
            if (_card.Damage != 0)
            {
                _target.ModifyHP(_card.Damage);
                _ui.WriteMessage($"\t{_target.Name} took {_card.Damage} damage.");
                _ui.PrintCharacterStats(_target);
            }
        }
    }
        

    public class EnemyCommand : ICommand
    {
        public Action _action;
        public Character _user;
        public Character _target;
        public UI _ui = new();

        public EnemyCommand(Action action, Character user, Character target)
        {
            _action = action;
            _user = user;
            _target = target;
        }

        public void Execute()
        {
            _ui.WriteMessage($"\n{_user.Name} uses {_action.Name}");

            if (_action.Damage != 0)
            {
                _target.ModifyHP(_action.Damage);
                _ui.WriteMessage($"\t{_target.Name} took {_action.Damage} damage.");
                _ui.PrintCharacterStats(_target);
            }

            if (_action.Block != 0)
            {
                _user.ModifyBlock(_action.Block);
                _ui.WriteMessage($"\t{_user.Name} adds {_action.Block} block.");
                _ui.PrintCharacterStats(_user);
            }
        }
    }
}
