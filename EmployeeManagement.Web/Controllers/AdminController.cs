using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole { Name = createRoleViewModel.RoleName };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var model = new EditRoleViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name
            };



            foreach (var user in await _userManager.GetUsersInRoleAsync(role.Name))
            {
                model.Users.Add(user.UserName);
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                if (role == null)
                {
                    return NotFound();
                }
                else
                {
                    role.Name = model.RoleName;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("index", "admin");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        [Authorize(Policy="DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("index", "admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            ViewBag.RoleId = id;

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            var users = usersInRole.Select(u => new RoleUserViewModel() { UserId = u.Id, UserName = u.UserName, IsInRole = true }).ToList();

            var allUsers = _userManager.Users.ToList();
            var extraUsers = allUsers.Except(usersInRole).Select(u => new RoleUserViewModel() { UserId = u.Id, UserName = u.UserName, IsInRole = false });

            users.AddRange(extraUsers);

            return View(users);


        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(string id, List<RoleUserViewModel> modelUsers)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            foreach (var modelUser in modelUsers)
            {
                var user = await _userManager.FindByIdAsync(modelUser.UserId);
                if (usersInRole.Any(u => u.Id == modelUser.UserId))
                {
                    if (!modelUser.IsInRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
                else
                {
                    if (modelUser.IsInRole)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
            }

            return RedirectToAction("editrole", "admin", new { id = id });


        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var userModel = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles,
                Claims = claims.Select(c => c.Value).ToList()
            };

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userModel.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = userModel.UserName;

                user.Email = userModel.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("listusers", "admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("listusers", "admin");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("ListUsers");
        }


        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.UserId = id;

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = _roleManager.Roles.Select(r => new UserRoleViewModel() { RoleId = r.Id, RoleName = r.Name, IsInRole = false }).ToList();

            roles.Where(r => userRoles.Contains(r.RoleName)).ToList().ForEach(r => r.IsInRole = true);

            return View(roles.ToList());

        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string id, List<UserRoleViewModel> modelRoles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var modelRole in modelRoles)
            {
                
                if (userRoles.Contains(modelRole.RoleName))
                {
                    if (!modelRole.IsInRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, modelRole.RoleName);
                    }
                }
                else
                {
                    if (modelRole.IsInRole)
                    {
                        await _userManager.AddToRoleAsync(user, modelRole.RoleName);
                    }
                }
            }

            return RedirectToAction("edituser", "admin", new { id = id });


        }

        [HttpGet]
        public async Task<IActionResult> EditUserClaims(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.UserId = id;

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = ClaimsStore.AllClaims.Select(c => new UserClaimsViewModel() { ClaimType=c.Type, IsSelected=false  }).ToList();

            claims.Where(c => userClaims.Any(uc=>uc.Type==c.ClaimType )).ToList().ForEach(c => c.IsSelected = true);

            return View(claims.ToList());

        }

        [HttpPost]
        public async Task<IActionResult> EditUserClaims(string id, List<UserClaimsViewModel> modelClaims)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            await _userManager.RemoveClaimsAsync(user, userClaims);

            await _userManager.AddClaimsAsync(user, modelClaims.Where(c=>c.IsSelected).Select(c=>new Claim(c.ClaimType, c.ClaimType)));

            return RedirectToAction("edituser", "admin", new { id = id });
        }
    }
}
