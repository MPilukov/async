using EasyNetQ;
using System;

namespace Async.Services.Publish
{
    public class TypeNameSerializer : ITypeNameSerializer
    {
        public Type DeSerialize(string typeName)
        {
            return Type.GetType(typeName);
        }

        public string Serialize(Type type)
        {
            if (type.FullName != null && type.FullName.Contains("EasyNetQ"))
            {
                return type + ":EasyNetQ";
            }

            return type.FullName;
        }
    }
}
