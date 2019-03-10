using System.Collections.Generic;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace BloomFilter
{
    public class RedisService
    {
        private static readonly ConnectionMultiplexer _conn;

        private static IDatabase _db;

        static RedisService()
        {
            _conn = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        }

        public RedisService()
        {
            _db = _conn.GetDatabase();
        }

        public async Task MultiSetBitAsync(string name, bool value, params long[] offsets)
        {
            foreach (var offset in offsets)
            {
                await _db.StringSetBitAsync(name, offset, value);
            }
        }

        public async Task<List<bool>> MultiGetBitAsync(string name, params long[] offsets)
        {
            var result = new List<bool>();

            foreach (var offset in offsets)
            {
                var bit = await _db.StringGetBitAsync(name, offset);
                result.Add(bit);
            }

            return result;
        }
    }
}
