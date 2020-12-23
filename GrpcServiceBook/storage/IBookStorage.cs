﻿using System.Collections.Generic;

namespace GrpcServiceBook.storage
{
    public interface IBookStorage
    {
        Common.model.Book GetBook(string isbn);
        List<Common.model.Book> Books();
    }
}