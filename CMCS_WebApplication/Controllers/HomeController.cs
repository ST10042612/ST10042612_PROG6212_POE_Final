using Azure.Data.Tables;
using CMCS_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMCS_WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly TableClient claimsTableClient;
        private readonly TableClient lecturersTableClient;

        public HomeController(IConfiguration configuration)
        {
            // Get connection string from appsettings.json
            var connectionStr = configuration.GetConnectionString("AzureTableStorage");

            // Initialize the TableClient for Claims and Lecturers
            var serviceClient = new TableServiceClient(connectionStr);
            claimsTableClient = serviceClient.GetTableClient("Claims");
            lecturersTableClient = serviceClient.GetTableClient("Lecturers");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lecturer()
        {
            return View("~/Views/Home/ClaimSubmissionPage.cshtml");
        }
        public IActionResult Manager()
        {
            return View("~/Views/Home/ClaimListPage.cshtml");
        }

    }
}
