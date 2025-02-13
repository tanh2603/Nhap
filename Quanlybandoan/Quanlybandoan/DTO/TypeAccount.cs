using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Quanlybandoan.DTO
{
    public class TypeAccount
    {
        public TypeAccount(string name, int id)
        {
            this.Name = name;
            this.ID = id;
        }

        public TypeAccount(DataRow row)
        {
            this.Name = row["Tenloaitaikhoan"].ToString();
            this.ID = (int)row["Maloaitk"];
        }

        private string name;
        private int id;

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public int ID
        {
            get { return id; }
            private set { id = value; }
        }
    }
}
