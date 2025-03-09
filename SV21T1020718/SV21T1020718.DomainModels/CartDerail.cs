using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020718.DomainModels
{
	public class CartDetail
	{
		public int CartDetailId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public int CartId { get; set; }
		public int ProductId { get; set; }
	}
}
