using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quanlybandoan.DTO;

namespace Quanlybandoan.DAO
{
    public class TypeAccountDAO
    {
        private static TypeAccountDAO instance;

        public static TypeAccountDAO Instance
        {
            get { if (instance == null) instance = new TypeAccountDAO(); return instance; }
            private set { instance = value; }
        }

        private TypeAccountDAO() { }

        public List<TypeAccount> GetListType()
        {
            List<TypeAccount> list = new List<TypeAccount>();

            string query = "select * from dbo.LOAITAIKHOAN";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                TypeAccount typeaccount = new TypeAccount(item);
                list.Add(typeaccount);
            }

            return list;
        }

        public TypeAccount GetAccountByIDLoai(int id)
        {
            TypeAccount typeAccount = null;

            string query = "select * from dbo.LOAITAIKHOAN where Maloaitk = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                typeAccount = new TypeAccount(item);
                return typeAccount;
            }

            return typeAccount;
        }
    }
}
