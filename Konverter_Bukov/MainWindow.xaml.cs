using CurrencyConverterApp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Konverter_Bukov
{
    public partial class MainWindow : Window
    {
        private CurrencyConverter _currencyConverter;

        public MainWindow()
        {
            InitializeComponent();
            _currencyConverter = new CurrencyConverter();
            LoadCurrencies();
        }

        // Метод для загрузки списка валют в ComboBox
        private async void LoadCurrencies()
        {
            await _currencyConverter.LoadCurrencyRates();
            FromCurrency.ItemsSource = _currencyConverter.CurrencyRates.Keys;
            ToCurrency.ItemsSource = _currencyConverter.CurrencyRates.Keys;
            CurrencyRatesGrid.ItemsSource = ConvertDictionaryToList(_currencyConverter.CurrencyRates);
        }

        // Обработчик события для кнопки "Обновить курсы"
        private async void UpdateRatesButton_Click(object sender, RoutedEventArgs e)
        {
            await _currencyConverter.LoadCurrencyRates();
            CurrencyRatesGrid.ItemsSource = ConvertDictionaryToList(_currencyConverter.CurrencyRates);
            MessageBox.Show("Курсы валют успешно обновлены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Обработчик события для кнопки "Конвертировать"
        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fromCurrency = FromCurrency.SelectedItem.ToString();
                string toCurrency = ToCurrency.SelectedItem.ToString();
                double amount = Convert.ToDouble(AmountTextBox.Text);
                double result = _currencyConverter.Convert(fromCurrency, toCurrency, amount);
                ResultTextBlock.Text = $"{amount} {fromCurrency} = {result:F2} {toCurrency}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка конвертации: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для преобразования словаря курсов в список для отображения в DataGrid
        private List<CurrencyRate> ConvertDictionaryToList(Dictionary<string, double> currencyRates)
        {
            var list = new List<CurrencyRate>();
            foreach (var rate in currencyRates)
            {
                list.Add(new CurrencyRate { Currency = rate.Key, Rate = rate.Value });
            }
            return list;
        }
    }

    // Вспомогательный класс для отображения курсов валют в DataGrid
    public class CurrencyRate
    {
        public string Currency { get; set; }
        public double Rate { get; set; }
    }
}
