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
}
