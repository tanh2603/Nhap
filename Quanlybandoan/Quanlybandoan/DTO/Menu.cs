using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quanlybandoan.DTO
{
    public class Menu
    {
        public Menu(string foodName, int count, float price, string sale, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.Sale = sale;
            this.TotalPrice = totalPrice;
            
        }
        public Menu(DataRow row)
        {
            this.FoodName = (string)row["Tenmonan"];
            this.Count = (int)row["Soluong"];
            this.Price = (float)Convert.ToDouble(row["Dongia"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["ThanhTien"].ToString());
            this.Sale = (string)row["Giamgia"];
        }

        private string sale;
        private float totalPrice;
        private float price;
        private int count;
        private string foodName;

        public string FoodName
        {
            get { return foodName; }
            set { foodName = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public float TotalPrice
        { 
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        public string Sale
        {
            get { return sale; }
            set { sale = value; }
        }
    }
}
