using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formmain
{
    public partial class FrmThemlop : Form
    {
        public FrmThemlop()
        {
            InitializeComponent();
            cbLoaiHinh.Items.Add("Lớp chuyên ngành");
            cbLoaiHinh.Items.Add("Lớp tín chỉ");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void FrmThemlop_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
