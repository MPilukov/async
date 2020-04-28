using EasyNetQ;

namespace Async.Services.Publish
{
    public class MyConventions : Conventions
    {
        public MyConventions(ITypeNameSerializer typeNameSerializer) : base(typeNameSerializer)
        {
            ExchangeNamingConvention = type =>
            {
                return $"{type.FullName}, Exchange";
            };

            QueueNamingConvention = (type, id) =>
            {
                return $"{type.FullName}, Queue";
            };

            ErrorQueueNamingConvention = info => "ErrorQueue";
            ErrorExchangeNamingConvention = info => "BusErrorExchange_" + info.RoutingKey;
        }
    }
}
