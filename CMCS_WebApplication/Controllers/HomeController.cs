using Azure.Data.Tables;
using CMCS_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

//Help with the creation and set up of the controllers: (Anderson, 2024) and (TutorialsPoint, n.d.)
namespace CMCS_WebApplication.Controllers
{
    public class HomeController : Controller
    {
        //variable declaration for the Table client Objects that will be used to read/write data fron their respective tables (Azure-SDK bot and Zhu, 2024)
        private readonly TableClient claimsTableClient;
        private readonly TableClient lecturersTableClient;

        //Constructor (Azure-SDK bot and Zhu, 2024)
        public HomeController(IConfiguration configuration)
        {
            // Gets the connection string from appsettings.json
            var connectionStr = configuration.GetConnectionString("AzureTableStorage");

            // Initializes the TableClient for the Claims and Lecturers tables
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
