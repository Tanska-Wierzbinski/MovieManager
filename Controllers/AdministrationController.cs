using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieManager.ViewModels;

namespace MovieManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListOfRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ListOfRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<ActionResult> EditRole(string id)
        {
            var result = await _roleManager.FindByIdAsync(id);

            if (result != null)
            {

            }
            var role = new EditRoleVM
            {
                Id = result.Id,
                Name = result.Name
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    role.Users.Add(user.UserName);
                }
            }
            return View(role);
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(EditRoleVM editRole)
        {
            var role = await _roleManager.FindByIdAsync(editRole.Id);


            role.Name = editRole.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListOfRoles", "Administration");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(editRole);
        }

        [HttpGet]
        public async Task<ActionResult> EditRoleUsers(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            var model = new List<UserRoleVM>();
            foreach (var user in _userManager.Users)
            {
                var userRole = new UserRoleVM
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }
                model.Add(userRole);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRoleUsers(List<UserRoleVM> users, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            for (int i = 0; i < users.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(users[i].UserId);

                IdentityResult result = null;

                if (users[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!users[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (users.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var deleteRole = new RoleForDeleteVM
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(deleteRole);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRole(RoleForDeleteVM deleteRole)
        {
            var role = await _roleManager.FindByIdAsync(deleteRole.Id);
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListOfRoles", "Administration");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(role);
        }
    }
}
