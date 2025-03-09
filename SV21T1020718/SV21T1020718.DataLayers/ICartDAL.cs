using SV21T1020718.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020718.DataLayers
{
	public interface ICartDAL
	{
		public int Add(Cart data);
		public int AddCartDetail(CartDetail data);
		public int Delete(int customerID);
		public bool Update(Cart data);
		public int Count(int customerID);
		public Cart GetByID(int cutomerID);
		public List<CartDetail> GetDetailList(int cartID);
		public CartDetail GetDetail(int cartID, int productID);
		bool SaveDetail(int cartID, int productID, int quantity);
		bool DeleteDetail(int cartID, int productID);
		bool SaveCart(int customerID, int sum);
		public CartDetail? checkProductExists(int cartID, int productID);
		public List<ViewCart> GetViewCarts(int userID);
		public bool DeleteCart(int cartDetailID );
	}
}