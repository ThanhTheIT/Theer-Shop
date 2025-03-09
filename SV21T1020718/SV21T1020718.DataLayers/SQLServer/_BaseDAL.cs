using Microsoft.Data.SqlClient;

namespace SV21T1020718.DataLayers.SQLServer
{
    /// <summary>
    /// Lớp cơ sở (lớp cha) của các lớp cài đặt các phép xử lý dữ liệu trên SQL Server
    /// </summary>
    public abstract class BaseDAL
    {
        /// <summary>
        /// Chuỗi tham số kêts nối đến CSDL SQL Server
        /// </summary>
        protected string connectionString = "";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Tạo và mở một kết nối đến CSDL (SQL Server)
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection("server=THANHTHE;user id=sa;password=123;database=LiteCommerceDB;TrustServerCertificate=True");
            connection.Open();
            return connection;
        }
    }
}