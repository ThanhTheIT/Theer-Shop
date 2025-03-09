using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020718.BusinessLayers;
using SV21T1020718.DomainModels;
using SV21T1020718.Shop.Models;
using System.Globalization;

namespace SV21T1020718.Shop.Controllers
{
    public class CartController : Controller
    {
        public const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";
        public const int PAGE_SIZE = 20;
        //Số mặt hàng được hiển thị trên một trang khi tìm kiếm mặt hàng để dựa vào
        //đơn hàng
        private const int PRODUCT_PAGE_SIZE = 5;
        //Tên biến session lưu điều kiện tìm kiếm mặt hàng khi lập đơn hàng
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchSale";
        //Tên biến session lưu giỏ hàng
       
        public IActionResult Index()
        {
            var model = SV21T1020718.BusinessLayers.CartDataService.GetViewCarts(Convert.ToInt32(User.GetUserData().UserId));

            return View(model);
        }
		private const string SHOPPING_CART = "ShoppingCart";
		[HttpGet]
		public IActionResult Detail(int productID)
		{
			Product? model = SV21T1020718.BusinessLayers.ProductDataService.GetProduct(productID);
			if (model == null)
			{
				// Trường hợp không tìm thấy sản phẩm
				return RedirectToAction("Index", "Home");
			}
			return View(model);
		}
        [Authorize]
        public IActionResult AddtoCart(int ProductID, int quantity)
        {
            var customer = User.GetUserData();
            var Cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId));
            if (Cart == null)
            {
                var newCart = new Cart();
                newCart.CustomerID = Convert.ToInt32(customer.UserId);
                newCart.Sum = 0;
                Cart = newCart;
                CartDataService.AddCart(Cart);
            }
            Cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId));
            var product = ProductDataService.GetProduct(ProductID);
            if (product != null)
            {
                var CartID = Cart.CartId;
                int productID = product.ProductID;
                var exists = CartDataService.checkProductExists(CartID, productID);
                if (exists == null)
                {
                    var data = new CartDetail();
                    data.Quantity = quantity;
                    data.Price = product.Price;
                    data.CartId = Cart.CartId;
                    data.ProductId = product.ProductID;
                    int id = CartDataService.AddCartDetail(data);
                    int sum = Cart.Sum + 1;
                    Cart.Sum = sum;
                    bool kq = CartDataService.SaveCart(Cart);
                }
                else
                {
                    int Quantity = exists.Quantity + quantity;
                    bool id = CartDataService.SaveCartdetail(CartID, ProductID, Quantity);
                }

            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult DeleteCart(int cartDetailID)
        {
			CartDataService.DeleteCart(cartDetailID);
			return RedirectToAction("Index");
        }
      
        private List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return shoppingCart;
        }
        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
            {
                shoppingCart.RemoveAt(index);
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        public IActionResult IndexHistory(int id=0)
        {
            return View();
        }
        public IActionResult SearchHistory()
        {
            return View();
        }
        
    }
}

