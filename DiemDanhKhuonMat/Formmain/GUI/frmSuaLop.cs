using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formmain.GUI
{
    public partial class frmSuaLop : Form
    {
        DataTable tb;
        int tag;
        string id;
        ConnectDB cn = new ConnectDB();
        SqlConnection connect;
        public frmSuaLop(DataTable tb,int tag)
        {
            InitializeComponent();
            this.tb = tb;
            this.tag = tag;
            connect = cn.getConnect();
            connect.Open();
            txtMaLop.Text = tb.Rows[0]["MaLop"].ToString();
            txtTenLop.Text = tb.Rows[0]["TenLop"].ToString();
            id= tb.Rows[0]["MaLop"].ToString(); 
        }

        private void frmSuaLop_Load(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string sql;
            if (tag==1)
            {
                sql = "update Lop set MaLop=@malop, TenLop =@tenlop where MaLop='"+id+"'";
            }
            else
            {
                sql = "update LopHocPhan set MaLop=@malop, TenLop =@tenlop where MaLop='" + id + "'";
            }
            SqlCommand cmd = new SqlCommand(sql,connect);
            cmd.Parameters.Add("@malop", SqlDbType.NVarChar).Value = txtMaLop.Text;
            cmd.Parameters.Add("@tenlop", SqlDbType.NVarChar).Value = txtTenLop.Text;
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công.", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa không thành công.", "Thông báo");
            }
        }
    }
}
