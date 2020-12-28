using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcServiceBook;
using Book = Common.model.Book;
using Genre = Common.model.Genre;

namespace WpfAppGrpcBook.Services
{
    public class BookService:IBookService
    {
        /// <summary>
        /// Get all Books
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Book>> GetAll()
        {
            IList<Book> books = new List<Book>();
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
             var client = new LibBook.LibBookClient(channel);
            var rest=await client.BooksAsync(new Empty());
            rest.Book.ToList().ForEach(b=>books.Add(new Book(){Title=b.Title,Author=b.Author,Genre=(Genre) b.Genre,Language=b.Language, Isbn = b.Isbn,Pages=b.Pages,Id=b.Id,Published=b.Published.ToDateTime()}));
            
            return books;
        }

        public async Task<Book> GetBookAsync(string isbn)
        {
            
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new LibBook.LibBookClient(channel);
            var b = await client.GetBookAsync(new RequestIsbn(){Isbn =isbn});
            Common.model.Book bookDto = new Common.model.Book()
            {
                Title = b.Title, Author = b.Author, Genre = (Genre) b.Genre, Language = b.Language, Isbn = b.Isbn,
                Pages = b.Pages, Id = b.Id, Published = b.Published.ToDateTime()
            };

            return bookDto;

        }
    }
}