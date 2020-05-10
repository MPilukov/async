using Async.Interfaces.Cache;
using Async.Interfaces.Publish;
using Async.Models.Books;
using Newtonsoft.Json;
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

        public override async Task Handle(CreateBookMessage message)
        {
            Console.WriteLine($"Пришло сообщение для создания книги : ({message.Id}, {message.Price}, {message.Title})");

            var book = new Book
            {
                Id = message.Id,
                Title = message.Title,
                Price = message.Price,
            };

            var bookStr = JsonConvert.SerializeObject(book);

            await _cache.SetAsync($"Book_{message.Id}", bookStr);
            Console.WriteLine($"Сохранили информацию о книге : {message.Id}");
        }
    }
}
