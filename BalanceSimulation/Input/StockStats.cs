using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BalanceSimulation
{
    class StockStats
    {
        public static List<StockStats> StatsRecords = new List<StockStats>();
        public SecurityRecord Record { get; set; }

        public double? GainLastYear { get; set; }

        public double GainLastMonth { get; set; } = 1.0;

        public Queue<double> MonthlyGainsInLastYear { get; set; } = new Queue<double>();

        public double Peak { get; set; }

        public static void CalcStocksStats(List<StockStats> stocksStats)
        {
            using (var reader = new StreamReader("Input\\us_stocks.csv"))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var stockRecords = csvReader.GetRecords<SecurityRecord>();
                foreach (var stockRecord in stockRecords)
                {
                    var newStockStats = new StockStats();
                    newStockStats.Record = stockRecord;
                    var lastStockStats = stocksStats.LastOrDefault();
                    if (lastStockStats != null)
                    {
                        double recordMonthlyGain = (stockRecord.IndexValue / lastStockStats.Record.IndexValue);
                        newStockStats.GainLastMonth = recordMonthlyGain;
                        //Security index is growing
                        if ((stockRecord.IndexValue / lastStockStats.Peak) - 1 > -0.05)
                        {
                            double newGainInYear;
                            Queue<double> newMonthltyGains;

                            if (newStockStats.Record.IndexValue > lastStockStats.Peak)
                            {
                                newStockStats.Peak = newStockStats.Record.IndexValue;
                            }
                            else
                            {
                                newStockStats.Peak = lastStockStats.Peak;
                            }

                            if (lastStockStats.MonthlyGainsInLastYear.Count() == 12)
                            {
                                newMonthltyGains = new Queue<double>(lastStockStats.MonthlyGainsInLastYear);
                                newGainInYear = (double)lastStockStats.GainLastYear;
                                newGainInYear /= newMonthltyGains.Dequeue();
                                newGainInYear *= recordMonthlyGain;
                                newMonthltyGains.Enqueue(recordMonthlyGain);
                                newStockStats.MonthlyGainsInLastYear = newMonthltyGains;
                                newStockStats.GainLastYear = newGainInYear;
                            }
                            else
                            {
                                newGainInYear = (double)lastStockStats.GainLastYear;
                                newGainInYear *= recordMonthlyGain;
                                newMonthltyGains = new Queue<double>(lastStockStats.MonthlyGainsInLastYear);
                                newMonthltyGains.Enqueue(recordMonthlyGain);
                                newStockStats.GainLastYear = newGainInYear;
                                newStockStats.MonthlyGainsInLastYear = newMonthltyGains;
                            }
                        }
                        else
                        {
                            newStockStats.Peak = lastStockStats.Peak;
                            newStockStats.GainLastYear = 1;
                        }
                    }
                    else
                    {
                        newStockStats.Peak = stockRecord.IndexValue;
                        newStockStats.GainLastYear = 1;
                    }
                    stocksStats.Add(newStockStats);
                }
            }
        }
    }
}
