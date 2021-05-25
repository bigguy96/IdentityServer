using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWeb.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SampleWeb.Services;

namespace SampleWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISampleClient _sampleClient;

        public HomeController(ISampleClient sampleClient, ILogger<HomeController> logger)
        {
            _sampleClient = sampleClient;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var weatherForecastViewModel = await _sampleClient.GetForecastAsync();

            _logger.LogInformation("Returning weather forecasts");

            return View(weatherForecastViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}