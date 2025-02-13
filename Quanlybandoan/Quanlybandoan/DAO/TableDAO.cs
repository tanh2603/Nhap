using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quanlybandoan.DTO;

namespace Quanlybandoan.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90;

        public TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            /// Tạo các tham số SqlParameter
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idTable1", SqlDbType.Int) { Value = id1 },
                new SqlParameter("@idTable2", SqlDbType.Int) { Value = id2 }
            };

            // Thực thi truy vấn với tham số
            string query = "EXEC USP_SwitchTable @idTable1, @idTable2";
            DataProvider.Instance.ExecuteNonQuery(query, parameters);

        }

        public void GopTable(int id1, int id2)
        {
            // Tạo các tham số SqlParameter
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idTable1", SqlDbType.Int) { Value = id1 },
                new SqlParameter("@idTable2", SqlDbType.Int) { Value = id2 }
            };

            // Thực thi truy vấn với tham số
            string query = "EXEC USP_GopBan @idTable1, @idTable2";
            DataProvider.Instance.ExecuteNonQuery(query, parameters);

        }


        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public List<Table> GetListTable()
        {
            List<Table> list = new List<Table>();

            string query = "select * from dbo.BAN";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                list.Add(table);
            }

            return list;
        }

        public bool InsertTable(string name, string status)
        {
            string query = string.Format("insert dbo.BAN (Tenban, Trangthai) values (N'{0}' , N'{1}' )", name, status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTable(int idTable, string name, string status)
        {
            string query = string.Format("update dbo.BAN set Tenban = N'{0}', Trangthai = N'{1}' where Maban = {2} ", name, status, idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteTable(int idTable)
        {
            BillDAO.Instance.DeleteBillByTableID(idTable);

            string query = string.Format("Delete dbo.BAN where Maban = {0} ", idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
