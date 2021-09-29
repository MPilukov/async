using System.Threading.Tasks;

namespace Async.Interfaces.Cache
{
    public interface ICache
    {
        string Get(string key);
        void Set(string key, string value);
        Task<string> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}
