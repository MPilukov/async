using Async.Interfaces.Cache;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Async.Services.Cache
{
    public class Cache : ICache
    {
        private static ConnectionMultiplexer _redis;
        private static volatile string isInitialized = null;
        private static readonly object initLock = new object();

        public Cache(string connetionString)
        {
            Init(connetionString);
        }

        private void Init(string connetionString)
        {
            if (isInitialized == null)
            {
                lock (initLock)
                {
                    if (isInitialized == null)
                    {
                        _redis = ConnectionMultiplexer.Connect(connetionString);
                        isInitialized = "";
                    }
                }
            }            
        }

        public string Get(string key)
        {
            return _redis.GetDatabase().StringGet(key);
        }

        public async Task<string> GetAsync(string key)
        {
            var response = await _redis.GetDatabase().StringGetAsync(key);
            return response;
        }

        public void Set(string key, string value)
        {
            _redis.GetDatabase().StringSet(key, value);
        }

        public Task SetAsync(string key, string value)
        {
            return _redis.GetDatabase().StringSetAsync(key, value);
        }
    }
}
