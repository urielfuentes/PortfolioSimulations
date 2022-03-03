using System;
using System.Collections.Generic;
using System.Linq;

namespace BalanceSimulation.Commands
{
    abstract class AbstractCommand : ICommand
    {
        readonly string _commandName;
        readonly string[] _options;

        public string CommandName { get => _commandName; }

        public AbstractCommand(string commandName)
        {
            _commandName = commandName;
        }

        public AbstractCommand(string commandName, string[] options) : this (commandName)
        {
            _options = options;
        }

        public void Execute()
        {
            Console.WriteLine("Simulating the strategy.");
            var simResults = CalculateStrategy();
            Console.WriteLine("Finished simulating the strategy.");

            if (IsSaveInOptions())
            {
                SimResult.SaveResults(simResults, CommandName);
                Console.WriteLine("Finished saving the results.");
            }

            ShowSummary(simResults);
        }

        private bool IsSaveInOptions()
        {
            bool result = false;
            if (_options != null)
            {
                result = _options.Any(o => o == "--save-results");
            }
            return result;
        }

        private static void ShowSummary(List<SimResult> simResults)
        {
            int samples = simResults.Count();
            int startYear = simResults.First().Year;
            int endYear = simResults.Last().Year;

            double avgAnnualGain = simResults.Sum(r => r.AnnualizedGain) / samples;
            Console.WriteLine($"\nAverage annual gain = {(avgAnnualGain - 1) * 100:F2}, number of samples = {samples}");
            Console.WriteLine($"Start year = {startYear}, End year = {endYear}");
        }

        internal abstract List<SimResult> CalculateStrategy();
    }
}
