using Async.Interfaces.Cache;
using Async.Interfaces.Logger;
using Async.Interfaces.Publish;
using Async.Models.Books;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Async.Messages.Books;

namespace Async.MessageHandlers.Books
{
    public class CreateBookMessageHandler : MessageHandler<CreateBookMessage>
    {
        private readonly ICache _cache;
        private readonly ILogger _logger;

        public CreateBookMessageHandler(ICache cache, ILogger logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public override async Task Handle(CreateBookMessage message)
        {
            _logger.Info($"Start book create : ({message.Id}, {message.Price}, {message.Title})");

            var book = new Book
            {
                Id = message.Id,
                Title = message.Title,
                Price = message.Price,
            };

            var bookStr = JsonConvert.SerializeObject(book);

            await _cache.SetAsync($"Book_{message.Id}", bookStr);
            _logger.Info($"Book saved : {message.Id}");
        }
    }
}
