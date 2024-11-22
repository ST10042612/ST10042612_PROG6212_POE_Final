using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using CMCS_WebApplication.Models;
using System.Linq;
using System.Threading.Tasks;


namespace CMCS_WebApplication.Controllers
{
    public class ManagerController : Controller
    {
        private readonly TableClient claimsTableClient;
        private readonly TableClient lecturersTableClient;


        public ManagerController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureTableStorage");
            var serviceClient = new TableServiceClient(connectionString);
            claimsTableClient = serviceClient.GetTableClient("Claims");
            lecturersTableClient = serviceClient.GetTableClient("Lecturers");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAllClaims()
        {
            var claims = new List<Claim>();
            await foreach (var claim in claimsTableClient.GetEntityAsync<Claim>())
            {
                claims.Add(claim);
            }

            return View(claims);
        }

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

