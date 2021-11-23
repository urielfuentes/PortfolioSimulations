using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceSimulation.Commands
{
    abstract class AbstractCommand : ICommand
    {
        readonly string _commandName;
        public string CommandName { get => _commandName; }
        
        public AbstractCommand(string commandName)
        {
            _commandName = commandName;
        }

        public void Execute()
        {
            Console.WriteLine("Simulating the strategy.");
            var simResults = CalculateStrategy();
            Console.WriteLine("Finished simulating the strategy.");
            SimResult.SaveResults(simResults, CommandName);
            Console.WriteLine("Finished saving the results.");
        }

        internal abstract List<SimResult> CalculateStrategy();
    }
}
