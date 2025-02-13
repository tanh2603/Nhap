using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quanlybandoan.DTO;

namespace Quanlybandoan.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return instance; }
            private set { BillInfoDAO.instance = value; }
        }
        private BillInfoDAO() { }

        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete dbo.HOADONCHITIET where Mamonan = " + id);
        }

        public List<BillInfo> GetBillInfos(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.HOADONCHITIET where Mahoadon = " + id);

            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            // Cập nhật query với các tham số
            string query = "EXEC USP_InsertBillInfo @idBill, @idFood, @count";

            // Khai báo các tham số với SqlDbType để đảm bảo dữ liệu truyền đúng
            SqlParameter[] parameters =
            {
                new SqlParameter("@idBill", SqlDbType.Int) { Value = idBill },
                new SqlParameter("@idFood", SqlDbType.Int) { Value = idFood },
                new SqlParameter("@count", SqlDbType.Int) { Value = count }
            };

            // Thực thi thủ tục với tham số đã khai báo
            DataProvider.Instance.ExecuteNonQuery(query, parameters);
        }
        

    }
}
