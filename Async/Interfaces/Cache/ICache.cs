using System;
using System.Collections.Generic;
using System.Text;
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
