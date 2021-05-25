using System.Collections.Generic;
using System.Threading.Tasks;
using SampleWeb.Models;

namespace SampleWeb.Services
{
    public interface ISampleClient
    {
        Task<IEnumerable<WeatherForecastViewModel>> GetForecastAsync();
    }
}