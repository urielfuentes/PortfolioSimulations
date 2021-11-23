using BalanceSimulation.Commands;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BalanceSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            StockStats.CalcStocksStats(StockStats.StatsRecords);
            BondStats.CalcBondsStats(BondStats.StatsRecords);

            var availableCommands = GetAvailableCommands();

            if (args.Length == 0)
            {
                PrintUsage(availableCommands);
                return;
            }

            var parser = new CommandParser(availableCommands);
            var command = parser.ParseCommand(args);
            command.Execute();
        }

        private static void CalcThresholds(List<StockStats> recordsStats)
        {
            Console.WriteLine("High Threshold");
            Console.WriteLine("Perc %\tTimes\t%Total");
            for (double highThreshold = 1.25; highThreshold < 1.50; highThreshold += 0.01)
            {
                int numHigh = recordsStats.Count(r => r.GainLastYear > highThreshold);
                Console.WriteLine($"{highThreshold * 100 - 100:0.##}\t{numHigh}\t{(double)numHigh / 1800 * 100:0.##}");
            }

            Console.WriteLine("Low Threshold");
            Console.WriteLine("Perc %\tTimes\t%Total");
            for (double lowThreshold = -0.27; lowThreshold > -0.6; lowThreshold -= 0.01)
            {
                int numLow = recordsStats.Count(r => r.Record.IndexValue / r.Peak - 1 < lowThreshold);
                Console.WriteLine($"{lowThreshold * 100:0.##}\t{numLow}\t{(double)numLow / 1800 * 100:0.##}");
            }
        }

        static IEnumerable<ICommandFactory> GetAvailableCommands()
        {
            return new ICommandFactory[]
                {
                    new SimBalanceStgyCommand(),
                    new SimPerfStgyCommand(),
                    new SimNoStgyCommand()
                };
        }

        private static void PrintUsage(IEnumerable<ICommandFactory> availableCommands)
        {
            Console.WriteLine("Usage: LoggingDemo CommandName Arguments");
            Console.WriteLine("Commands:");
            foreach (var command in availableCommands)
                Console.WriteLine("  {0}", command.CommandName);
            Console.WriteLine("Options:");
            Console.WriteLine("  --save-results");
        }
    }
}
