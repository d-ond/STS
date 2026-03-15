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
        private string _payload = string.Empty;

        public SimpleCommand(string payload)
        {
            _payload = payload;
        }

        public void Execute()
        {
            Console.WriteLine($"Executing command: {_payload}");
        }
    }

    public class Receiver
    {
        public void DoSomething(string a)
        {
            Console.WriteLine($"Doing {a}");
        }

        public void DoSomethingElse(string b)
        {
            Console.WriteLine($"Doing {b}");
        }
    }

    public class ComplexCommand : ICommand
    {
        private Receiver _receiver;

        public string _a;
        public string _b;

        public ComplexCommand(Receiver receiver, string a, string b)
        {
            this._receiver = receiver;
            _a = a; _b = b;
        }

        public void Execute()
        {
            Console.WriteLine("Complex command usage here.");
            _receiver.DoSomething(_a);
            _receiver.DoSomethingElse(_b);
        }
    }

    public class CardCommand : ICommand
    {
        public Card _card;
        public Character _user;
        public Character _target;
        
        public CardCommand(Card card, Character user, Character target)
        {
            _card = card;
            _user = user;
            _target = target;
        }

        public void Execute()
        {
            Console.WriteLine($"{_user.Name} uses {_card.Name}");
            _user.Energy -= _card.Cost;
            if (_card.Block != 0)
            {
                _user.ModifyBlock(_card.Block);
                Console.WriteLine($"\t{_user.Name} receives {_card.Block} block");
                _user.PrintCharacterStats();
            }
            if (_card.Damage != 0)
            {
                _target.ModifyHP(_card.Damage * -1);
                Console.WriteLine($"\t{_target.Name} took {_card.Damage} damage.");
                _target.PrintCharacterStats();
            }
        }
    }

    public class UI
    {
        public void PrintHand(List<Card> pile)
        {
            foreach (var card in pile)
            {
                Console.WriteLine(card.Name);
            }
        }
    }
}
