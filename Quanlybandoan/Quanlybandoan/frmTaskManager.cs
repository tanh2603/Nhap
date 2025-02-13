using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quanlybandoan.DAO;
using Quanlybandoan.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Quanlybandoan
{
    public partial class frmTaskManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount 
        {
            get { return loginAccount; }
            private set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public frmTaskManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboxTable(cbxChuyenban);

        }
        #region Menthod
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinToolStripMenuItem.Text += LoginAccount.DisplayName;
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Click += btn_Click;
                btn.Tag = item;

                // Cập nhật màu sắc và trạng thái bàn
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.SkyBlue;
                        break;
                    default:
                        btn.BackColor = Color.Violet;
                        break;
                }

                btn.Text = item.Name + Environment.NewLine + item.Status;
                flpTable.Controls.Add(btn);
            }
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbxLoai.DataSource = listCategory;
            cbxLoai.DisplayMember = "Name";
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);

            cbxFood.DataSource = listFood;
            cbxFood.DisplayMember = "Name";
        }

        void ShowBill(int id)
        {
            // Xóa tất cả món trong danh sách
            lsvBill.Items.Clear();
            List<Quanlybandoan.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);

            float TotalPrice = 0;

            foreach (Quanlybandoan.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.Sale.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                TotalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");
            txtThanhtien.Text = TotalPrice.ToString("c", culture);
            
        }

        void LoadComboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion


        #region Events
        private void thêmMónĂnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAdd_Click(this, new EventArgs());
        }

        private void chuyểnBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnChuyenban_Click(this, new EventArgs());
        }

        private void gộpBànToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnGopBan_Click(this, new EventArgs());
        }
        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnThanhtoan_Click(this, new EventArgs());
        }

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmProfile f = new frmProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAcccount;
            f.ShowDialog();
        }

        void f_UpdateAcccount(object sender, AccountEvent e)
        {
            thôngTinToolStripMenuItem.Text = e.Acc.DisplayName;
        }

        private void aminToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAmin f = new frmAmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.UpdateFood += f_UpdateFood;
            f.DeleteFood += f_DeleteFood;
            f.InsertCategory += f_InsertCategory;
            f.UpdateCategory += f_UpdateCategory;
            f.DeleteCategory += f_DeleteCategory;
            f.InsertTable += f_InsertTable;
            f.UpdateTable += f_UpdateTable;
            f.DeleteTable += f_DeleteTable;
            f.ShowDialog();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        void f_InsertCategory(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadCategory();
        }

        void f_UpdateCategory(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadCategory();
        }

        void f_DeleteCategory(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadCategory();
            LoadTable();
        }

        void f_InsertTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadCategory();
            LoadTable();
        }

        void f_UpdateTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadCategory();
            LoadTable();
        }

        void f_DeleteTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbxLoai.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadCategory();
            LoadTable();
        }

        private void cbxLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn trước khi thêm món ăn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbxFood.SelectedItem as Food).ID;
            int count = (int)numSoluong.Value;


            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                idBill = BillDAO.Instance.GetMaxIDBill(); // Lấy ID hóa đơn mới sau khi chèn
            }

            if (idBill > 0)
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
                ShowBill(table.ID); // Cập nhật lại danh sách hóa đơn
            }
             LoadTable();
        }

        private void btnThanhtoan_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)numDiscountBill.Value; // % giảm giá
            double totalPrice = double.Parse(txtThanhtien.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("vi-VN"));

            double discountFinal = (totalPrice * discount) / 100; // Số tiền giảm giá chính xác
            double finalTotalPrice = totalPrice - discountFinal;  // Tổng tiền sau khi giảm giá

            // Format số tiền có dấu chấm phân cách hàng nghìn
            string totalPriceStr = totalPrice.ToString("#,##0");
            string discountFinalStr = discountFinal.ToString("#,##0");
            string finalTotalPriceStr = finalTotalPrice.ToString("#,##0");

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format(
                    "Bạn có chắc muốn thanh toán cho bàn {0}\nGiảm giá {1}% = {2} đ\nTổng tiền sau khi trừ giảm giá là {3} đ",
                    table.Name, discount, discountFinalStr, finalTotalPriceStr),
                    "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }

            }
        }

        private void btnChuyenban_Click(object sender, EventArgs e)
        {

            if (cbxChuyenban.SelectedItem == null || lsvBill.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần chuyển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbxChuyenban.SelectedItem as Table).ID;

            if (id1 == id2)
            {
                MessageBox.Show("Bạn không thể chuyển bàn sang chính nó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                    string.Format("Bạn muốn chuyển từ bàn {0} qua bàn {1} không?",
                        (lsvBill.Tag as Table).Name,
                        (cbxChuyenban.SelectedItem as Table).Name),
                    "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
                ShowBill(id2); // Hiển thị hóa đơn của bàn mới
            }

        }

        private void btnGopBan_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn bàn và bàn đang có hóa đơn hay chưa
            if (cbxChuyenban.SelectedItem == null || lsvBill.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần gộp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id1 = (lsvBill.Tag as Table).ID; // Bàn hiện tại
            int id2 = (cbxChuyenban.SelectedItem as Table).ID; // Bàn được chọn để gộp

            if (id1 == id2)
            {
                MessageBox.Show("Bạn không thể gộp bàn với chính nó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                    string.Format("Bạn muốn gộp bàn {0} và bàn {1} không?",
                        (lsvBill.Tag as Table).Name,
                        (cbxChuyenban.SelectedItem as Table).Name),
                    "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // Gọi hàm gộp bàn
                TableDAO.Instance.GopTable(id1, id2);

                // Cập nhật giao diện lại với các bàn
                LoadTable();
                ShowBill(id1); // Hiển thị hóa đơn của bàn sau khi gộp
            }

        }

        private void btnAdd_MouseMove(object sender, MouseEventArgs e)
        {
            btnAdd.BackColor = Color.MistyRose; // Màu khi di chuột vào
        }

        private void btnAdd_MouseLeave(object sender, EventArgs e)
        {
            btnAdd.BackColor = Color.Snow; // Màu khi rời chuột ra
        }

        private void btnChuyenban_MouseLeave(object sender, EventArgs e)
        {
            btnChuyenban.BackColor = Color.PowderBlue;
        }

        private void btnChuyenban_MouseMove(object sender, MouseEventArgs e)
        {
            btnChuyenban.BackColor = Color.SkyBlue;
        }

        private void btnGopBan_MouseMove(object sender, MouseEventArgs e)
        {
            btnGopBan.BackColor = Color.Green;
            btnGopBan.ForeColor = Color.White;
        }

        private void btnGopBan_MouseLeave(object sender, EventArgs e)
        {
            btnGopBan.BackColor = Color.DarkSeaGreen;
            btnGopBan.ForeColor = Color.Black;
        }

        private void btnThanhtoan_MouseMove(object sender, MouseEventArgs e)
        {
            btnThanhtoan.BackColor = Color.BlueViolet;
        }

        private void btnThanhtoan_MouseLeave(object sender, EventArgs e)
        {
            btnThanhtoan.BackColor = Color.MediumPurple;
        }
        #endregion



    }
}
