using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateRestaurantBranchesDropDownListAsync();
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePostAsync(OrderTable orderTable)
        {
            var client = _httpClientFactory.CreateClient();
            using (HttpResponseMessage response = await client.PostAsJsonAsync("http://localhost:54517/api/Reservation", orderTable))
            {
                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadAsAsync<OrderTable>();
                    return RedirectToAction("ThankYouAsync", routeValues: new { orderId = order.Id });
                }
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> ThankYouAsync(int orderId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:54517");
            using (HttpResponseMessage response = await httpClient.GetAsync($"api/Reservation/{orderId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var orderTable = await response.Content.ReadAsAsync<OrderTable>();
                    return View(orderTable);
                }
            }
            return View("Error");
        }

        private async Task PopulateRestaurantBranchesDropDownListAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:54517");
            using (HttpResponseMessage response = await httpClient.GetAsync("api/RestaurantBranches"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var restaurantBranches = await response.Content.ReadAsAsync<IEnumerable<RestaurantBranch>>();
                    ViewBag.RestaurantBranches = new SelectList(restaurantBranches, dataValueField: "Id", dataTextField: "City");
                }
            }
        }
    }
}