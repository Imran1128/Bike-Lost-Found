using BikeLostAndFound.Interfaces;
using BikeLostAndFound.Models;
using BikeLostAndFound.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BikeLostAndFound.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace BikeLostAndFound.Controllers
{

    public class HomeController : Controller

    {
        private readonly IBikeLostAndFoundRepository blfRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IDataProtector Protector;

        public HomeController(IBikeLostAndFoundRepository blfRepository, IWebHostEnvironment webHostEnvironment,IDataProtectionProvider dataProtectionProvider,DataProtectionPurposeString dataProtectionPurposeString)
        {
            this.blfRepository = blfRepository;
            this.webHostEnvironment = webHostEnvironment;
            Protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeString.BikeRouteValue);
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var addbikeViewModel = new AddBikeViewModel
            {
                UserID = User.FindFirst("Id").Value,
            };

            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddBikeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UniqueFileName = null;
                if (model.BikePhoto != null)

                {
                    string UploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    UniqueFileName = Guid.NewGuid().ToString() + "_" + model.BikePhoto.FileName;
                    string FilePath = Path.Combine(UploadFolder, UniqueFileName);
                    model.BikePhoto.CopyTo(new FileStream(FilePath, FileMode.Create));
                }
                var LostAndFoundBikeInfo = new LostAndFoundBikeInformation
                {
                    UserID = model.UserID,
                    BikePhoto = UniqueFileName,
                    BikeName = model.BikeName,
                    BikeRegNo = model.BikeRegNo,
                    BikeSN = model.BikeSN,
                    OwnerAddress = model.OwnerAddress,
                    OwnerEmail = model.OwnerEmail,
                    OwnerPhone = model.OwnerPhone,
                    OwnerName = model.OwnerName,
                    PlaceWhereLost = model.PlaceWhereLost,
                    Date = model.Date,
                    GDNumber = model.GDNumber,
                };
                return (await blfRepository.InsertByAsync(LostAndFoundBikeInfo)) ? RedirectToAction("MyList","Profile") : RedirectToAction("Create");

            }
            return View();
        }
        public async Task<IActionResult> LostBikes()
        {

            var bikes = await blfRepository.GetAll();
                var result = bikes.Select(e =>
                {
                    e.EncryptedId = Protector.Protect(e.Id.ToString());
                    return e;
                });

            return View(result);
            
        }


        public async Task<IActionResult> FoundBikes()
        {

            var result = await blfRepository.GetAll();
            return View(result);
        }
        public async Task<IActionResult> FullDetails(string BikeId)
        {
            int deCryptedId = Convert.ToInt32(Protector.Unprotect(BikeId));
            var Bike =await blfRepository.FindByIdAsync(deCryptedId);
            if (Bike == null)
            {
                ViewBag.ErrorTitle = "Bike not Found";
                return View("~/Views/Error/Error.cshtml");
            }

            FullDetailsViewModel fullDetailsViewModel = new FullDetailsViewModel()
            {
                lostAndFoundBikeInformation = Bike,
            };
            
            return View(fullDetailsViewModel);
        }

        public async Task<IActionResult> DeleteBike(string BikeRegNo)
        {
            var Bike = blfRepository.GetByReg(BikeRegNo);
            await blfRepository.DeleteByAsync(Bike);
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("LostBikes", "Home");
            }
            return RedirectToAction("MyList", "Profile");
        }




    }
}
