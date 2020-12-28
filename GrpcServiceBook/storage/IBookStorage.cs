using System.Collections.Generic;

namespace GrpcServiceBook.storage
{
    public interface IBookStorage
    {
        Common.model.Book GetBook(string isbn);
        List<Common.model.Book> Books();
        Common.model.Book AddBook(Common.model.Book book);
    }
}