using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.IdentityDtos;

namespace AdminDashboard.Controllers
{
    public class AdminController(SignInManager<ApplicationUser> _signInManager , UserManager<ApplicationUser> _userManager) 
        : Controller
    {

        [HttpGet]
        public IActionResult Login() => View();
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid login attempt. Please check your email and password.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded || !await _userManager.IsInRoleAsync(user , "Admin"))
            {
                ModelState.AddModelError("", "You are not authorized.");
                return View(model);
            }
            return RedirectToAction("Index" , "Home");
        }


        public async Task<IActionResult> Logout(LoginDto model)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Admin");
        }

    }
}
