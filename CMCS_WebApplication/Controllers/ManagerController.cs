using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using CMCS_WebApplication.Models;
using System.Linq;
using System.Threading.Tasks;

//Help with the creation and set up of the controllers: (Anderson, 2024) and (TutorialsPoint, n.d.)
namespace CMCS_WebApplication.Controllers
{
    public class ManagerController : Controller
    {
        //variable declaration for TableClient objects to be used later for reading and writng to the claims and lecturers tables (Azure-SDK bot and Zhu, 2024)
        private readonly TableClient claimsTableClient;
        private readonly TableClient lecturersTableClient;

        //Constructor (Azure-SDK bot and Zhu, 2024)
        public ManagerController(IConfiguration configuration)
        {
            // Gets the connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("AzureTableStorage");

            // Initializes the TableClient for the Claims and Lecturers tables
            var serviceClient = new TableServiceClient(connectionString);
            claimsTableClient = serviceClient.GetTableClient("Claims");
            lecturersTableClient = serviceClient.GetTableClient("Lecturers");
        }

        public IActionResult Index()
        {
            return View("~Views/Home/Index.cshtml");
        }
        
        //Fetches all the Claims from the Claims table in order for them to be shown later on
        public async Task<IActionResult> ViewAllClaims()
        {
            var claims = new List<Claim>();
            await foreach (var claim in claimsTableClient.GetEntityAsync<Claim>())
            {
                claims.Add(claim);
            }

            return View(claims);
        }

        //Fetches all claims with the status of approved in order to show them
        public async Task<IActionResult> ViewApprovedClaims()
        {
            var claims = new List<Claim>();
            await foreach (var claim in claimsTableClient.GetEntityAsync<Claim>())
            {
                if (claim.Claim_Status == "Accepted")
                {
                    claim.FinalPay = claim.Hours_Worked * claim.Hourly_Rate;
                    claims.Add(claim);
                }
            }

            return View(claims);
        }

        //Fetches all the Lecturers from the Lecturers Table in order for the entries to be viewed/edited later on
        public async Task<IActionResult> EditLecturers()
        {
            var lecturers = new List<Lecturer>();
            await foreach (var lecturer in lecturersTableClient.GetEntityAsync<Lecturer>())
            {
                lecturers.Add(lecturer);
            }

            return View(lecturers);
        }
        
    }
}

