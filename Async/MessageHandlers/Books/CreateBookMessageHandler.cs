using Async.Interfaces.Cache;
using Async.Interfaces.Publish;
using RabbitMq.Messages.Books;
using System;
using System.Threading.Tasks;

namespace Async.MessageHandlers.Books
{
    public class CreateBookMessageHandler : MessageHandler<CreateBookMessage>
    {
        private readonly ICache _cache;
        public CreateBookMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public override Task Handle(CreateBookMessage message)
        {
            Console.WriteLine($"Пришло сообщение для создания книги : ({message.Id}, {message.Price}, {message.Title})");

            _cache.Set($"Book_{message.Id}",
                @"{""Id"":""" + message.Id + @""", ""Title"":""" 
                + message.Title + @"""}");

            return Task.FromResult(0);
        }
    }
}
