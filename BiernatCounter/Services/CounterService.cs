using BiernatCounter.Models;
using System.Collections.Generic;
using System.IO;

namespace BiernatCounter.Services
{
    public class CounterService
    {
        private const string FileName = "counters.txt";
        private string FilePath => Path.Combine(FileSystem.Current.AppDataDirectory, FileName);

        public List<CounterModel> LoadCounters()
        {
            var counters = new List<CounterModel>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int value))
                    {
                        counters.Add(new CounterModel { Name = parts[0], InitialValue = value });
                    }
                }
            }
            return counters;
        }

        public void SaveCounters(List<CounterModel> counters)
        {
            using (var writer = new StreamWriter(FilePath))
            {
                foreach (var counter in counters)
                {
                    writer.WriteLine($"{counter.Name}:{counter.InitialValue}");
                }
            }
        }
    }
}
