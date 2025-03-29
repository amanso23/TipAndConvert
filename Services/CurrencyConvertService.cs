using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TipAndConvert.Models;

namespace TipAndConvert.Services
{
    internal class CurrencyConvertService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public CurrencyConvertService()
        {
            _httpClient = new HttpClient();
            _apiKey = ConfigurationManager.AppSettings["API_KEY"]!;
            _baseUrl = ConfigurationManager.AppSettings["BASE_URL"]!;
        }

        public async Task<float> ConvertCurrencyAsync(string from, string to, float amount)
        {
            string url = $"{_baseUrl}/{_apiKey}/latest/{from}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ExchangeRateResult>(stream);

                if(result?.conversion_rates != null && result.conversion_rates.TryGetValue(to, out float rate)){
                    return rate * amount;
                }

                throw new Exception("Moneda de destino no encrontrada en la respuesta.");
                
            }

            throw new Exception("Error al consultar la API de conversión");
        }

    }
}
