using EasyNetQ;

namespace Async.Services.Publish
{
    public class MyConventions : Conventions
    {
        public MyConventions(ITypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
        {
            ExchangeNamingConvention = type => $"{type.FullName}, Exchange";
            QueueNamingConvention = (type, id) => $"{type.FullName}, Queue";
            ErrorQueueNamingConvention = info => "ErrorQueue";
            ErrorExchangeNamingConvention = info => "BusErrorExchange_" + info.RoutingKey;
        }
    }
}
