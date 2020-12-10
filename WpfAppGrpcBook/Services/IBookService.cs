using System.Collections.Generic;
using System.Threading.Tasks;
using Common.model;

namespace WpfAppGrpcBook.Services
{
    public interface IBookService
    {
        Task<IList<Book>> GetAll();
    }
}