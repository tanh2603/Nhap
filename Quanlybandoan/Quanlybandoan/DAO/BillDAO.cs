using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quanlybandoan.DTO;

namespace Quanlybandoan.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }
        //Thanh cong : bill id
        //That bai -1
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.HOADON where Maban = " + id + " and Trangthai = 0");

            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "UPDATE dbo.HOADON SET Ngayxuathoadon = GETDATE() , Trangthai = 1, " + "Giamgiabill = " + discount + ", Tongtien = " + totalPrice + " WHERE Mahoadon = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void InsertBill(int id)
        {
            // Tạo danh sách tham số SQL an toàn
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@idTable", SqlDbType.Int) { Value = id }
            };

            // Thực thi thủ tục lưu trữ với tham số
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable", parameters);
        }

        public DataTable GetListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            string query = $"EXEC USP_GetListBillByDate @checkIn , @checkOut ";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { checkIn.ToString("yyyy-MM-dd"), checkOut.ToString("yyyy-MM-dd") });
        }

        public DataTable GetListBillByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNumber)
        {
            string query = $"EXEC USP_GetListBillByDateAndPage @checkIn , @checkOut , @page ";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { checkIn.ToString("yyyy-MM-dd"), checkOut.ToString("yyyy-MM-dd"), pageNumber});
        }

        public int GetNumListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            string query = $"EXEC USP_GetNumBillByDate @checkIn , @checkOut ";
            return (int)DataProvider.Instance.ExecuteScalar(query, new object[] { checkIn.ToString("yyyy-MM-dd"), checkOut.ToString("yyyy-MM-dd") });
        }

        public void DeleteBillByTableID(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete dbo.HOADON where Maban = " + id);
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select max(Mahoadon) from dbo.Hoadon");
            }
            catch
            {
                return 1;
            }
            
        }

        public DataTable Exportfile(DataGridView dgv)
        {
            DataTable dataTable = new DataTable();

            // Định nghĩa cột theo thứ tự mới
            dataTable.Columns.Add("Tenban", typeof(string));
            dataTable.Columns.Add("Ngaylaphoadon", typeof(DateTime));
            dataTable.Columns.Add("Ngayxuathoadon", typeof(DateTime));
            dataTable.Columns.Add("Giamgiabill", typeof(decimal));
            dataTable.Columns.Add("Tongtien", typeof(decimal));

            // Lặp qua từng dòng của DataGridView và thêm vào DataTable
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow) // Bỏ qua dòng trống cuối
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["Tenban"] = row.Cells[0].Value ?? DBNull.Value;
                    dataRow["Ngaylaphoadon"] = row.Cells[3].Value ?? DBNull.Value;
                    dataRow["Ngayxuathoadon"] = row.Cells[4].Value ?? DBNull.Value;
                    dataRow["Giamgiabill"] = row.Cells[2].Value ?? DBNull.Value;
                    dataRow["Tongtien"] = row.Cells[1].Value ?? DBNull.Value;

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }


    }
}
