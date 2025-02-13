using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlybandoan.DTO
{
    public class Account
    {
        public Account(string userName, string displayName, int type, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }

        public Account(DataRow row)
        {
            this.UserName = row["Tendangnhap"].ToString();
            this.DisplayName = row["Tenhienthi"].ToString();
            this.Type = (int)row["Loai"];
            this.Password = row["Matkhau"].ToString();
        }

        private string userName;
        private string password;
        private string displayName;
        private int type;

        public string UserName
        {
            get { return userName; }
            private set { userName = value; }
        }
        
        public string DisplayName
        {
            get { return displayName; }
            private set { displayName = value; }
        }
        public int Type
        {
            get { return type; }
            private set { type = value; }
        }

        public string Password
        {
            get { return password; }
            private set { password = value; }
        }
    }
}
