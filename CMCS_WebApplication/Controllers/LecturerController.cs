using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using CMCS_WebApplication.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CMCS_WebApplication.Controllers
{
    public class LecturerController : Controller
    {
        private readonly TableClient claimsTableClient;

        public LecturerController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureTableStorage");
            var serviceClient = new TableServiceClient(connectionString);
            claimsTableClient = serviceClient.GetTableClient("Claims");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewClaims(int lecturerId)
        {
            var claims = new List<Claim>();
            await foreach (var claim in claimsTableClient.GetEntityAsync<Claim>())
            {
                if (claim.Lecturer_ID == lecturerId)
                {
                    claims.Add(claim);
                } 
            }
            return View(claims);
        }

        public IActionResult SubmitClaim()
        {
            return View("~Views/Home/ClaimSubmissionPage.cshtml");
        }

        public async Task<IActionResult> SubmitClaim(Claim claim)
        {
            if (ModelState.IsValid)
            {

                if (claim.Hours_Worked >= 5 && claim.Hours_Worked <= 60 && claim.Hourly_Rate >= 23 && claim.Hourly_Rate <= 120)
                {
                    claim.Claim_Status = "Accepted";
                }
                else
                {
                    claim.Claim_Status = "Rejected";
                }

                claim.Final_Pay = claim.Hours_Worked * claim.Hourly_Rate;

                claim.RowKey = Guid.NewGuid().ToString();

                await claimsTableClient.AddEntityAsync(claim);

                return RedirectToAction("~Views/Home/ClaimSubmissionPage.cshtml", new { lecturerId = claim.Lecturer_ID });
            }

            return View("~Views / Home / ClaimSubmissionPage.cshtml");
        }
    }
}