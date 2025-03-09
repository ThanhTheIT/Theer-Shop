using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020718.BusinessLayers;
using SV21T1020718.DomainModels;
using SV21T1020718.Web.Models;

namespace SV21T1020718.Web.Controllers
{
  //  [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]
    public class SupplierController : Controller
    {
        private int PAGE_SIZE = 2;
        private const string SUPPLIER_SEARCH_CONDITION = "SupplierSearchCondition";
        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(SUPPLIER_SEARCH_CONDITION);
            if (condition == null)
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            return View(condition);

        }

        public IActionResult Search(PaginationSearchInput condition)
        {
            int rowCount;
            var data = CommonDataService.ListOfSuppliers(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            SupplierSearchResult model = new SupplierSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(SUPPLIER_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp mới";
            var data = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit", data);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhà cung cấp";
            var data = CommonDataService.GetSupplier(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult Save(Supplier data)
        {
            ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp mới" : "Cập nhật thông tin nhà cung cấp";

            //Kiểm tra nếu dữ liệu đầu vào không hợp lệ thì tạo ra một thông báo lỗi và lưu trữ vào ModelState
            if (string.IsNullOrWhiteSpace(data.SupplierName))
                ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrEmpty(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Hãy chọn tỉnh thành");
            if (string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Địa chỉ nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Hãy nhập số điện thoại");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Email nhà cung cấp không được để trống");

            //Dựa vào thuộc tính IsValid của ModelState để biết có tồn tại hay không ?
            if (ModelState.IsValid == false) //!ModelState.IsValid
            {
                return View("Edit", data);
            }
            //try
            //{
                if (data.SupplierID == 0)
                {
                    int id = CommonDataService.AddSupplier(data);
                    if (id > 0)
                    {
                        //ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                        //return View("Edit", data);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    bool result = CommonDataService.UpdateSupplier(data);
                    if (result == false)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("Error", "Hệ thống tạm thời gián đoạn");
            //    return View("Edit", data);
            //}
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetSupplier(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }
    }
}
