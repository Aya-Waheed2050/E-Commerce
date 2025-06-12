using AdminDashboard.Models.Roles;
using AdminDashboard.Models.Users;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Controllers
{
    public class UserController(UserManager<ApplicationUser> _userManager , RoleManager<IdentityRole> _roleManager) 
        : Controller
    {
      
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync(); 

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); 
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList() 
                });
            }

            return View(userViewModels);
        }

        #region Edit


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = new List<RoleViewModel>()
            };

            foreach (var role in roles)
            {
                userRoles.Roles.Add(new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(userRoles);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {

            var user = await _userManager.FindByIdAsync(model.UserId);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (roles.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!roles.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion



    }
}
