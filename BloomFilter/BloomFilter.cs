using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BloomFilter
{
    public class BloomFilter
    {
        private readonly string _dictName;

        private readonly RedisService _redisService;

        public BloomFilter(string dictName)
        {
            _dictName = dictName;
            _redisService = new RedisService();
        }

        public async Task AddEntriesAsync(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            using (var streamReader = new StreamReader(fileStream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = await streamReader.ReadLineAsync();

                    var hashes = line.Hash();
                    var offsets = hashes.Select(Convert.ToInt64).ToArray();

                    await _redisService.MultiSetBitAsync(_dictName, true, offsets);
                }
            }
        }

        public async Task<bool> EntryExistsAsync(string entry)
        {
            var hashes = entry.Hash();
            var offsets = hashes.Select(Convert.ToInt64).ToArray();
            var result = await _redisService.MultiGetBitAsync(_dictName, offsets);

            return result.All(bit => bit);
        }
    }
}
