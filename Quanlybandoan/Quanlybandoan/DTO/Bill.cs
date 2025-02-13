using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quanlybandoan.DAO;

namespace Quanlybandoan.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dateCheckin, DateTime? dateCheckout, int status, int discount)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckin;
            this.DateCheckOut = dateCheckout;
            this.Status = status;
            this.Discount = discount;
        }

        public Bill(DataRow row)
        {
            this.ID = (int)row["Mahoadon"];
            this.DateCheckIn = (DateTime?)row["Ngaylaphoadon"];
            var dateCheckOutTemp = row["Ngayxuathoadon"];
            if(dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            this.Status = (int)row["Trangthai"];
            if (row["Giamgiabill"] != DBNull.Value)
                this.Discount = Convert.ToInt32(row["Giamgiabill"]);
            else
                this.Discount = 0; // Giá trị mặc định nếu NULL
        }

        private int discount;
        private int status;
        private DateTime? dateCheckOut;
        private DateTime? dateCheckIn;
        private int iD;

        public int ID
        {
            get {  return iD; }
            private set { iD = value; }
        }

        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            private set { dateCheckIn = value; }
        }

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            private set { dateCheckOut = value; }
        }

        public int Status
        {
            get { return status; }
            private set { status = value; }
        }

        public int Discount
        {
            get { return discount; }
            private set { discount = value; }
        }
    }
}
