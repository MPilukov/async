using Async.Interfaces.Publish;
using System;

namespace RabbitMq.Messages.Books
{
    public class CreateBookMessage : Message
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
