using System;
using System.Threading.Tasks;
using BikeLostAndFound.Models;
using BikeLostAndFound.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using BikeLostAndFound.Data;
using BikeLostAndFound.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeLostAndFound.Controllers
{
    [Authorize(Roles = "Admin,Administrator")]
    public class AdministrationController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBikeLostAndFoundRepository _blfRepository;
        private readonly MyDbContext _myDbContext;

        public AdministrationController(Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IBikeLostAndFoundRepository blfRepository, MyDbContext myDbContext)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _blfRepository = blfRepository;
            _myDbContext = myDbContext;
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AllRoles()
        {
            var result = roleManager.Roles;
            return View(result);
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult AllRolesAPI()
        {
            var result = roleManager.Roles;
            if (result != null)
            {
                return Json(result);
            }
            else
            {
                return BadRequest("Roles Not available");
            }
            
        }
        [HttpGet]
        public IActionResult AllRolesApiGet()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityrole = new IdentityRole { Name = model.RoleName };
                var result = await roleManager.CreateAsync(identityrole);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorTitle = "Role Not Found";
                return View("~/Views/Error/Error.cshtml");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
            };
            var list = await userManager.Users.ToListAsync();
            foreach (var user in list)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    ViewBag.ErrorTitle = "Role Not Found";
                    return View("~/Views/Error/Error.cshtml");
                }
                role.Name = model.Name;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorTitle = "Role Not Found";
                return View("~/Views/Error/Error.cshtml");
            }
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("AllRoles");
            }

            ViewBag.ErrorTitle = "Cannot Delete The Role";
            return View("~/Views/Error/Error.cshtml");
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> UserInRole(string roleId)
        {
            ViewBag.RoleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.Error = $"The User With USername ={roleId} cannot Be Found";
                return View("~/Views/Error/Error.cshtml");
            }
            var model = new List<UserInRoleViewModel>();
            var userList = await userManager.Users.ToListAsync();
            foreach (var user in userList)
            {
                var userRoleViewModel = new UserInRoleViewModel
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;

                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> UserInRole(List<UserInRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.Error = $"The User With USername ={roleId} cannot Be Found";
                return View("~/Views/Error/Error.cshtml");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByNameAsync(model[i].UserName);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole", new { roleId = roleId });
                }
            }
            return RedirectToAction("EditRole");
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var result = userManager.Users;
            return View(result);
        }


        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorTitle = "User Not Found";
                return View("~/Views/Error/Error.cshtml");
            }

            if (!(await userManager.IsInRoleAsync(user, "Admin,Administration")) || User.IsInRole("Administration"))
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllUsers");
                }
            }

            return RedirectToAction("AllUsers");
        }

        public async Task<IActionResult> LockUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorTitle = "User Not Found";
                return View("~/Views/Error/Error.cshtml");
            }

            if (!(await userManager.IsInRoleAsync(user, "Admin,Administration")) || User.IsInRole("Administration"))
            {
                await userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(2000));
            }


            return RedirectToAction("AllUsers");
        }
        public async Task<IActionResult> UnLockUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorTitle = "User Not Found";
                return View("~/Views/Error/Error.cshtml");
            }
            if (!(await userManager.IsInRoleAsync(user, "Admin,Administration")) || User.IsInRole("Administration"))
            {
                await userManager.SetLockoutEndDateAsync(user, DateTime.Now);
            }


            return RedirectToAction("AllUsers");
        }
    }
}
