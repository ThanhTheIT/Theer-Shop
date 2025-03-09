using Dapper;
using SV21T1020718.DomainModels;
using System.Data;

namespace SV21T1020718.DataLayers.SQLServer
{
    public class CustomerAccountDAL : BaseDAL, IUserAccountDAL
    {
        public CustomerAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
			UserAccount? data = null;
			using (var connection = OpenConnection())
			{
				var sql = @"
                            SELECT 
                                CustomerID AS UserId, 
                                Email AS UserName, 
                                CustomerName AS DisplayName 
                            FROM Customers 
                            WHERE Email = @Email AND Password = @Password";

				var parameters = new
				{
					Email = username,
					Password = password
				};
				data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: CommandType.Text);
				connection.Close();
			}
			return data;
		}

		public bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			bool result = false;
			using (var cn = OpenConnection())
			{
				var sql = @"update Customers 
                            set Password = @newPassword 
                            where Email = @userName and Password = @oldPassword";
				var parameters = new
				{
					userName = userName,
					oldPassword = oldPassword,
					newPassword = newPassword
				};
				result = cn.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
				cn.Close();
			}
			return result;
		}
	}
}