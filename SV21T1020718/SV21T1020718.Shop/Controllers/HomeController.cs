using Microsoft.AspNetCore.Mvc;
using SV21T1020718.BusinessLayers;
using SV21T1020718.DomainModels;
using SV21T1020718.Shop.Models;
using System.Diagnostics;

namespace SV21T1020718.Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

		private const int PAGE_SIZE = 36;
		private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
		public IActionResult Index()
		{
			ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
			if (condition == null)
			{
				condition = new ProductSearchInput()
				{
					Page = 1,
					PageSize = PAGE_SIZE,
					SearchValue = "",
					CategoryID = 0,
					SupplierID = 0,
					MinPrice = 0,
					MaxPrice = 0
				};
			}
			return View(condition);
		}
		public IActionResult Search(ProductSearchInput condition)
		{
			int rowCount;
			var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "",
														condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);
			ProductSearchResult model = new ProductSearchResult()
			{
				Page = condition.Page,
				PageSize = condition.PageSize,
				SearchValue = condition.SearchValue ?? "",
				CategoryID = condition.CategoryID,
				SupplierID = condition.SupplierID,
				MinPrice = condition.MinPrice,
				MaxPrice = condition.MaxPrice,
				RowCount = rowCount,
				Data = data
			};
			ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
			return View(model);
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		[HttpGet]
        public IActionResult ProductDetail(int ProductID)
        {
			var Product = SV21T1020718.BusinessLayers.ProductDataService.GetProduct(ProductID);
            return View(Product);
        }
        public IActionResult Support()
        {
            return View();
        }
        public IActionResult Edit(int customerId = 0)
        {
            ViewBag.Title = "Cập nhật thông tin của khách hàng";
            var data = CommonDataService.GetCustomer(customerId);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult Save(Customer data)
		{
			//TODO: Kiểm soát dữ liệu đầu vào
			ViewBag.Title = data.CustomerId == 0 ? "Bổ sung khách hàng mới" : "Cập nhật thông tin khách hàng";
			// kiểm tra nếu dữ liệu đầu vào không hợp lệ thì tạo ra một thông báo lỗi và lưu trữ vào ModelState
			if (string.IsNullOrWhiteSpace(data.CustomerName))
				ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được để trống");
			if (string.IsNullOrWhiteSpace(data.ContactName))
				ModelState.AddModelError(nameof(data.ContactName), " Tên giao tịch không được để trống");
			if (string.IsNullOrWhiteSpace(data.Phone))
				ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập điện thoại của khách hàng");
			if (string.IsNullOrWhiteSpace(data.Email))
				ModelState.AddModelError(nameof(data.Email), "vui lòng nhập Email của khách hàng");
			if (string.IsNullOrWhiteSpace(data.Address))
				ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ của khách hàng");
			if (string.IsNullOrWhiteSpace(data.Province))
				ModelState.AddModelError(nameof(data.Province), "Vui lòng  chọn  tỉnh/ thành phố cho khách hàng");
			// dựa vào thuộc tính IsVAlid của ModelState để bieetd có tồn tại lỗi hay không?
			if (ModelState.IsValid == false)
			{
				return View("Edit", data);
			}
			try
			{
				if (data.CustomerId == 0)
				{
					int id = CommonDataService.AddCustomer(data);
					if (id <= 0)
					{
						ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
						return View("Edit", data);
					}
				}
				else
				{
					bool result = CommonDataService.UpdateCustomer(data);
					if (result == false)
					{
						ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
						return View("Edit", data);
					}
				}
				return RedirectToAction("Index");
			}
			catch
			{
				ModelState.AddModelError("Error", " Hệ thống tạm thời gián đoạn");
				return View("Edit", data);
			}

		}

    }
}
