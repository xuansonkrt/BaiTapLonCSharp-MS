using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formmain.GUI
{
    public partial class frmThemSinhVienLHP : Form
    {
        string text;
        ConnectDB cn = new ConnectDB();
        SqlConnection connect;
        string ID="";
        string sql = "";
        bool IsFind = false;
        public frmThemSinhVienLHP(string text) // text ma lop hoc phan can them sinh vien
        {
            InitializeComponent();
            txtHoTen.ReadOnly = true;
            txtLop.ReadOnly = true;
            txtNgaySinh.ReadOnly = true;
                     

            this.text = text;
            btnXacNhan.Enabled = false;
            ActiveControl = txtMaSV;

            //  add();
        }
        public void add()
        {
            btnXacNhan.Enabled = false;

            connect = cn.getConnect();
            connect.Open();
          //  do
            {
                ID = txtMaSV.Text;
               // if (ID == "")
                   // continue;
                sql = "select * from SinhVien where MaSV='" + ID + "'";
                SqlDataAdapter da = new SqlDataAdapter(sql,connect);
                DataTable tb = new DataTable();
                da.Fill(tb);
                if (tb.Rows.Count > 0)
                {
                    btnXacNhan.Enabled = true;
                    btnXacNhan.Focus();
                    txtHoTen.Text = tb.Rows[0]["HoTen"].ToString();
                    sql = "select TenLop from Lop where MaLop='" + tb.Rows[0]["MaLop"].ToString() + "'";
                    SqlDataAdapter da1 = new SqlDataAdapter(sql, connect);
                    DataTable tb1 = new DataTable();
                    da1.Fill(tb1);
                    txtLop.Text = tb1.Rows[0]["TenLop"].ToString();
                    DateTime date = Convert.ToDateTime(tb.Rows[0]["NgaySinh"].ToString(), new CultureInfo("en-US")).Date;

                    txtNgaySinh.Text = date.ToString("dd/MM/yyyy");
                    string gt = tb.Rows[0]["GioiTinh"].ToString();
                    if (gt.Equals("Nam") == true)
                        radNam.Checked = true;
                    else
                        radNu.Checked = true;
                    MemoryStream stream = new MemoryStream((byte[])tb.Rows[0]["Image"]);
                    imgSV.Image = new Image<Bgr, byte>(new Bitmap(stream));
                   // break;
                    
                }
                else
                {
                    clear();
                  //  break;
                }
            } //while (true);
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        public void clear()
        {
            txtHoTen.Text = "";
            txtLop.Text = "";
          //  txtMaSV.Text = "";
            txtNgaySinh.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;
            imgSV.Image = null;
        }

        private void frmThemSinhVienLHP_FormClosed(object sender, FormClosedEventArgs e)
        {
           // DialogResult = DialogResult.Cancel;
        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {
            add();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            // clear();
            //txtMaSV.Text = "";
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void frmThemSinhVienLHP_Load(object sender, EventArgs e)
        {

        }
    }

}
