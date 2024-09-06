using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json; // Не забудьте добавить Newtonsoft.Json

namespace CurrencyConverterApp
{
    public class CurrencyConverter
    {      
        public Dictionary<string, double> CurrencyRates { get; private set; } = new Dictionary<string, double>();
                 
        public async Task LoadCurrencyRates()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "https://open.er-api.com/v6/latest/RUB"; 
                    var response = await client.GetStringAsync(url);
                    var data = JsonConvert.DeserializeObject<CurrencyData>(response);
                  
                    CurrencyRates = data.Rates;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки курсов: " + ex.Message);
                }
            }
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            if (CurrencyRates.ContainsKey(fromCurrency) && CurrencyRates.ContainsKey(toCurrency))
            {
                double fromRate = CurrencyRates[fromCurrency];
                double toRate = CurrencyRates[toCurrency];
                return amount * toRate / fromRate;
            }
            else
            {
                throw new Exception("Неизвестная валюта");
            }
        }
    }

    public class CurrencyData
    {
        public string BaseCode { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}
