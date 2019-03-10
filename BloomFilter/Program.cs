using System;
using System.Threading.Tasks;

namespace BloomFilter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var bf = new BloomFilter("__ENGLISH_COMMON_WORDS__");

            await bf.AddEntriesAsync("words.txt");

            while (true)
            {
                var line = Console.ReadLine();

                if (line == "END")
                {
                    break;
                }

                var result = await bf.EntryExistsAsync(line);

                Console.WriteLine($"Word: {line}; Result: {result}");
            }
        }
    }
}
