using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using NeXt.Twitch.Chat.Wpf.Annotations;

namespace NeXt.Twitch.Chat.Wpf
{
    public abstract class NotifyObjectBase
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public event PropertyChangingEventHandler PropertyChanging;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (isNotifyEnabled)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (isNotifyEnabled)
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private bool isNotifyEnabled = true;

        protected void NotifyOff()
        {
            isNotifyEnabled = false;
        }

        protected void NotifyOn()
        {
            isNotifyEnabled = true;
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;

            OnPropertyChanging(propertyName);
            field = value;
            OnPropertyChanged(propertyName);
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        protected void Notify(string propertyName)
        {
            OnPropertyChanging(propertyName);
            OnPropertyChanged(propertyName);
        }
    }

    internal interface INotifyObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        void OnPropertyChanged([CallerMemberName] string propertyName = null);
        void OnPropertyChanging([CallerMemberName] string propertyName = null);
    }
}