using BikeLostAndFound.Interfaces;
using BikeLostAndFound.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using BikeLostAndFound.Data;
using BikeLostAndFound.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using BikeLostAndFound.Migrations;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

namespace BikeLostAndFound.Controllers
{
    public class ProfileController: Controller
    {
        private readonly IBikeLostAndFoundRepository blfRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDataProtector Protector;

        public ProfileController(IBikeLostAndFoundRepository blfRepository,IWebHostEnvironment webHostEnvironment,IDataProtectionProvider dataProtectionProvider,DataProtectionPurposeString dataProtectionPurposeString, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.blfRepository = blfRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.signInManager = signInManager;
            Protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeString.BikeRouteValue);

        }
        public IActionResult profile()
        {
            var encryptedUserId = User.FindFirst("EncryptedId")?.Value;
            string userId = User.FindFirst("Id").Value;
            encryptedUserId = Protector.Protect(userId);
            ViewBag.Id = encryptedUserId;
            return View();
        }
        public async Task<IActionResult> MyList()
        {
            var bike = await blfRepository.GetAll();
            var result = bike.Select(e =>
            {
                e.EncryptedId = Protector.Protect(e.Id.ToString());
                return e;
            });
            
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> EditList(string BikeId )
        {
            var decryptedBikeId = Convert.ToInt32(Protector.Unprotect(BikeId));
            LostAndFoundBikeInformation Bike = await blfRepository.FindByIdAsync(decryptedBikeId);
            var CurrentUserId = User.FindFirst("Id").Value;

            
            if (Bike.UserID == CurrentUserId )
            {
                

               var model = new UpdateListViewModel

                {
                    Id = Bike.Id,
                    BikeName = Bike.BikeName,
                    BikeSN = Bike.BikeSN,
                    BikeRegNo = Bike.BikeRegNo,
                    ExistingPhoto = Bike.BikePhoto,
                    OwnerName = Bike.OwnerName,
                    OwnerAddress = Bike.OwnerAddress,
                    OwnerPhone = Bike.OwnerPhone,
                    OwnerEmail = Bike.OwnerEmail,
                    Date = Bike.Date,
                    GDNumber = Bike.GDNumber,
                    PlaceWhereLost = Bike.PlaceWhereLost,
                    IsFound = Bike.IsFound,
                };
                return View(model);
            }
            else
            {
                ViewBag.ErrorTitle = "Wrong User";
                return View("~/Views/Error/Error.cshtml");

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditList(UpdateListViewModel model)
        {
            if (ModelState.IsValid)
            {

                LostAndFoundBikeInformation lostAndFoundBike =await blfRepository.FindByIdAsync(model.Id);
                
                var CurrentUserId = User.FindFirst("Id").Value;


                if (lostAndFoundBike.UserID == CurrentUserId)
                {
                    lostAndFoundBike.BikeName = model.BikeName;
                    lostAndFoundBike.BikeSN = model.BikeSN;
                    lostAndFoundBike.BikeRegNo = model.BikeRegNo;
                    lostAndFoundBike.OwnerName = model.OwnerName;
                    lostAndFoundBike.OwnerAddress = model.OwnerAddress;
                    lostAndFoundBike.OwnerPhone = model.OwnerPhone;
                    lostAndFoundBike.OwnerEmail = model.OwnerEmail;
                    lostAndFoundBike.GDNumber = model.GDNumber;
                    lostAndFoundBike.PlaceWhereLost = model.PlaceWhereLost;
                    lostAndFoundBike.IsFound = model.IsFound;
                    if (model.BikePhoto != null)
                    {
                        lostAndFoundBike.BikePhoto = FileUploadProcess(model);
                        if (model.ExistingPhoto != null)
                        {
                            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images", model.ExistingPhoto);
                            System.IO.File.Delete(filePath);
                        }


                    }

                    await blfRepository.UpdateByAsync(lostAndFoundBike);

                    return RedirectToAction("MyList", "Profile");
                }
                else
                {
                    ViewBag.ErrorTitle = "Wrong User";
                    return View("~/Views/Error/Error.cshtml");

                }
            }

            return View(model);
        }

        private string FileUploadProcess(UpdateListViewModel model)
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
