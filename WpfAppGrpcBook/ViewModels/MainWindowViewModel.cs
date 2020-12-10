using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Common.model;
using Prism.Commands;
using Prism.Mvvm;
using WpfAppGrpcBook.Services;

namespace WpfAppGrpcBook.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        public ObservableCollection<Book> Books { get; private set; }=new ObservableCollection<Book>();

         private readonly IBookService _booksService;
        public MainWindowViewModel(IBookService booksService)
        {
            _booksService = booksService;
        }

        private DelegateCommand _commandLoad = null;
     

        public DelegateCommand CommandLoad =>
            _commandLoad ??= new DelegateCommand(CommandLoadExecute);

        private async void CommandLoadExecute()
        {
            //   Books.Clear();
            foreach (var book in await _booksService.GetAll())
            {
                Books.Add(book);
            }
        }
    }
}