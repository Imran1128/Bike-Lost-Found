using BikeLostAndFound.Models;
using BikeLostAndFound.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using BikeLostAndFound.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.DataProtection;
using BikeLostAndFound.Data;

namespace BikeLostAndFound.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<AccountController> logger;
        private readonly IBikeLostAndFoundRepository blfRepository;
        private readonly IDataProtector Protector;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment, ILogger<AccountController> logger, IBikeLostAndFoundRepository BlfRepository,IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeString dataProtectionPurposeString)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            blfRepository = BlfRepository;
            Protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeString.BikeRouteValue);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UniqueFileName = null;
                if (model.Photo != null)

                {
                    string UploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    UniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string FilePath = Path.Combine(UploadFolder, UniqueFileName);
                    model.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));
                }
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Gender = model.Gender,

                    PhotoPath = UniqueFileName


                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var ConfirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    Sendemail(model.Email, "Confirmation Email", ConfirmationLink,
                        "Please verify your email address to complete the registration");
                    ViewBag.SuccessTitle = "Registration Successful";
                    ViewBag.SuccessMessage = "Please Confirm Your Email to Login";
                    return View("~/Views/Success/Success.cshtml");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task Sendemail(String RecieverEmail, string subject, string body, string message)
        {
            var mail = new MailMessage();
            mail.Subject = subject;
            mail.Body = $"<h1>Confirmation Email</h1><p>{message}</p><p><a href='{body}'>Click here to verify </a></p>";
            mail.To.Add(new MailAddress(RecieverEmail));
            mail.From = new MailAddress("bikelostandfound@gmail.com");
            mail.IsBodyHtml = true;
            var smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("bikelostandfound@gmail.com", "kfsamowvjqemwrjr");
            smtpClient.EnableSsl = true;
            await smtpClient.SendMailAsync(mail);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null && token == null)
            {
                RedirectToAction("Index", "home");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User Not Found";
                return View("~/Views/Error/Error.cshtml");
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorMessage = $"Email {user.Email} cannot be confirmed";
            return View("~/Views/Error/Error.cshtml");
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> EmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(false);
            }

            return Json(true);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User Not Found");
                    return View(model);
                }

                if (!user.EmailConfirmed && (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError("", "Email is not confirmed yet");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                user.LastloginDate = DateTimeOffset.UtcNow;
                await userManager.UpdateAsync(user);
                var userLockoutTime = await userManager.GetLockoutEndDateAsync(user);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", userLockoutTime > DateTime.Now.AddMinutes(15) ?
                        "User is Blocked" :
                        "User is Blocked for 15 minutes");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                }
            }

            // Re-populate the ReturnUrl in case it was null or if ModelState is invalid
            ViewData["ReturnUrl"] = ReturnUrl;
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(user);
                    ViewBag.SuccessTitle = "Your Password is changed successfully";
                    return View("~/Views/Success/Success.cshtml");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string Id)
        {
            var decryptedId = Protector.Unprotect(Id);
            var CurrentUser = User.FindFirst("Id").Value;
            if (decryptedId == CurrentUser)
            {
                var user = await userManager.FindByIdAsync(decryptedId);

                if (user == null)
                {
                    ViewBag.ErrorTitle = "User Not Found or Wrong User";
                    return View("~/Views/Error/Error.cshtml");
                }

                var model = new UpdateUserViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    ExistingPhoto = user.PhotoPath
                };
                return View(model);

            }
            else
            {
                ViewBag.ErrorTitle = "User Not Found or Wrong User";
                return View("~/Views/Error/Error.cshtml");
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ViewBag.ErrorTitle = "User not found";
                    return View("~/Views/Error/Error.cshtml");
                }

                user.Name = model.Name;
                user.Email = model.Email;
                user.Address = model.Address;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                if (model.Photo != null)
                {
                    user.PhotoPath = FileUploadProcess(model);
                    if (model.ExistingPhoto != null)
                    {
                        var filepath = Path.Combine(webHostEnvironment.WebRootPath, "Images", model.ExistingPhoto);
                        System.IO.File.Delete(filepath);
                    }
                }

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("MyList", "Profile");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View();
        }

        private string FileUploadProcess(UpdateUserViewModel model)
        {
            string UniqueFileName = null;
            if (ModelState.IsValid)
            {

                {
                    string FileUploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    UniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filepath = Path.Combine(FileUploadFolder, UniqueFileName);
                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        model.Photo.CopyTo(filestream);
                    }
                }
            }

            return UniqueFileName;
        }
        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.IsEmailConfirmedAsync(user))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var PasswordResetLink = Url.Action("ResetPassword", "Account",
                    new { Email = model.Email, token = token }, Request.Scheme);
                Sendemail(model.Email, "Reset Password", PasswordResetLink,
                    "Please verify your email address to reset password");
                ViewBag.SuccessMessage = "If you  have any account with this email a password reset link has been sent to your email";
                return View("~/Views/Success/Success.cshtml"); ;
            }
            ViewBag.SuccessMessage = "If you  have any account with this email a password reset link has been sent to your email";
            return View("~/Views/Success/Success.cshtml"); ;
        }

        [HttpGet]
        public IActionResult ResetPassword(string Email, string token)
        {
            if (Email == null || token == null)
            {
                ModelState.AddModelError("", "Invalid Reset Password Link");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var result = await userManager.ResetPasswordAsync(user, model.token, model.Password);
                if (result.Succeeded)
                {
                    var lockedOutTime = await userManager.GetLockoutEndDateAsync(user);
                    if (lockedOutTime.HasValue && lockedOutTime.Value <= DateTime.Now.AddMinutes(15))
                    {
                        await userManager.SetLockoutEndDateAsync(user, DateTime.Now);
                    }
                    ViewBag.SuccessMessage = "Your Password is changed successfully";
                    return View("~/Views/Success/Success.cshtml"); ;
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            ViewBag.SuccessTitle = "Your Password is changed successfully";
            ViewBag.SuccessMessage = "Please Login to your account";
            return View("~/Views/Success/Success.cshtml"); ;
        }


    }
}
