using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Quanlybandoan.DTO;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;


namespace Quanlybandoan.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance 
        {  
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string username, string password)
        {
            /*byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }*/
            /*var list = hasData.ToString();
            list.Reverse();*/

            string query = " USP_Login @userName , @passWord ";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {username, password /*list*/});
            return result.Rows.Count > 0;
        }

        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            // Tạo tham số SqlParameter[]
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@userName", SqlDbType.NVarChar) { Value = userName },
                new SqlParameter("@displayName", SqlDbType.NVarChar) { Value = displayName },
                new SqlParameter("@password", SqlDbType.NVarChar) { Value = pass },
                new SqlParameter("@newPassword", SqlDbType.NVarChar) { Value = newPass }
            };

            // Thực thi câu lệnh với các tham số
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @userName , @displayName , @password , @newPassword", parameters);

            // Kiểm tra kết quả và trả về true hoặc false
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select Tendangnhap, Tenhienthi, Loai from dbo.TAIKHOAN");
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.TAIKHOAN where Tendangnhap = '" + userName +"'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("insert dbo.TAIKHOAN (Tendangnhap, Tenhienthi, Loai) values (N'{0}' , N'{1}', {2} )", userName, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string displayName, int type, string userName)
        {
            string query = string.Format("update dbo.TAIKHOAN set Tenhienthi = N'{0}' , Loai = {1} where Tendangnhap = N'{2}'", displayName, type, userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string userName)
        {
            string query = string.Format("Delete dbo.TAIKHOAN where Tendangnhap = N'{0}' ", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string userName)
        {
            string query = string.Format("update dbo.TAIKHOAN set Matkhau = N'123' where Tendangnhap = N'{0}' ", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
 