using EmployeeManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Controllers
{
    [Authorize(Roles ="Admin")]
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
                    var result= await _roleManager.UpdateAsync(role);
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

            var users = usersInRole.Select(u => new UserRoleViewModel() { UserId = u.Id, UserName = u.UserName, IsInRole = true }).ToList();

            var allUsers = _userManager.Users.ToList();
            var extraUsers = allUsers.Except(usersInRole).Select(u => new UserRoleViewModel() { UserId = u.Id, UserName = u.UserName, IsInRole = false });

            users.AddRange(extraUsers);

            return View(users);


        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(string id, List<UserRoleViewModel> modelUsers)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }           

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            foreach(var modelUser in modelUsers)
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
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return View();
        }

    }
}
