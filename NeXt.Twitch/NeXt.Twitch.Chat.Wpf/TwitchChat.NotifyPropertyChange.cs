using System.ComponentModel;
using System.Runtime.CompilerServices;
using NeXt.Twitch.Chat.Wpf.Annotations;

namespace NeXt.Twitch.Chat.Wpf
{
    public partial class TwitchChat
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public event PropertyChangingEventHandler PropertyChanging;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        void INotifyObject.OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        void INotifyObject.OnPropertyChanging(string propertyName)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanging(propertyName);
        }
    }
}