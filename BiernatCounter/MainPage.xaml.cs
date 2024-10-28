using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;

namespace BiernatCounter
{
    public partial class MainPage : ContentPage
    {
        private const string FileName = "counters.txt";
        private string FilePath => Path.Combine(FileSystem.Current.AppDataDirectory, FileName);

        private List<CounterModel> counters = new List<CounterModel>();

        public MainPage()
        {
            InitializeComponent();
            LoadCounters();
        }

        private void SaveCounters()
        {
            using (var writer = new StreamWriter(FilePath))
            {
                foreach (var counter in counters)
                {
                    writer.WriteLine($"{counter.Name}:{counter.InitialValue}");
                }
            }
        }

        private void LoadCounters()
        {
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int value))
                    {
                        var counter = new CounterModel { Name = parts[0], InitialValue = value };
                        counters.Add(counter);
                        AddC(counter.Name, counter.InitialValue);
                    }
                }
            }
        }

        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Nowy licznik", "Podaj nazwę licznika:");
            if (string.IsNullOrWhiteSpace(name))
                return;

            string initialValueText = await DisplayPromptAsync("Wartość początkowa", "Podaj początkową wartość:", initialValue: "0");
            int initialValue = int.TryParse(initialValueText, out int parsedValue) ? parsedValue : 0;

            counters.Add(new CounterModel { Name = name, InitialValue = initialValue });
            AddC(name, initialValue);
            SaveCounters();
        }

        private void AddC(string name, int initialValue)
        {
            int count = initialValue;

            var counterBorder = new Border
            {
                Stroke = Colors.Gray,
                StrokeThickness = 2,
                Padding = 20,
                BackgroundColor = Colors.LightGray,
                HorizontalOptions = LayoutOptions.Center
            };

            var counterLayout = new VerticalStackLayout
            {
                Spacing = 15,
                HorizontalOptions = LayoutOptions.Center
            };

            var titleLabel = new Label
            {
                Text = name,
                TextColor = Colors.Black,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 18
            };

            var counterLabel = new Label
            {
                Text = count.ToString(),
                TextColor = Colors.Black,
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center
            };

            var minusButton = new Button
            {
                Text = "-",
                BackgroundColor = Colors.Red,
                TextColor = Colors.White,
                WidthRequest = 80,
                HeightRequest = 40,
                FontSize = 20
            };
            minusButton.Clicked += (s, e) =>
            {
                count--;
                counterLabel.Text = count.ToString();

                var counter = counters.Find(c => c.Name == name);
                if (counter != null) counter.InitialValue = count;
                SaveCounters();
            };

            var plusButton = new Button
            {
                Text = "+",
                BackgroundColor = Colors.Green,
                TextColor = Colors.White,
                WidthRequest = 80,
                HeightRequest = 40,
                FontSize = 20
            };
            plusButton.Clicked += (s, e) =>
            {
                count++;
                counterLabel.Text = count.ToString();

                var counter = counters.Find(c => c.Name == name);
                if (counter != null) counter.InitialValue = count;
                SaveCounters();
            };

            var buttonLayout = new HorizontalStackLayout
            {
                Spacing = 20,
                HorizontalOptions = LayoutOptions.Center
            };
            buttonLayout.Add(minusButton);
            buttonLayout.Add(plusButton);

            counterLayout.Add(titleLabel);
            counterLayout.Add(counterLabel);
            counterLayout.Add(buttonLayout);

            counterBorder.Content = counterLayout;
            CounterBox.Add(counterBorder);
        }
    }

    public class CounterModel
    {
        public string Name { get; set; } = string.Empty;
        public int InitialValue { get; set; }
    }
}


/*Na 
 ocene dopuszczającą:
  -Aplikacja ma mieć na sztywno jeden licznik, który ma przycisk plus/minus i wartość +
  -Licznik ma zwiększać i zmniejszać swoją wartość po wciśnięciu odpowiedniego przycisku +

 Na ocene dostateczną:
  -Aplikacja ma mieć możliwość dynamicznego dodawania liczników +
  -Każdy licznik powinien być też opatrzony nazwą +

 Na ocene dobrą:
  -Aplikacja powinna wczytywać zapisane stany liczników na starcie aplikacji +
  -Aplikacja powinna posiadać system zapisu wartości liczników +

 Na ocenę bardzo dobrą:
  -Aplikacja powinna mieć system auto-zapisu po każdej zmianie wartości licznika +
  -Aplikacja powinna umożliwiać nadanie wartości początkowej licznikowi ( tylko przy twoarzeniu licznika, nie w trakcie jego użytkowania) +

 Na ocenę celującą:
  -Aplikacja powinna mieć możliwość personalizacji koloru licznika (też ma być ta informacja wczytywana)
  -Aplikacja powinna przechowywać dane w bardziej sensownej formie niż pliki txt np .xml lub ewentualnie .json, bazy danych lokalnej (SQLite), lub bazy zewnętrznej (np. FireBase) itp.
  -Aplikacja powinna mieć możliwość resetowania liczników do wartości początkowej podanej przy tworzeniu licznika
  -Aplikacja powinna mieć możliwość usunięcia licznika*/