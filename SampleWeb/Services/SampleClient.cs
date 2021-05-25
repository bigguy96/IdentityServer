using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SampleWeb.Models;

namespace SampleWeb.Services
{
    public class SampleClient : ISampleClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public SampleClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("sample");
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<IEnumerable<WeatherForecastViewModel>> GetForecastAsync()
        {
            var response = await _httpClient.GetAsync("api/WeatherForecast").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Problem with fetching data from the API: {response.ReasonPhrase}");

            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecastViewModel>>(stream);
        }
    }
}