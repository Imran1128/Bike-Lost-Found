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

namespace BikeLostAndFound.Controllers
{

    public class HomeController : Controller

    {
        private readonly IBikeLostAndFoundRepository blfRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(IBikeLostAndFoundRepository blfRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.blfRepository = blfRepository;
            this.webHostEnvironment = webHostEnvironment;
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
        [HttpGet]
        public async Task<IActionResult> LostBikes()
        {

            try
            {
                var result = await blfRepository.GetAll();
                return View(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<IActionResult> FoundBikes()
        {

            var result = await blfRepository.GetAll();
            return View(result);
        }
        public IActionResult FullDetails(string BikeRegNo)
        {
            var Bike = blfRepository.GetByReg(BikeRegNo);
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
