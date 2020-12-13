using ChatClient.model;

namespace ChatClient.ViewModels
{
    public interface INotifySettings
    {
        public void SettingChanged(Settings settings);
    }
}