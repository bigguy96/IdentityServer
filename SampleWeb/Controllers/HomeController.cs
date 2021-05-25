using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWeb.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PrivacyAsync()
        {
            var client = new HttpClient();
            var metaDataResponse = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metaDataResponse.UserInfoEndpoint,
                Token = accessToken
            });

            if (response.IsError)
            {
                throw new Exception("Problem while fetching data from the UserInfo endpoint", response.Exception);
            }

            var addressClaim = response.Claims.FirstOrDefault(c => c.Type.Equals("address"));
            if (addressClaim != null)
            {
                User.AddIdentity(new ClaimsIdentity(new List<Claim> { new(addressClaim.Type, addressClaim.Value) }));
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return RedirectToAction("Index");
        }
    }
}