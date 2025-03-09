using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020718.BusinessLayers;
using SV21T1020718.DomainModels;
using SV21T1020718.Web.Models;

namespace SV21T1020718.Web.Controllers
{
   // [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
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
        public IActionResult Create()
        {
            var data = new Product()
            {
                ProductID = 0,
                IsSelling = false
            };
            ViewBag.Title = "Bổ sung sản phẩm mới";
            return View("Edit", data);
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var data = ProductDataService.GetProduct(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin của mặt hàng";
            var data = ProductDataService.GetProduct(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    var data = new ProductPhoto()
                    {
                        PhotoID = 0,
                        DisplayOrder = 0,
                        ProductID = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Cập nhật ảnh cho mặt hàng";
                    var md = ProductDataService.GetPhoto(photoId);
                    if (md == null)
                        return RedirectToAction("Edit");
                    return View(md);
                case "delete":
                    //TODO: Xóa ảnh có mã photoID (Xóa trực tiếp, không cần phải xác nhận)
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính của mặt hàng";
                    var data = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính của mặt hàng";
                    var md = ProductDataService.GetAttribute(attributeId);
                    if (md == null)
                        return RedirectToAction("Edit");
                    return View(md);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Save(Product data, IFormFile? _Photo)
        {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng mới" : "Cập nhật thông tin mặt hàng";

            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được để trống");
            if (string.IsNullOrWhiteSpace(data.ProductDescription))
                ModelState.AddModelError(nameof(data.ProductDescription), "Nhập mô tả");
            if (data.CategoryID == 0)
                ModelState.AddModelError(nameof(data.CategoryID), "Tên loại hàng không được để trống");
            if (data.SupplierID == 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống");
            if (data.Price <= 0)
                ModelState.AddModelError(nameof(data.Price), "Vui lòng nhập giá");


            if (!ModelState.IsValid) // NẾu trường hợp Modelstate không hợp lệ
            {

                return View("Edit", data);
            }
            if (_Photo != null)
            {
                //Tên file sẽ lưu trên server
                string fileName = $"{DateTime.Now.Ticks}_{_Photo.FileName}";
                //Đường dẫn đến file sẽ lưu trên server (vd: D:\MyWeb\wwwroot\images\employees\photo.png)
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    _Photo.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            try
            {
                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError("Error", "Không thêm được mặt hàng.");
                        return View("Edit", data);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);
                    if (!result)
                    {
                        ModelState.AddModelError("Error", "Không cập nhật được mặt hàng.");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View("Edit", data);
            }


        }
        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? _Photo)
        {
            ViewBag.Title = data.PhotoID == 0 ? "Bổ sung ảnh mới" : "Cập nhật thông tin ảnh";
            if (string.IsNullOrWhiteSpace(data.Description))
                ModelState.AddModelError(nameof(data.Description), "Mô tả không được để trống");
            if (data.DisplayOrder == 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng chọn nhập thứ tự hiển thị");

            if (!ModelState.IsValid) // NẾu trường hợp Modelstate không hợp lệ
            {
                ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                return View("Photo", data);
            }
            try
            {
                if (_Photo != null)
                {
                    //Tên file sẽ lưu trên server
                    string fileName = $"{DateTime.Now.Ticks}_{_Photo.FileName}";
                    string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        _Photo.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                if (data.PhotoID == 0)
                {
                    long id = ProductDataService.AddPhoto(data);
                    if (id >= 0)
                    {
                        //ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                        //return View("Photo", data);
                        return RedirectToAction("Edit", new { id = data.ProductID });
                    }
                }
                else
                {
                    bool result = ProductDataService.UpdatePhoto(data);
                    if (!result)
                    {
                        //ViewBag.Title = "Cập nhật thông tin cho ảnh mặt hàng";
                        //return View("Photo", data);
                        return RedirectToAction("Edit", new { id = data.ProductID });
                    }
                }
                return RedirectToAction("Edit", new {id = data.ProductID});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View("Photo", data);
            }

        }
        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            if (string.IsNullOrWhiteSpace(data.AttributeName))
                ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính không được để trống");
            if (string.IsNullOrWhiteSpace(data.AttributeValue))
                ModelState.AddModelError(nameof(data.AttributeValue), "Giá trị thuộc tính không được để trống");
            if (data.DisplayOrder == 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Thứ tự không được để trống ");

            if (!ModelState.IsValid && data.AttributeID > 0) // NẾu trường hợp Modelstate không hợp lệ
            {
                ViewBag.Title = "Cập nhật thuộc tính của mặt hàng";
                return View("Attribute", data);
            }
            if (!ModelState.IsValid) // NẾu trường hợp Modelstate không hợp lệ
            {
                ViewBag.Title = "Bổ sung thuộc tính của mặt hàng";
                return View("Attribute", data);
            }
            if (data.AttributeID == 0)
            {
                long id = ProductDataService.AddAttribute(data);
                if (id >= 0)
                {
                    ViewBag.Title = "Bổ sung thuộc tính của mặt hàng";
                    return RedirectToAction("Edit", new {id = data.ProductID});
                }
            }
            else
            {
                bool result = ProductDataService.UpdateAttribute(data);
                if (!result)
                {
                    //ViewBag.Title = "Cập nhật thông tin thuộc tính của ảnh mặt hàng";
                    //return View("Edit", data);
                    return RedirectToAction("Edit", new { id = data.ProductID });
                }
            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }
    }
}