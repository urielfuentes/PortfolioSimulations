using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BalanceSimulation
{
    public class SimResult
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double AnnualizedGain { get; set; }

        public static double GetAnnualPerformance(double finalValue, double initialValue, int years)
        {
            return Math.Pow(finalValue / initialValue, 1.0 / (double)years);
        }

        public static bool SaveResults(List<SimResult> results, string commandName)
        {
            try
            {
                using var writer = new StreamWriter($"{commandName}_results.csv");
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(results);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {commandName}");
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
