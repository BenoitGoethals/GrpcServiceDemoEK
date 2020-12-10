using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServiceBook.Services
{
    public class BookService:LibBook.LibBookBase
    {
        private readonly ILogger<BookService> _logger;

        public BookService(ILogger<BookService> logger)
        {
            _logger = logger;
        }

        public override Task<BookCollection> Books(Empty request, ServerCallContext context)
        {
            IList<Book> books = new List<Book>()
            {
                new Book(){Author = "benoit",Genre = Genre.Novel,Isbn = "dfdsfdf656556",Pages=500,Published=Timestamp.FromDateTime(DateTime.UtcNow),Id=1,Language="NL",Title="war"},
                new Book(){Author = "benoit",Genre = Genre.Novel,Isbn = "dfdsfdf656556",Pages=500,Published=Timestamp.FromDateTime(DateTime.UtcNow),Id=2,Language="BE",Title="war2"},
                new Book(){Author = "ilse",Genre = Genre.Novel,Isbn = "dfdsfdf656556",Pages=500,Published=Timestamp.FromDateTime(DateTime.UtcNow),Id=3,Language="NL",Title="war3"},
                new Book(){Author = "mais",Genre = Genre.Novel,Isbn = "dfdsfdf656556",Pages=500,Published=Timestamp.FromDateTime(DateTime.UtcNow),Id=4,Language="NL",Title="war4"},
                new Book(){Author = "Ikke",Genre = Genre.Novel,Isbn = "dfdsfdf656556",Pages=500,Published=Timestamp.FromDateTime(DateTime.UtcNow),Id=5,Language="NL",Title="war5"},
            };
            var reply= new BookCollection(){Book={books}};
            return Task.FromResult(reply);
        }
    }
}
