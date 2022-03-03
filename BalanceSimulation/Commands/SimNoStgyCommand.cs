using System.Collections.Generic;
using System.Linq;

namespace BalanceSimulation.Commands
{
    /// <summary>
    /// Calculates the performance in different periods when no rebalancing is made from the capital.
    /// </summary>

    class SimNoStgyCommand : AbstractCommand, ICommandFactory
    {

        public SimNoStgyCommand() : base("SimNoStgy") { }

        public SimNoStgyCommand(string commandName, string[] options) : base(commandName, options){ }

        override internal List<SimResult> CalculateStrategy()
        {
            var yearsStats = StockStats.StatsRecords.Zip(BondStats.StatsRecords, (s, b) => new { Stocks = s, Bonds = b });

            var simResults = new List<SimResult>();

            for (int run = 0; run <= (yearsStats.Count() - SimulationParams.RUN_YEARS * 12); run++)
            {
                var firstRecord = yearsStats.Skip(run).FirstOrDefault();
                var lastRecord = yearsStats.Skip(run + SimulationParams.RUN_YEARS * 12 - 1).FirstOrDefault();

                var runResult = new SimResult
                {
                    Year = firstRecord.Bonds.Record.Year,
                    Month = firstRecord.Bonds.Record.Month
                };

                double initialStocksVal = firstRecord.Stocks.Record.IndexValue * SimulationParams.STOCKS_INIT_RATIO;
                double initialBondsVal = firstRecord.Bonds.Record.IndexValue * (1 - SimulationParams.STOCKS_INIT_RATIO);
                double initialPortFolioVal = initialStocksVal + initialBondsVal;

                double finalStocksVal = lastRecord.Stocks.Record.IndexValue * SimulationParams.STOCKS_INIT_RATIO;
                double finalBondsVal = lastRecord.Bonds.Record.IndexValue * (1 - SimulationParams.STOCKS_INIT_RATIO);
                double finalPortFolioVal = finalStocksVal + finalBondsVal;

                double annualPerformance = SimResult.GetAnnualPerformance(finalPortFolioVal, initialPortFolioVal, SimulationParams.RUN_YEARS);
                runResult.AnnualizedGain = annualPerformance;
                simResults.Add(runResult);
            }

            return simResults;
        }

        public ICommand MakeCommand(string[] arguments)
        {
            //pass command name and arguments
            return new SimNoStgyCommand(arguments[0], arguments.Skip(1).ToArray());
        }
    }
}
