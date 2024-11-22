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
        //variable declaration for the Table client Objects that will be used to read/write data fron their respective tables
        private readonly TableClient claimsTableClient;

        Random rnd = new Random(); //Used to generate a random number later on

        //Constructor
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

            //sets the Claim status to either accepted or rejected based on a set of min and max criteria
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

                //the automatic total payment calculator
                claim.Final_Pay = claim.Hours_Worked * claim.Hourly_Rate;

                //generates a random Claim_ID
                int randNum = rnd.Next(3, 100);
                claim.Claim_ID = randNum;

                //Adds the claim to the Claims table
                await claimsTableClient.AddEntityAsync(claim);

                return RedirectToAction("~Views/Home/ClaimSubmissionPage.cshtml", new { lecturerId = claim.Lecturer_ID });
            }

            return View("~Views / Home / ClaimSubmissionPage.cshtml");
        }
    }
}