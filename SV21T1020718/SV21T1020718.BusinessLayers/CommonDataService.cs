
using SV21T1020718.DataLayers.SQLServer;
using SV21T1020718.DataLayers;
using SV21T1020718.DomainModels;

namespace SV21T1020718.BusinessLayers
{
	public static class CommonDataService
	{
		private static readonly ISimpleQueryDAL<Province> provinceDB;
		private static readonly ICommonDAL<Customer> customerDB;
		private static readonly ICommonDAL<Shipper> shipperDB;
		private static readonly ICommonDAL<Supplier> supplierDB;
		private static readonly ICommonDAL<Employee> employeeDB;
		private static readonly ICommonDAL<Category> categoryDB;
		/// <summary>
		/// 
		/// </summary>
		/// 
		static CommonDataService()
		{
			string connectionString = Configuration.ConnectionString;

			provinceDB = new ProvinceDAL(connectionString);
			customerDB = new CustomerDAL(connectionString);
			shipperDB = new ShipperDAL(connectionString);
			supplierDB = new SupplierDAL(connectionString);
			employeeDB = new EmployeeDAL(connectionString);
			categoryDB = new CategoryDAL(connectionString);
		}

		public static List<Province> ListOfProvinces()
		{
			return provinceDB.List();
		}
		/// <summary>
		/// Tìm kiếm và lấy danh sách khách hàng dưới dạng phân trang
		/// </summary>
		/// <param name="rowCount">Tham số đầu ra cho biết số dòng tìm được </param>
		/// <param name="page">Trang cần hiển thị</param>
		/// <param name="pageSize">Số dòng hiển thị trên mỗi trang  </param>
		/// <param name="searchValue">Tên khách hàng hoặc tên giao dịch cần tìm </param>
		/// <returns></returns>
		public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
		{
			rowCount = customerDB.Count(searchValue);
			return customerDB.List(page, pageSize, searchValue);
		}
		/// <summary>
		/// Lấy thông tin khách hàng dựa vào mã
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Customer? GetCustomer(int id)
		{
			return customerDB.GetByID(id);
		}/// <summary>
		 /// Thêm 1 khách hàng
		 /// </summary>
		 /// <param name="data"></param>
		 /// <returns></returns>
		public static int AddCustomer(Customer data)
		{
			return customerDB.Add(data);
		}
		/// <summary>
		/// Cập nhật thông tin khách hàng 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool UpdateCustomer(Customer data)
		{
			return customerDB.Update(data);
		}
		/// <summary>
		/// Xóa 1 khách hàng
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool DeleteCustomer(int id)
		{
			if (customerDB.InUse(id))
				return false;
			return customerDB.Delete(id);
		}
		/// <summary>
		/// Kiểm tra xem 1 khách hàng hiện có đơn hay không?
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool InUsedCustomer(int id)
		{
			return customerDB.InUse(id);
		}
		public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
		{
			rowCount = shipperDB.Count(searchValue);
			return shipperDB.List(page, pageSize, searchValue);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static int AddShipper(Shipper data)
		{
			return shipperDB.Add(data);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Shipper? GetShipper(int id)
		{
			return shipperDB.GetByID(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool UpdateShipper(Shipper data)
		{
			return shipperDB.Update(data);
		}
		public static bool DeleteShipper(int id)
		{
			if (shipperDB.InUse(id))
				return false;
			return shipperDB.Delete(id);
		}
		/// <summary>   
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool InUsedShipper(int id)
		{
			return shipperDB.InUse(id);
		}
		public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
		{
			rowCount = supplierDB.Count(searchValue);
			return supplierDB.List(page, pageSize, searchValue);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static int AddSupplier(Supplier data)
		{
			return supplierDB.Add(data);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Supplier? GetSupplier(int id)
		{
			return supplierDB.GetByID(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool UpdateSupplier(Supplier data)
		{
			return supplierDB.Update(data);
		}
		public static bool DeleteSupplier(int id)
		{
			if (supplierDB.InUse(id))
				return false;
			return supplierDB.Delete(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool InUsedSupplier(int id)
		{
			return supplierDB.InUse(id);
		}
		public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
		{
			rowCount = employeeDB.Count(searchValue);
			return employeeDB.List(page, pageSize, searchValue);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static int AddEmployee(Employee data)
		{
			return employeeDB.Add(data);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Employee? GetEmployee(int id)
		{
			return employeeDB.GetByID(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool UpdateEmployee(Employee data)
		{
			return employeeDB.Update(data);
		}
		public static bool DeleteEmployee(int id)
		{
			if (employeeDB.InUse(id))
				return false;
			return employeeDB.Delete(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool InUsedEmployee(int id)
		{
			return employeeDB.InUse(id);
		}
		public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
		{
			rowCount = categoryDB.Count(searchValue);
			return categoryDB.List(page, pageSize, searchValue);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static int AddCategory(Category data)
		{
			return categoryDB.Add(data);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Category? GetCategory(int id)
		{
			return categoryDB.GetByID(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool UpdateCategory(Category data)
		{
			return categoryDB.Update(data);
		}
		public static bool DeleteCategory(int id)
		{
			if (categoryDB.InUse(id))
				return false;
			return categoryDB.Delete(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool InUsedCategory(int id)
		{
			return categoryDB.InUse(id);
		}
		public static List<Category> GetAllCategory()
		{
			return categoryDB.GetAll();
		}
		public static List<Supplier> GetAllSupplier()
		{
			return supplierDB.GetAll();
		}
		public static List<Customer> GetAllCustomer()
		{
			return customerDB.GetAll();
		}
        public static int Resgister(Customer data)
        {
            return customerDB.AddResgister(data);
        }
    }
}