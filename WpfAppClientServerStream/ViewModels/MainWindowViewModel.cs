using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using WpfAppClientServerStream.Services;

namespace WpfAppClientServerStream.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand LoadedCommand { get; private set; }
        private readonly ITempatureService _tempatureService;
        public MainWindowViewModel(ITempatureService tempatureService)
        {
            this._tempatureService = tempatureService;
            tempatureService.TempatureChange += TempatureService_TempatureChange;
            LoadedCommand = new DelegateCommand(()=>
            {
                tempatureService.StartProcess();
            });
          
        }

        private void TempatureService_TempatureChange(object sender, int e)
        {
            Tempature = e;
            

           
            //Thread.Sleep(TimeSpan.FromSeconds(1));
        }

    

        private int _tempature;

        public int Tempature
        {
            get => _tempature;
            set
            {
                _tempature = value;
                var cg = SetProperty<int>(ref _tempature, value);
                RaisePropertyChanged(nameof(Tempature));
                Console.WriteLine(cg);
            }
        }
    }
}