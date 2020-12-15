using ChatClient.Views;
using Prism.Ioc;
using System.Windows;
using ChatClient.model;
using ChatClient.Service;
using ChatClient.ViewModels;
using ChatCommon.model;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<Settings>(new Settings());
            containerRegistry.RegisterInstance<ChatRoom>(new ChatRoom());
            containerRegistry.Register<RoomService>();
            containerRegistry.RegisterDialog<Login, LoginViewModel>();
        }
    }
}
