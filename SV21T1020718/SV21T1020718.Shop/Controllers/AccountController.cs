using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using SV21T1020718.BusinessLayers;
using SV21T1020718.DomainModels;
using static SV21T1020718.BusinessLayers.UserAccountService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020718.Shop.Controllers
{
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
			var userAccount = UserAccountService.Authorize(UserAccountService.UserTypes.Customer, username, password);
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
					var result = UserAccountService.ChangePassword(UserTypes.Customer, userName, oldPassword, newPassword);
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
		public IActionResult Resgister()
		{
			ViewBag.Title = "Bổ sung khách hàng mới";
			var data = new Customer()
			{
				CustomerId = 0,
				IsLocked = false
			};
			return View( data);
		}
		[HttpPost]
        public IActionResult Save(Customer data, string password, string confirmpass)
        {

            //Kiểm tra nếu dữ liệu đầu vào không hợp lệ thì tạo ra một thông báo lỗi và lưu trữ vào ModelState
            if (string.IsNullOrWhiteSpace(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được để trống");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Email khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Địa chỉ khách hàng không được để trống");
            if (string.IsNullOrEmpty(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Hãy chọn tỉnh thành");
            if (password != confirmpass)
                ModelState.AddModelError("Error", "Xác nhật mật mật khẩu sai");
            //Dựa vào thuộc tính IsValid của ModelState để biết có tồn tại hay không ?
            if (ModelState.IsValid == false) //!ModelState.IsValid
            {
                return View("Resgister", data);
            }
            
					data.password = password;
                    int id = CommonDataService.Resgister(data);
                   
                        ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                        return RedirectToAction("Login");
        }
    }
}
