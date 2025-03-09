using Dapper;
using SV21T1020718.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020718.DataLayers.SQLServer
{
	public class CartDAL : BaseDAL, ICartDAL
	{
		public CartDAL(string connectString) : base(connectString)
		{
		}

		public int Add(Cart Cart)
		{
			int id = 0;

			using (var connection = OpenConnection())
			{
				var sql = @"insert into Carts(Sum, CustomerID)
                                    values(@Sum, @CustomerID);
                                    select SCOPE_IDENTITY();";
				var parameters = new
				{
					Sum = Cart.Sum,
					CustomerID = Cart.CustomerID
				};
				id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
				connection.Close();
			}
			return id;
		}

		public int AddCartDetail(CartDetail data)
		{
			int id = 0;

			using (var connection = OpenConnection())
			{
				var sql = @"insert into Cartdetails(Quantity, Price, CartID, ProductID)
                                    values(@Quantity, @Price, @CartID, @ProductID);
                                    select SCOPE_IDENTITY();";
				var parameters = new
				{
					Quantity = data.Quantity,
					Price = data.Price,
					CartID = data.CartId,
					ProductID = data.ProductId

				};
				id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
				connection.Close();
			}
			return id;
		}

		public CartDetail? checkProductExists(int CartID, int productID)
		{
			CartDetail? data = new CartDetail();

			using (var connection = OpenConnection())
			{
				var sql = @"select * from Cartdetails where CartID = @CartID and ProductID = @ProductID";
				var parameters = new
				{
					CartID = CartID,
					ProductID = productID

				};
				data = connection.QueryFirstOrDefault<CartDetail>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
				connection.Close();
			}
			return data;

		}

		public int Count(int customerID)
		{
			int count = 0;
			using (var connection = OpenConnection())
			{
				string sql = @" select count(*)
                                from Carts
                                where CustomerID = @CustomerID;
                               ";
				var parameters = new { CustomerID = customerID };
				count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
				connection.Close();
			}
			return count;
		}

		public int Delete(int customerID)
		{
			throw new NotImplementedException();
		}

		public int DeleteAll(int customerID)
		{
			throw new NotImplementedException();
		}

		public bool DeleteDetail(int cartID, int productID)
		{
			throw new NotImplementedException();
		}

		public Cart GetByID(int customerID)
		{
			var cart = new Cart();
			using (var connection = OpenConnection())
			{
				string sql = @" select * from Carts
                                where CustomerID = @CustomerID";
				var parameters = new
				{
					CustomerID = customerID
				};
				cart = connection.QueryFirstOrDefault<Cart>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
				connection.Close();
			}
			return cart;
		}

		public CartDetail GetDetail(int cartID, int productID)
		{
			throw new NotImplementedException();
		}

		public List<CartDetail> GetDetailList(int cartID)
		{
			throw new NotImplementedException();
		}

		public bool SaveCart(int customerID, int sum)
		{
			throw new NotImplementedException();
		}

		public bool SaveDetail(int CartID, int ProductID, int Quantity)
		{
			bool result = false;

			using (var connection = OpenConnection())
			{
				var sql = @"update Cartdetails set Quantity = @Quantity 
                            where CartID = @CartID and ProductID = @ProductID";
				var parameters = new
				{
					Quantity = Quantity,
					CartID = CartID,
					ProductID = ProductID

				};
				result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
				connection.Close();
			}
			return result;
		}

		public bool Update(Cart Cart)
		{
			bool result = false;

			using (var connection = OpenConnection())
			{
				var sql = @"update Carts set Sum = @Sum 
                            where CustomerID = @CustomerID and CartID = @CartID";
				var parameters = new
				{
					Sum = Cart.Sum,
					CustomerID = Cart.CustomerID,
					CartID = Cart.CartId

				};
				result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
				connection.Close();
			}
			return result;
		}
		public List<ViewCart> GetViewCarts(int userID)
		{
			List<ViewCart> viewCarts = new List<ViewCart>();
			using (var connection = OpenConnection())
			{
				var sql = @"SELECT dbo.CartDetails.CartdetailID, dbo.CartDetails.Quantity, dbo.CartDetails.Price, dbo.Carts.CustomerID, dbo.Products.ProductName, dbo.Products.Photo, dbo.Products.ProductID
							FROM     dbo.CartDetails INNER JOIN
											  dbo.Carts ON dbo.CartDetails.CartID = dbo.Carts.CartID INNER JOIN
											  dbo.Customers ON dbo.Carts.CustomerID = dbo.Customers.CustomerID INNER JOIN
											  dbo.Products ON dbo.CartDetails.ProductID = dbo.Products.ProductID
                          Where dbo.Carts.CustomerID = @CustomerID";
				var parameters = new
				{
					CustomerID = userID

				};
				viewCarts = connection.Query<ViewCart>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
			}
			return viewCarts;
		}

        public bool DeleteCart(int cartDetailID)
        {
			bool kq = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from CartDetails
									where CartDetailID = @CartDetailID";
				var parameters = new
				{
					cartDetailID = cartDetailID

                };
                kq = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
            }
            return kq;
        }
    }
}