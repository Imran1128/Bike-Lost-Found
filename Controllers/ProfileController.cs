using BikeLostAndFound.Interfaces;
using BikeLostAndFound.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using BikeLostAndFound.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using BikeLostAndFound.Migrations;

namespace BikeLostAndFound.Controllers
{
    public class ProfileController: Controller
    {
        private readonly IBikeLostAndFoundRepository blfRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProfileController(IBikeLostAndFoundRepository blfRepository,IWebHostEnvironment webHostEnvironment)
        {
            this.blfRepository = blfRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult profile()
        {
            return View();
        }
        public async Task<IActionResult> MyList()
        {
            var result = await blfRepository.GetAll();
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> EditList(string BikeRegNo/*,LostAndFoundBikeInformation lostAndFoundBike*/ )
        {
            
            LostAndFoundBikeInformation Bike = blfRepository.GetByReg(BikeRegNo);
            var CurrentUserId = User.FindFirst("Id").Value;

            
            if (Bike.UserID == CurrentUserId )
            {
                

               var model = new UpdateListViewModel

                {

                    BikeName = Bike.BikeName,
                    BikeSN = Bike.BikeSN,
                    BikeRegNo = BikeRegNo,
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

                LostAndFoundBikeInformation lostAndFoundBike = blfRepository.GetByReg(model.BikeRegNo);
                
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
                    string FileUploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    UniqueFileName = Guid.NewGuid().ToString() + "_" + model.BikePhoto.FileName;
                    string filepath = Path.Combine(FileUploadFolder, UniqueFileName);
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
