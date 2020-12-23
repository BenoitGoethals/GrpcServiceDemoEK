using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GrpcServiceBook.storage
{
    public class BookStorage: IBookStorage
    {
        private List<Common.model.Book> _books;

        public BookStorage()
        {
            Load();
        }
        private void Load()
        {

            var myJsonString = File.ReadAllText(@"dataBooks.json");
            _books = JsonConvert.DeserializeObject<List<Common.model.Book>>(myJsonString);

        }

        public Common.model.Book GetBook(string isbn)
        {
            return _books.Find(b => b.Isbn.Equals(isbn));
        }

        public List<Common.model.Book> Books()
        {
            return _books;
        }
    }
}
