using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceSimulation.Commands
{
    class NotFoundCommand : ICommand
    {
        public string Name { get; set; }
        public void Execute()
        {
            Console.WriteLine("Couldn't find command: " + Name);
        }
    }
}
