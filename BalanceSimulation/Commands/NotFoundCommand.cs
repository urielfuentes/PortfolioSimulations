using System;

namespace BalanceSimulation.Commands
{
    class NotFoundCommand : ICommand
    {
        public string CommandName { get; set; }
        public void Execute()
        {
            Console.WriteLine("Couldn't find command: " + CommandName);
        }
    }
}
