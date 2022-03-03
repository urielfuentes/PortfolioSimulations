using System.Collections.Generic;
using System.Linq;

namespace BalanceSimulation.Commands
{
    /// <summary>
    /// Rebalance strategy based in assets wheights. 
    /// When the weight of stcoks is below or above a threshold rebalancing is made.
    /// </summary>

    class SimBalanceStgyCommand : AbstractCommand, ICommandFactory
    {
        public SimBalanceStgyCommand() : base("SimBalanceStgy") { }

        public SimBalanceStgyCommand(string commandName, string[] options) : base(commandName, options) { }

        override internal List<SimResult> CalculateStrategy()
        {
            var yearsStats = StockStats.StatsRecords.Zip(BondStats.StatsRecords, (s, b) => new { Stocks = s, Bonds = b });

            var simResults = new List<SimResult>();

            for (int run = 0; run < (yearsStats.Count() - SimulationParams.RUN_YEARS * 12); run++)
            {
                var firstRecord = yearsStats.Skip(run).FirstOrDefault();

                var runResult = new SimResult
                {
                    Year = firstRecord.Bonds.Record.Year,
                    Month = firstRecord.Bonds.Record.Month
                };

                double stockPortFolioVal = 100 * SimulationParams.STOCKS_INIT_RATIO;
                double bondPortFolioVal = 100 * (1 - SimulationParams.STOCKS_INIT_RATIO);

                foreach (var yearStats in yearsStats.Skip(run).Take(SimulationParams.RUN_YEARS * 12))
                {
                    bondPortFolioVal *= yearStats.Bonds.GainLastMonth;
                    stockPortFolioVal *= yearStats.Stocks.GainLastMonth;
                    double stocksPerc = stockPortFolioVal / (stockPortFolioVal + bondPortFolioVal);

                    if (stocksPerc > SimulationParams.BALANCING_HIGH || stocksPerc < SimulationParams.BALANCING_LOW)
                    {
                        double targetValueStocks = (stockPortFolioVal + bondPortFolioVal) * 0.8;
                        double targetValueBonds = (stockPortFolioVal + bondPortFolioVal) * 0.2;
                        stockPortFolioVal = targetValueStocks;
                        bondPortFolioVal = targetValueBonds;
                    }
                }

                double annualPerformance = SimResult.GetAnnualPerformance(stockPortFolioVal + bondPortFolioVal, 100, SimulationParams.RUN_YEARS);
                runResult.AnnualizedGain = annualPerformance;
                simResults.Add(runResult);
            }

            return simResults;
        }

        public ICommand MakeCommand(string[] arguments)
        {
            //pass command name and arguments
            return new SimBalanceStgyCommand(arguments[0], arguments.Skip(1).ToArray());
        }
    }
}
