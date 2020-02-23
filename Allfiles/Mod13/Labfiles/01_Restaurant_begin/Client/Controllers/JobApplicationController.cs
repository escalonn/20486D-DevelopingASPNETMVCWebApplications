using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Client.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public JobApplicationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateEmployeeRequirementsDropDownListAsync();
            return View();
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        private async Task PopulateEmployeeRequirementsDropDownListAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:54517");
            using (HttpResponseMessage response = await httpClient.GetAsync("api/RestaurantWantedAd"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var employeeRequirements = await response.Content.ReadAsAsync<IEnumerable<EmployeeRequirements>>();
                    ViewBag.EmployeeRequirements = new SelectList(employeeRequirements, dataValueField: "Id", dataTextField: "JobTitle");
                }
            }
        }
    }
}