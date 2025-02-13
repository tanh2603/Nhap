using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlybandoan.DTO
{
    public class BillInfo
    {
        public BillInfo(int id, int billID, int foodID, int count, string sale)
        {
            this.ID = id;
            this.BillID = billID;
            this.FoodID = foodID;
            this.Count = count;
            this.Sale = sale;
        }

        public BillInfo(DataRow row)
        {
            this.ID = (int)row["MaHDCT"];
            this.BillID = (int)row["Mahoadon"];
            this.FoodID = (int)row["Mamonan"];
            this.Count = (int)row["Soluong"];
            this.Sale = (string)row["Giamgia"];
        }

        private int iD;

        public int ID
        {
            get { return iD; }
            private set { iD = value; }
        }
        private int billID;

        public int BillID
        {
            get { return billID; }
            private set { billID = value; }
        }
        private int foodID;

        public int FoodID
        {
            get { return foodID; }
            private set { foodID = value; }
        }
        private int count;

        public int Count
        {
            get { return count; }
            private set { count = value; }
        }
        private string sale;

        public string Sale
        {
            get { return sale; }
            private set { sale = value; }
        }

    }
}
