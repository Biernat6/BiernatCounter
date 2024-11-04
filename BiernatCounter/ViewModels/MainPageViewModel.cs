using BiernatCounter.Models;
using BiernatCounter.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BiernatCounter.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private readonly CounterService _counterService;
        public ObservableCollection<CounterModel> Counters { get; set; }

        public ICommand AddCounterCommand { get; }
        public ICommand IncrementCounterCommand { get; }
        public ICommand DecrementCounterCommand { get; }

        public MainPageViewModel()
        {
            _counterService = new CounterService();
            Counters = new ObservableCollection<CounterModel>(_counterService.LoadCounters());
            AddCounterCommand = new Command(OnAddCounter);
            IncrementCounterCommand = new Command<CounterModel>(OnIncrementCounter);
            DecrementCounterCommand = new Command<CounterModel>(OnDecrementCounter);
        }

        private async void OnAddCounter()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Nowy licznik", "Podaj nazwę licznika:");
            if (string.IsNullOrWhiteSpace(name) || Counters.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", "Licznik o tej nazwie już istnieje!", "OK");
                return;
            }

            string initialValueText = await Application.Current.MainPage.DisplayPromptAsync("Wartość początkowa", "Podaj początkową wartość:", initialValue: "0");
            if (!int.TryParse(initialValueText, out int initialValue))
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", "Wartość początkowa musi być liczbą!", "OK");
                return;
            }

            var newCounter = new CounterModel { Name = name, InitialValue = initialValue };
            Counters.Add(newCounter);
            _counterService.SaveCounters(Counters.ToList());
        }

        private void OnIncrementCounter(CounterModel counter)
        {
            if (counter == null) return;
            counter.InitialValue++;
            _counterService.SaveCounters(Counters.ToList());
            OnPropertyChanged(null);
        }

        private void OnDecrementCounter(CounterModel counter)
        {
            if (counter == null) return;
            counter.InitialValue--;
            _counterService.SaveCounters(Counters.ToList());
            OnPropertyChanged(null);
        }
    }
}
