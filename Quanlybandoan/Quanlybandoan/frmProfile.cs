using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quanlybandoan.DAO;
using Quanlybandoan.DTO;

namespace Quanlybandoan
{
    public partial class frmProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            private set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public frmProfile(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }
        void ChangeAccount(Account acc)
        {
            txtTendangnhap.Text = LoginAccount.UserName;
            txtTenhienthi.Text = LoginAccount.DisplayName;
        }

        void UpdateAccountInfo()
        {
            string displayName = txtTenhienthi.Text;
            string password = txtMatkhau.Text;
            string newPass = txtMkmoi.Text;
            string reenterpass = txtXacnhanmk.Text;
            string userName = txtTendangnhap.Text;

            if (!newPass.Equals(reenterpass))
            {
                MessageBox.Show("Vui lòng lại mật khẩu đúng với mật khẩu mới!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, password, newPass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if(updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;

        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }
    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc 
        { 
            get { return acc; }
            set { acc = value; } 
        }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
