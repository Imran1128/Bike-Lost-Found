using System.Threading.Tasks;
using BikeLostAndFound.Models;
using BikeLostAndFound.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace BikeLostAndFound.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            
        }

        public async Task<IActionResult> AllRoles()
        {
            var result =  roleManager.Roles;
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }

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
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }

            return View(model);
        }

    }
}
