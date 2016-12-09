using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NeXt.Twitch.Chat.Wpf
{
    internal sealed class NotifyObjectHelper
    {
        private readonly INotifyObject instance;

        public NotifyObjectHelper(INotifyObject owner)
        {
            instance = owner;
        }

        private bool isNotifyEnabled = true;

        public void Off()
        {
            isNotifyEnabled = false;
        }

        public void On()
        {
            isNotifyEnabled = true;
        }


        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;

            if (isNotifyEnabled)
            {
                instance.OnPropertyChanging(propertyName);
            }

            field = value;

            if (isNotifyEnabled)
            {
                instance.OnPropertyChanged(propertyName);
            }
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public void Exec(string propertyName)
        {
            if (isNotifyEnabled)
            {
                instance.OnPropertyChanging(propertyName);
                instance.OnPropertyChanged(propertyName);
            }
        }
    }
}