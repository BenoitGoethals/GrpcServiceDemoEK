using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceBook.storage;
using Microsoft.Extensions.Logging;

namespace GrpcServiceBook.Services
{
    public class BookService:LibBook.LibBookBase
    {
        private readonly ILogger<BookService> _logger;
        private readonly IBookStorage _bookStorage;
        private IMapper _mapper;
        public BookService(ILogger<BookService> logger, IBookStorage bookStorage, IMapper mapper)
        {
            _logger = logger;
            _bookStorage = bookStorage;
            _mapper = mapper;
        }

        public override Task<BookCollection> Books(Empty request, ServerCallContext context)
        {
            
            
            IList<Book> books=new List<Book>();
            _bookStorage.Books().ForEach(b=>
            {
                b.Genre ??= Common.model.Genre.Novel;
                books.Add(new Book() { Isbn = b.Isbn,Author = b.Author,Genre =  (Genre) b.Genre,Id = b.Id,Language = b.Language,Pages = b.Pages,Published = new Timestamp(),Title = b.Title});
            });
            var reply= new BookCollection(){Book={books}};
            return Task.FromResult(reply);
        }


        public override Task<Book> GetBook(RequestIsbn request, ServerCallContext context) 
        {
          
            var book = _bookStorage.GetBook(isbn: request.Isbn);
            if (book == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound,$"{request.Isbn} Not Found"));
            }
            book.Genre ??= Common.model.Genre.Novel;
            var reply = new Book()
            {
                Isbn = book.Isbn, Author = book.Author, Genre = (Genre) book.Genre, Id = book.Id, Language = book.Language,
                Pages = book.Pages, Published = new Timestamp(), Title = book.Title
            };


            return Task.FromResult(reply);
        }


        public override Task<Book> insertBook(Book book, ServerCallContext context)
        {
         
            var reply = new Common.model.Book()
            {
                Isbn = book.Isbn,
                Author = book.Author,
                Genre = (Common.model.Genre?)book.Genre,
                Id = book.Id,
                Language = book.Language,
                Pages = book.Pages,
              //  Published = new Timestamp(),
                Title = book.Title
            };
             reply =  _bookStorage.AddBook(reply);

             book.Id = reply.Id;
            return  Task.FromResult(book);
        }
    }
}
