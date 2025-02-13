using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quanlybandoan.DTO;

namespace Quanlybandoan.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return instance; }
            private set { MenuDAO.instance = value; }
        }
        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            string query = "SELECT \r\n    f.Tenmonan, \r\n    bi.Soluong, \r\n    f.DonGia, \r\n\tbi.Giamgia, \r\n    f.DonGia * bi.Soluong * (1 - ISNULL(TRY_CAST(bi.Giamgia AS FLOAT), 0) / 100) AS ThanhTien\r\nFROM dbo.HOADONCHITIET AS bi\r\nINNER JOIN dbo.HOADON AS b ON bi.Mahoadon = b.Mahoadon\r\nINNER JOIN dbo.MENU AS f ON bi.Mamonan = f.Mamonan\r\nWHERE b.Trangthai = 0 and b.Maban = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
