using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020718.BusinessLayers;
using SV21T1020718.DomainModels;
using System.Data;
using static SV21T1020718.BusinessLayers.UserAccountService;

namespace SV21T1020718.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;
            //kiem tra thong tin ddau vao
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu");
                return View();
            }
            var userAccount = UserAccountService.Authorize(UserAccountService.UserTypes.Employee, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Sai tên đăng nhập hoặc mật khẩu!");
                return View();
            }
            // Dang nhap thanh cong:
            var userData = new WebUserData
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
                Roles = userAccount.RoleNames.Split(",").ToList()
            };

            //
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenined()
        {
            return View();
        }
        public IActionResult ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            if (Request.Method == "POST")
            {
                if (confirmPassword.Trim().Equals(newPassword.Trim()) == false)
                    ModelState.AddModelError("confirmPass", "Xác nhận lại mật khẩu sai");
                if (ModelState.IsValid == false)
                {
                    return View();
                }
                else
                {
                    var result = UserAccountService.ChangePassword(UserTypes.Customer , userName, oldPassword, newPassword);
                    if (result == true)
                    {
                        return RedirectToAction("Logout");
                    }
                    else
                    {
                        ModelState.AddModelError("oldPass", "Mật khẩu cũ không đúng");
                        return View();
                    }
                }
            }
            return View();
        }
    }
}