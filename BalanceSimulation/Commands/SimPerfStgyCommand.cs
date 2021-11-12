using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BalanceSimulation.Commands
{
    class SimPerfStgyCommand : AbstractCommand, ICommandFactory 
    {
        public SimPerfStgyCommand(): base("SimPerfStgy") { 
        }

        override internal List<SimResult> CalcStrategy()
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

                    double monthFromPeakDec = yearStats.Stocks.Record.IndexValue / yearStats.Stocks.Peak - 1;
                    double annualIncRatio = CalcIncreaseRatio(monthFromPeakDec);

                    if (annualIncRatio > 0)
                    {
                        double avgRatio = (yearStats.Stocks.Peak + yearStats.Stocks.Record.IndexValue) / (2 * yearStats.Stocks.Record.IndexValue);
                        double incValue = avgRatio * (annualIncRatio / 12) * stockPortFolioVal;
                        if (bondPortFolioVal - incValue > 0)
                        {
                            stockPortFolioVal += incValue;
                            bondPortFolioVal -= incValue;
                        }
                        else
                        {

                        }
                    }

                    if (yearStats.Stocks.GainLastYear.HasValue)
                    {
                        double annualDecRatio = CalcDecreaseRatio((double)yearStats.Stocks.GainLastYear);

                        if (annualDecRatio > 0)
                        {
                            double gain = (double)yearStats.Stocks.GainLastYear;
                            double decRatio = (gain - (gain - 1) / 2) / gain;
                            double decValue = decRatio * stockPortFolioVal * (annualDecRatio / 12);
                            stockPortFolioVal -= decValue;
                            bondPortFolioVal += decValue;
                        }
                    }
                }

                double annualPerformance = SimResult.GetAnnualPerformance(stockPortFolioVal + bondPortFolioVal, initialValue: 100, SimulationParams.RUN_YEARS);
                runResult.AnnualizedGain = annualPerformance;

                simResults.Add(runResult);
            }
            return simResults;
        }



        public ICommand MakeCommand(string[] arguments)
        {
            return new SimPerfStgyCommand();
        }

        private double CalcIncreaseRatio(double fromPeakDecrease)
        {
            double decRatio = 0;
            int breakpoint = 0;

            while (breakpoint < SimulationParams.STGY_HIGH_BREAKPOINTS.Length && SimulationParams.STGY_LOW_BREAKPOINTS[breakpoint] >= fromPeakDecrease)
            {
                breakpoint++;
            }

            if (breakpoint > 0)
            {
                decRatio = SimulationParams.STGY_RATIOS[breakpoint - 1];
            }

            return decRatio;
        }

        private double CalcDecreaseRatio(double lastYearGain)
        {
            double incRatio = 0;
            int breakpoint = 0;

            while (breakpoint < SimulationParams.STGY_HIGH_BREAKPOINTS.Length && SimulationParams.STGY_HIGH_BREAKPOINTS[breakpoint] <= lastYearGain)
            {
                breakpoint++;
            }

            if (breakpoint > 0)
            {
                incRatio = SimulationParams.STGY_RATIOS[breakpoint - 1];
            }

            return incRatio;
        }
    }
}
