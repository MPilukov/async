using System;
using System.Threading.Tasks;

namespace Async.Interfaces.Logger
{
    public interface ILogElasticClient
    {
        Task Log(string index, DateTime time, string message, string level, string exception = null, 
            string request = null, string module = null);
    }
}
