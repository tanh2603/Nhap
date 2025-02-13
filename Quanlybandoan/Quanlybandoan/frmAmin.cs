using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Quanlybandoan.DAO;
using Quanlybandoan.DTO;

namespace Quanlybandoan
{
    public partial class frmAmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource CatelogyList = new BindingSource();
        BindingSource TableList = new BindingSource();
        BindingSource AccountList = new BindingSource();

        public Account loginAccount;

        public frmAmin()
        {
            InitializeComponent();
            Load();
        }
        #region methods

        List<Food> SearchFoodByname(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }

        void Load()
        {
            dgvFood.DataSource = foodList;
            dgvDanhmuc.DataSource = CatelogyList;
            dgvBan.DataSource = TableList;
            dgvTaikhoan.DataSource = AccountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            AddFoodBinding();
            LoadListCatelogy();
            AddCatelogyBinding();
            LoadListTable();
            AddTableBinding();
            LoadListAccount();
            AddAccountBinding();
            LoadCatelogyIntoCombobox(cbxLoaimonan);
            LoadStatusIntoCombobox(cbxTrangthai);
            LoadAccountIntoCombobox(cbxLoaitaikhoan);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvDoanhthu.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);

            dgvDoanhthu.Columns["Tenban"].HeaderText = "Tên bàn";
            dgvDoanhthu.Columns["Tongtien"].HeaderText = "Tổng tiền";
            dgvDoanhthu.Columns["Giamgiabill"].HeaderText = "Giảm giá";
            dgvDoanhthu.Columns["Ngaylaphoadon"].HeaderText = "Ngày lập hóa đơn";
            dgvDoanhthu.Columns["Ngayxuathoadon"].HeaderText = "Ngày xuất hóa đơn";
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();

            dgvFood.Columns["ID"].HeaderText = "Mã món ăn";
            dgvFood.Columns["Name"].HeaderText = "Tên món ăn";
            dgvFood.Columns["CategoryID"].HeaderText = "Mã loại";
            dgvFood.Columns["Price"].HeaderText = "Đơn giá";
        }

        void LoadListCatelogy()
        {
            CatelogyList.DataSource = CategoryDAO.Instance.GetListCategory();

            dgvDanhmuc.Columns["ID"].HeaderText = "Mã loại món ăn";
            dgvDanhmuc.Columns["Name"].HeaderText = "Tên loại món ăn";
        }

        void LoadListTable()
        {
            TableList.DataSource = TableDAO.Instance.GetListTable();

            dgvBan.Columns["ID"].HeaderText = "Mã bàn";
            dgvBan.Columns["Name"].HeaderText = "Tên bàn";
            dgvBan.Columns["Status"].HeaderText = "Trạng thái";
        }

        void LoadListAccount()
        {
            AccountList.DataSource = AccountDAO.Instance.GetListAccount();

            dgvTaikhoan.Columns["Tendangnhap"].HeaderText = "Tên đăng nhập";
            dgvTaikhoan.Columns["Tenhienthi"].HeaderText = "Tên hiển thị";
            dgvTaikhoan.Columns["Loai"].HeaderText = "Loại tài khoản";
        }

        void AddFoodBinding()
        {
            txtMamonan.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtTenmonan.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            numGia.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void AddCatelogyBinding()
        {
            txtMadanhmuc.DataBindings.Add(new Binding("Text", dgvDanhmuc.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtDanhmuc.DataBindings.Add(new Binding("Text", dgvDanhmuc.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void AddTableBinding()
        {
            txtMaban.DataBindings.Add(new Binding("Text", dgvBan.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtTenban.DataBindings.Add(new Binding("Text", dgvBan.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBinding()
        {
            txtTentaikhoan.DataBindings.Add(new Binding("Text", dgvTaikhoan.DataSource, "Tendangnhap", true, DataSourceUpdateMode.Never));
            txtTenhienthi.DataBindings.Add(new Binding("Text", dgvTaikhoan.DataSource, "Tenhienthi", true, DataSourceUpdateMode.Never));
        }

        void LoadCatelogyIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadStatusIntoCombobox(ComboBox cb)
        {
            List<string> statusList = new List<string>() { "Trống", "Có người" };

            cb.DataSource = statusList;
        }

        void LoadAccountIntoCombobox(ComboBox cb)
        {
            cb.DataSource = TypeAccountDAO.Instance.GetListType();
            cb.DisplayMember = "Name";
        }

        #endregion

        #region Events
        private void btnThongke_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void txtMamonan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbxLoaimonan.SelectedItem = category;

                    // Tìm kiếm trong danh sách các Category và gán lại nếu không chọn được đúng
                    foreach (Category item in cbxLoaimonan.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            cbxLoaimonan.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void dgvBan_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra nếu có dòng nào được chọn trong dgvBan
            if (dgvBan.SelectedCells.Count > 0)
            {
                // Lấy giá trị của cột "Status" từ dòng đã chọn
                string status = dgvBan.SelectedCells[0].OwningRow.Cells["Status"].Value.ToString();

                // Cập nhật giá trị của ComboBox (chỉ hiển thị theo giá trị cột Trangthai)
                cbxTrangthai.Text = status;
            }
        }

        private void txtTentaikhoan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvTaikhoan.SelectedCells.Count > 0)
                {
                    int id = (int)dgvTaikhoan.SelectedCells[0].OwningRow.Cells["Loai"].Value;

                    TypeAccount typeAccount = TypeAccountDAO.Instance.GetAccountByIDLoai(id);

                    cbxLoaitaikhoan.SelectedItem = typeAccount;

                    // Tìm kiếm trong danh sách các Category và gán lại nếu không chọn được đúng
                    foreach (TypeAccount item in cbxLoaitaikhoan.Items)
                    {
                        if (item.ID == typeAccount.ID)
                        {
                            cbxLoaitaikhoan.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtTenmonan.Text;
            int catelogyID = (cbxLoaimonan.SelectedItem as Category).ID;
            float price = (float)numGia.Value;

            if (FoodDAO.Instance.InsertFood(name, catelogyID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm món không thành công");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string name = txtTenmonan.Text;
            int catelogyID = (cbxLoaimonan.SelectedItem as Category).ID;
            float price = (float)numGia.Value;
            int id = Convert.ToInt32(txtMamonan.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, catelogyID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa món không thành công");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtMamonan.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa món không thành công");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private void btnThemloai_Click(object sender, EventArgs e)
        {
            string name = txtDanhmuc.Text;

            if (CategoryDAO.Instance.InsertCatelogy(name))
            {
                MessageBox.Show("Thêm loại món ăn thành công");
                LoadListCatelogy();
                if (insertCategory != null)
                    insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm loại món ăn không thành công");
            }
        }

        private void btnSualoai_Click(object sender, EventArgs e)
        {
            string name = txtDanhmuc.Text;
            int id = Convert.ToInt32(txtMadanhmuc.Text);

            if (CategoryDAO.Instance.UpdateCatelogy(name, id))
            {
                MessageBox.Show("Sửa loại món ăn thành công");
                LoadListCatelogy();
                if (updateCategory != null)
                    updateCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa loại món ăn không thành công");
            }
        }

        private void Xoaloai_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtMadanhmuc.Text);

            if (CategoryDAO.Instance.DeleteCatelogy(id))
            {
                MessageBox.Show("Xóa loại món ăn thành công");
                LoadListCatelogy();
                if (deleteCategory != null)
                    deleteCategory(this, new EventArgs());
                    
            }
            else
            {
                MessageBox.Show("Xóa loại món ăn không thành công");
            }
        }

        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }

        private void btnThemban_Click(object sender, EventArgs e)
        {
            string name = txtTenban.Text;
            string status = cbxTrangthai.SelectedItem.ToString();

            if (TableDAO.Instance.InsertTable(name, status))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadListTable();
                if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm bàn không thành công");
            }
        }

        private void btnSuaban_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtMaban.Text);
            string name = txtTenban.Text;
            string status = cbxTrangthai.SelectedItem.ToString();

            if (TableDAO.Instance.UpdateTable(id, name, status))
            {
                MessageBox.Show("Sửa thông tin bàn thành công");
                LoadListTable();
                if (updateTable != null)
                    updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa thông tin bàn không thành công");
            }
        }

        private void btnXoaban_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtMaban.Text);

            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công");
                LoadListTable();
                if (deleteTable != null)
                    deleteTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa bàn không thành công");
            }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByname(txtSearchFood.Text);
        }

        private void btnThemAcc_Click(object sender, EventArgs e)
        {
            string userName = txtTentaikhoan.Text;
            string displayName = txtTenhienthi.Text;
            int type = (cbxLoaitaikhoan.SelectedItem as TypeAccount).ID;

            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
                LoadListAccount();
                if (insertAccount != null)
                    insertAccount(this, new EventArgs());

            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
        }

        private void btnSuaAcc_Click(object sender, EventArgs e)
        {
            string userName = txtTentaikhoan.Text;
            string displayName = txtTenhienthi.Text;
            int type = (cbxLoaitaikhoan.SelectedItem as TypeAccount).ID;

            if (AccountDAO.Instance.UpdateAccount(displayName, type, userName))
            {
                MessageBox.Show("Sửa tài khoản thành công");
                LoadListAccount();
                if (updateAccount != null)
                    updateAccount(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại");
            }
        }

        private void btnXoaacc_Click(object sender, EventArgs e)
        {
            string userName = txtTentaikhoan.Text;

            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không được xóa tài khoản đang đăng nhập ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
                LoadListAccount();
                if (deleteAccount != null)
                    deleteAccount(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
        }

        private void btnDatlaimk_Click(object sender, EventArgs e)
        {
            string userName = txtTentaikhoan.Text;

            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Reset mật khẩu của tài khoản thành công");
                LoadListAccount();
                if (resetPassword != null)
                    resetPassword(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Reset mật khẩu của tài khoản thất bại");
            }
        }

        private event EventHandler insertAccount;
        public event EventHandler InsertAccount
        {
            add { insertAccount += value; }
            remove { insertAccount -= value; }
        }

        private event EventHandler updateAccount;
        public event EventHandler UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        private event EventHandler deleteAccount;
        public event EventHandler DeleteAccount
        {
            add { deleteAccount += value; }
            remove { deleteAccount -= value; }
        }

        private event EventHandler resetPassword;
        public event EventHandler ResetPassword
        {
            add { resetPassword += value; }
            remove { resetPassword -= value; }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            txtPage.Text = "1";

            dgvDoanhthu.Columns["Mahoadon"].HeaderText = "Mã hóa đơn";
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txtPage.Text = lastPage.ToString();
            dgvDoanhthu.Columns["Mahoadon"].HeaderText = "Mã hóa đơn";
        }

        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            dgvDoanhthu.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txtPage.Text));
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPage.Text);

            if (page > 1)
                page--;

            txtPage.Text = page.ToString();
            dgvDoanhthu.Columns["Mahoadon"].HeaderText = "Mã hóa đơn";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPage.Text);
            int sumRecord = BillDAO.Instance.GetNumListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txtPage.Text = page.ToString();
            dgvDoanhthu.Columns["Mahoadon"].HeaderText = "Mã hóa đơn";
        }

        public void ExportFileExcel(DataTable dataTable, string sheetName, string title)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo các đối tượng Excel
            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
            oExcel.Visible = true;
            oExcel.DisplayAlerts = false;

            Microsoft.Office.Interop.Excel.Workbook oBook = oExcel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets[1];
            oSheet.Name = sheetName;

            // Tạo phần tiêu đề
            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "E1");
            head.MergeCells = true;
            head.Value2 = title;
            head.Font.Bold = true;
            head.Font.Size = 20;
            head.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SeaGreen);
            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Tạo tiêu đề cột với thứ tự mới
            string[] columnNames = { "Tên bàn", "Ngày lập hóa đơn", "Ngày xuất hóa đơn", "Giảm giá \n (%)", "Tổng tiền \n (đồng)" };
            string[] columnLetters = { "A", "B", "C", "D", "E" };
            double[] columnWidths = { 12, 15, 20, 12, 25 }; // Điều chỉnh kích thước phù hợp

            for (int i = 0; i < columnNames.Length; i++)
            {
                Microsoft.Office.Interop.Excel.Range cl = oSheet.get_Range(columnLetters[i] + "3", columnLetters[i] + "3");
                cl.Value2 = columnNames[i];
                cl.ColumnWidth = columnWidths[i];
                cl.Font.Bold = true;
                cl.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
                cl.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // Căn giữa cả theo chiều ngang và dọc
                cl.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                cl.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Thêm màu nền 
                cl.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 235, 219));
            }

            // Tạo mảng dữ liệu
            object[,] arr = new object[dataTable.Rows.Count, dataTable.Columns.Count];

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                DataRow dataRow = dataTable.Rows[row];
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    arr[row, col] = dataRow[col] ?? ""; // Kiểm tra null tránh lỗi
                }
            }

            // Xác định phạm vi điền dữ liệu
            int rowStart = 4;
            int columnStart = 1;
            int rowEnd = rowStart + dataTable.Rows.Count - 1;
            int columnEnd = dataTable.Columns.Count;

            // Định dạng cột ngày tháng
            Microsoft.Office.Interop.Excel.Range colNgayLap = oSheet.get_Range("B4", "B" + rowEnd);
            colNgayLap.NumberFormat = "dd/MM/yyyy";

            Microsoft.Office.Interop.Excel.Range colNgayXuat = oSheet.get_Range("C4", "C" + rowEnd);
            colNgayXuat.NumberFormat = "dd/MM/yyyy";

            // Định dạng cột "Tổng tiền" hiển thị dấu phẩy và chữ "đ"
            Microsoft.Office.Interop.Excel.Range colTongTien = oSheet.get_Range("E4", "E" + rowEnd);
            colTongTien.NumberFormat = "#,##0\"đ\"";
            colTongTien.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255, 229, 204));
            colTongTien.ColumnWidth = 15;

            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, columnStart];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnEnd];
            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);

            // Điền dữ liệu
            range.Value2 = arr;
            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            
            // === Thêm hàng tổng doanh thu ===
            int totalRow = rowEnd + 1; // Hàng ngay sau bảng dữ liệu

            // Ghép ô từ A đến D cho tiêu đề "Tổng doanh thu:"
            Microsoft.Office.Interop.Excel.Range totalLabelRange = oSheet.get_Range("A" + totalRow, "D" + totalRow);
            totalLabelRange.MergeCells = true;
            totalLabelRange.Value2 = "Tổng doanh thu:";
            totalLabelRange.Font.Bold = true;
            totalLabelRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            totalLabelRange.Interior.Color = System.Drawing.Color.FromArgb(255, 229, 204);

            // Tính tổng cột "Tổng tiền" (E)
            Microsoft.Office.Interop.Excel.Range totalValueCell = oSheet.get_Range("E" + totalRow, "E" + totalRow);
            totalValueCell.Formula = $"=SUM(E4:E{rowEnd})";
            totalValueCell.NumberFormat = "#,##0\"đ\""; // Hiển thị có dấu phẩy phân tách hàng nghìn + chữ " đồng"
            totalValueCell.Font.Bold = true;
            totalValueCell.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red); // Màu đỏ
            totalValueCell.Interior.Color = System.Drawing.Color.FromArgb(255, 229, 204);
            totalValueCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Định dạng tổng doanh thu
            totalLabelRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            totalValueCell.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

        }


        private void btnExportFile_Click(object sender, EventArgs e)
        {
            // Kiểm tra DataGridView có dữ liệu không trước khi export
            if (dgvDoanhthu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = BillDAO.Instance.Exportfile(dgvDoanhthu);

            if (dt.Rows.Count > 0)
            {
                ExportFileExcel(dt, "Doanh Thu", "BÁO CÁO DOANH THU");
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        #endregion


    }
}
