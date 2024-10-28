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
        private readonly IBikeAdvertisement _bikeAdvertisement;
        private readonly IDataProtector Protector;

        public HomeController(IBikeLostAndFoundRepository blfRepository, IWebHostEnvironment webHostEnvironment,IDataProtectionProvider dataProtectionProvider,DataProtectionPurposeString dataProtectionPurposeString, IBikeAdvertisement bikeAdvertisement)
        {
            this.blfRepository = blfRepository;
            _bikeAdvertisement = bikeAdvertisement;
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
            var Bike = await blfRepository.GetSingleData(c=>c.BikeRegNo.Equals(BikeRegNo));
            await blfRepository.DeleteByAsync(Bike);
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("LostBikes", "Home");
            }
            return RedirectToAction("MyList", "Profile");
        }
        
        [HttpGet]
        public IActionResult AdCreate()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> AdCreate(CreateAdViewModel modell)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .Select(m => new { Field = m.Key, Error = m.Value.Errors.First().ErrorMessage })
                    .ToList();

                foreach (var error in errors)
                {
                    Console.WriteLine($"Field: {error.Field}, Error: {error.Error}");
                }

                return View(modell); // Return the view to show validation errors
            }
            if (ModelState.IsValid)
{
    string UniqueFileName = null;
    if (modell.BikePhoto != null)

    {
        string UploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
        UniqueFileName = Guid.NewGuid().ToString() + "_" + modell.BikePhoto.FileName;
        string FilePath = Path.Combine(UploadFolder, UniqueFileName);
                    modell.BikePhoto.CopyTo(new FileStream(FilePath, FileMode.Create));
    }
    var bikeAdvertisement = new BikeAdvertisement
    {
        UserID = modell.UserID,
        BikePhoto = UniqueFileName,
        Brand = modell.Brand,
        Model = modell.Model,
        KilometersRun = modell.KilometersRun,
        OwnerAddress = modell.OwnerAddress,
        OwnerEmail = modell.OwnerEmail,
        OwnerPhone = modell.OwnerPhone,
        OwnerName = modell.OwnerName,
        EngineCapacity = modell.EngineCapacity,
        YearOfManufacture = modell.YearOfManufacture,
        BikeRegNo = modell.BikeRegNo,

    };
    return (await _bikeAdvertisement.InsertByAsync(bikeAdvertisement)) ? RedirectToAction("MyAdList", "Home") : RedirectToAction("MyAdList", "Home");

}
            return View();
        }
        public async Task<IActionResult> BikeAds()
        {

            var bikes = await _bikeAdvertisement.GetAll();
            

            return View(bikes);

        }

        public async Task<IActionResult> FullAdDetails(int BikeId)
        {
            var Bike = await _bikeAdvertisement.FindByIdAsync(BikeId);
            if (BikeId == null)
            {
                ViewBag.ErrorTitle = "Bike not Found";
                return View("~/Views/Error/Error.cshtml");
            }

            FullAdViewModel fullAdViewModel = new FullAdViewModel()
            {
                bikeAdvertisement = Bike,
            };

            return View(fullAdViewModel);
        }

        public async Task<IActionResult> DeleteBikeAd(string BikeRegNo)
        {
            var Bike = await _bikeAdvertisement.GetSingleData(c => c.BikeRegNo.Equals(BikeRegNo));
            await _bikeAdvertisement.DeleteByAsync(Bike);
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("BikeAds", "Home");
            }
            return RedirectToAction("MyAdList", "Home");
        }
        public async Task<IActionResult> MyAdList()
        {
            var bike = await _bikeAdvertisement.GetAll();


            return View(bike);

        }
        public async Task<IActionResult> EditAdList(int BikeId)
        {
            
            BikeAdvertisement Bike = await _bikeAdvertisement.FindByIdAsync(BikeId);
            var CurrentUserId = User.FindFirst("Id").Value;


            if (Bike.UserID == CurrentUserId)
            {


                var modell = new UpdateAdViewModel

                {
                    Id = Bike.Id,
                    UserID = Bike.UserID,
                    Brand = Bike.Brand,
                    Model = Bike.Model,
                    KilometersRun = Bike.KilometersRun,
                    OwnerAddress = Bike.OwnerAddress,
                    OwnerEmail = Bike.OwnerEmail,
                    OwnerPhone = Bike.OwnerPhone,
                    OwnerName = Bike.OwnerName,
                    EngineCapacity = Bike.EngineCapacity,
                    YearOfManufacture = Bike.YearOfManufacture,
                    BikeRegNo = Bike.BikeRegNo,
                    ExistingPhoto = Bike.BikePhoto,
                    
                };
                return View(modell);
            }
            else
            {
                ViewBag.ErrorTitle = "Wrong User";
                return View("~/Views/Error/Error.cshtml");

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditAdList(UpdateAdViewModel modell)
        {
            if (ModelState.IsValid)
            {

                BikeAdvertisement bikeAdvertisement = await _bikeAdvertisement.FindByIdAsync(modell.Id);

                var CurrentUserId = User.FindFirst("Id").Value;


                if (bikeAdvertisement.UserID == CurrentUserId)
                {
                    bikeAdvertisement.Brand = modell.Brand;
                    bikeAdvertisement.Model = modell.Model;
                    bikeAdvertisement.BikeRegNo = modell.BikeRegNo;
                    bikeAdvertisement.OwnerName = modell.OwnerName;
                    bikeAdvertisement.OwnerAddress = modell.OwnerAddress;
                    bikeAdvertisement.OwnerPhone = modell.OwnerPhone;
                    bikeAdvertisement.OwnerEmail = modell.OwnerEmail;
                    bikeAdvertisement.KilometersRun = modell.KilometersRun;
                    bikeAdvertisement.EngineCapacity = modell.EngineCapacity;
                    bikeAdvertisement.YearOfManufacture = modell.YearOfManufacture;
                    if (modell.BikePhoto != null)
                    {
                        bikeAdvertisement.BikePhoto = FileUploadProcess(modell);
                        if (modell.ExistingPhoto != null)
                        {
                            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images", modell.ExistingPhoto);
                            System.IO.File.Delete(filePath);
                        }


                    }

                    await _bikeAdvertisement.UpdateByAsync(bikeAdvertisement);

                    return RedirectToAction("MyAdList", "Home");
                }
                else
                {
                    ViewBag.ErrorTitle = "Wrong User";
                    return View("~/Views/Error/Error.cshtml");

                }
            }

            return View(modell);
        }

        private string FileUploadProcess(UpdateAdViewModel model)
        {
            string UniqueFileName = null;
            if (ModelState.IsValid)
            {

                {
                    string fileUploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    UniqueFileName = Guid.NewGuid().ToString() + "_" + model.BikePhoto.FileName;
                    string filepath = Path.Combine(fileUploadFolder, UniqueFileName);
                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        model.BikePhoto.CopyTo(filestream);
                    }
                }
            }

            return UniqueFileName;
        }


    }
}
