using System;
using System.Collections.Generic;
using System.Text;

namespace Async.Models.Books
{
    [Serializable]
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
