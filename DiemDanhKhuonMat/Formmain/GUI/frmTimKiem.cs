using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formmain.GUI
{
    public partial class frmTimKiem : Form
    {
        string key;
        public frmTimKiem()
        {
            InitializeComponent();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if(txtTimKiem.Text.Trim()=="")
            {
                MessageBox.Show("Bạn chưa nhận giá trị.", "Thông báo");
                ActiveControl = txtTimKiem;
                return;
            }
            key = txtTimKiem.Text.Trim();
            this.Close();
        }
        public string getKey()
        {
            return key;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            key = "";
            this.Close();
        }

        private void frmTimKiem_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
