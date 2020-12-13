using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ChatClient.model;
using Prism.Services.Dialogs;

namespace ChatClient.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {

        private Settings _settings;
        public LoginViewModel(Settings settings)
        {
            this._settings = settings;
        }


        public string LoginName
        {
            get => _loginName;
            set { _loginName = value; SetProperty(ref _loginName, value); }
        }



        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ??= new DelegateCommand<string>(CloseDialog);

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title = "Login";
        private string _loginName;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public event Action<IDialogResult> RequestClose;

        protected virtual void CloseDialog(string parameter)
        {
            _settings.ChatterLocal=new ChatCommon.model.Chatter(){Guid = new Guid(),Enabled=true,Name=LoginName};

            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }
    }
}
