using System.ComponentModel;

namespace BiernatCounter.Models
{
    public class CounterModel : INotifyPropertyChanged
    {
        private int _initialValue;

        public string Name { get; set; } = string.Empty;

        public int InitialValue
        {
            get => _initialValue;
            set
            {
                if (_initialValue != value)
                {
                    _initialValue = value;
                    OnPropertyChanged(nameof(InitialValue));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
