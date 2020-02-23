using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class RestaurantBranchesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RestaurantBranchesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:54517");
            using (HttpResponseMessage response = await httpClient.GetAsync("api/RestaurantBranches"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var restaurantBranches = await response.Content.ReadAsAsync<IEnumerable<RestaurantBranch>>();
                    return View(restaurantBranches);
                }
            }
            return View("Error");
        }
    }
}