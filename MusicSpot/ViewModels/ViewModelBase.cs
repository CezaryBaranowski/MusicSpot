using System.ComponentModel;
using System.Runtime.CompilerServices;
using MusicSpot.Annotations;

namespace MusicSpot
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            //if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
