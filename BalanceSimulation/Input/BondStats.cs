using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;

namespace BalanceSimulation
{
    class BondStats
    {
        public static List<BondStats> StatsRecords = new List<BondStats>();
        public SecurityRecord Record { get; set; }
        public double GainLastMonth { get; set; } = 1.0;

        public static void CalcBondsStats(List<BondStats> bondsStats)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var reader = new StreamReader("Input\\us_bonds.csv"))
            using (var csvReader = new CsvReader(reader, config))
            {
                var bondRecords = csvReader.GetRecords<SecurityRecord>();
                foreach (var bondRecord in bondRecords)
                {
                    var newBondStats = new BondStats { Record = bondRecord };
                    var lastBondStats = bondsStats.LastOrDefault();
                    if (lastBondStats != null)
                    {
                        newBondStats.GainLastMonth = bondRecord.IndexValue / lastBondStats.Record.IndexValue;
                    }
                    bondsStats.Add(newBondStats);
                }
            }
        }
    }
}
