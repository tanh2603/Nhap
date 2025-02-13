using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quanlybandoan.DTO;

namespace Quanlybandoan.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "select * from dbo.LOAIMONAN";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }

            return list;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;

            string query = "select * from dbo.LOAIMONAN where Maloai = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }

            return category;
        }

        public bool InsertCatelogy(string name)
        {
            string query = string.Format("insert dbo.LOAIMONAN (Tenloai) values (N'{0}' )", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCatelogy(string name, int idCategory)
        {
            string query = string.Format("update dbo.LOAIMONAN set Tenloai = N'{0}' where Maloai = {1} ", name, idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCatelogy(int idCategory)
        {

            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idCategory);
            FoodDAO.Instance.DeleteFoodByCategoryID(idCategory);

            string query = string.Format("Delete dbo.LOAIMONAN where Maloai = {0} ", idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
