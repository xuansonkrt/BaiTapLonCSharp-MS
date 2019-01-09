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
    public partial class frmSuaSV : Form
    {
        DataTable tb;
        string oldID;
        public frmSuaSV()
        {
            InitializeComponent();
        }
        public frmSuaSV(DataTable tb)
        {
            InitializeComponent();
            this.tb = tb;
            if (tb.Rows.Count > 0)
            {
                oldID= tb.Rows[0]["MaSV"].ToString();
                txtSMaSV.Text = tb.Rows[0]["MaSV"].ToString();
                txtSHoTen.Text = tb.Rows[0]["HoTen"].ToString();
                //  txtsNgaySinh.Text = tb.Rows[0]["NgaySinh"].ToString();
                DateTime date = Convert.ToDateTime(tb.Rows[0]["NgaySinh"].ToString()).Date;
                txtsNgaySinh.Text = date.ToString("dd/MM/yyyy");
                txtLop.Text= tb.Rows[0]["MaLop"].ToString();
                if (tb.Rows[0]["GioiTinh"].ToString().Equals("Nam") == true)
                {
                    radNam.Checked = true;
                }
                else
                {
                    radNu.Checked = true;
                }
                MemoryStream stream = new MemoryStream((byte[])tb.Rows[0]["Image"]);
                imgSV.Image = new Image<Bgr, byte>(new Bitmap(stream));
            }
        }
        SqlConnection connect = null;
        //   string strconnect = " Server= HUUMANH\\SQLEXPRESS; Database=CSDLDiemDanh2; User Id=Diemdanh; pwd=123";
        string strconnect = " Server= MYPC\\SQLEXPRESS; Database=CSDLDiemDanh; User Id=diemdanh; pwd=123";
        

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmSuaSV_Load(object sender, EventArgs e)
        {

        }

        private void btnXacNhan1_Click(object sender, EventArgs e)
        {

          /*  string s = txtsNgaySinh.Text;
            int s1 = Convert.ToInt16(s.Substring(0, 2));
            int s2 = Convert.ToInt16(s.Substring(3, 2));
            int s3 = Convert.ToInt16(s.Substring(6, 4));*/
            if (connect == null)
                connect = new SqlConnection(strconnect);
            if (connect.State == ConnectionState.Closed)
                connect.Open();
            // update lai thong tin
            string str = " update DH_SV_LCN set MaSV=@ma where MaSV= " + oldID;
            string str2 = " update  DH_SV_LHP set MaSV=@ma where MaSV= " + oldID;
            string str3 = " update  SV_LHP set MaSV=@ma where MaSV= " + oldID;
            string str1 = " update SinhVien set MaSV=@ma,Hoten=@Ten,NgaySinh=@ngay,GioiTinh=@gt where MaSV= " + oldID;

            SqlCommand command = new SqlCommand(str, connect);
            SqlCommand command2 = new SqlCommand(str2, connect);
            SqlCommand command3 = new SqlCommand(str3, connect);
            SqlCommand command1 = new SqlCommand(str1, connect);
            command.Parameters.Add("@ma", SqlDbType.Char).Value = txtSMaSV.Text;
            command1.Parameters.Add("@ma", SqlDbType.Char).Value = txtSMaSV.Text;
            command2.Parameters.Add("@ma", SqlDbType.Char).Value = txtSMaSV.Text;
            command3.Parameters.Add("@ma", SqlDbType.Char).Value = txtSMaSV.Text;

            command1.Parameters.Add("@ten", SqlDbType.NVarChar).Value = txtSHoTen.Text;
            command1.Parameters.Add("@ngay", SqlDbType.Date).Value =Convert.ToDateTime(txtsNgaySinh.Text);//new DateTime(s3, s2, s1);
            command1.Parameters.Add("@gt", SqlDbType.NVarChar).Value = radNam.Checked == true ? "Nam" : "Nữ";

            int ret = command1.ExecuteNonQuery();
            command1.ExecuteNonQuery();
            command2.ExecuteNonQuery();
            command3.ExecuteNonQuery();

            if (ret > 0)
            {

                MessageBox.Show("Sửa thành công.","Thông báo");
            }
            else
            {
                MessageBox.Show("Sửa không thành công.", "Thông báo");
            }
        }

        private void frmSuaSV_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}
