using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Formmain.GUI;
using Emgu.CV;
using System.IO;
using System.Globalization;

namespace Formmain
{
    public partial class Form1 : Form
    {
        ConnectDB cn = new ConnectDB();
        SqlConnection connect;// = cn.getConnect();
        DataSet dataSet = new DataSet();
        DataTable dataTable;
        SqlDataAdapter dataAdapter;
        SqlCommand sqlCommand;
        string sql;
        public Form1()
        {
            InitializeComponent();
            connect = cn.getConnect();
         
        }


        DataTable TruyVan(string sql, SqlConnection con)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.Fill(dt);
            return dt;
        }

        void showTreeView()
        {
            TvDanhSachLop.Nodes.Clear();
            SqlConnection con = cn.getConnect();
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DataTable nodeBase = new DataTable();
            DataTable nodeSub = new DataTable();
            nodeBase = TruyVan("SELECT * FROM LoaiHinhLop", con);
            for (int i = 0; i < nodeBase.Rows.Count; i++)
            {
                TvDanhSachLop.Nodes.Add(nodeBase.Rows[i]["TenLoaiHinhLop"].ToString());
                // TvDanhSachLop.Nodes[i].Tag = "1";
                nodeSub = TruyVan("SELECT * FROM Lop WHERE MaLoaiHinhLop =N'" + nodeBase.Rows[i][0] + "'", con);
                for (int j = 0; j < nodeSub.Rows.Count; j++)
                {
                    TvDanhSachLop.Nodes[i].Nodes.Add(nodeSub.Rows[j]["TenLop"].ToString());
                    TvDanhSachLop.Nodes[i].Nodes[j].Tag = "1";
                }
                nodeSub = TruyVan("SELECT * FROM LopHocPhan WHERE MaLoaiHinhLop =N'" + nodeBase.Rows[i][0] + "'", con);
                for (int j = 0; j < nodeSub.Rows.Count; j++)
                {
                    TvDanhSachLop.Nodes[i].Nodes.Add(nodeSub.Rows[j]["TenLop"].ToString());
                    TvDanhSachLop.Nodes[i].Nodes[j].Tag = "2";
                }

            }
            con.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            showListSV();
        }
        private void showListSV()
        {
            //  dataGridView1.DataSource = null;
            TreeNode theNode = this.TvDanhSachLop.SelectedNode;
            if (theNode.Tag == "2")
            {                
                dataGridView1.DataSource = getTable(theNode.Text, 2); //cn.getTable(sql);
                color();
                convertDate();
            }
            else
            {
                if (theNode.Tag == "1")
                {                   
                    dataGridView1.DataSource = getTable(theNode.Text,1);//cn.getTable(sql);
                    color();
                    convertDate();

                }
                else
                {
                    dataGridView1.DataSource = null;
                }
            }
        }

        private void thêmMơiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thêmLớpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            FrmThemlop frm = new FrmThemlop();
            lap: frm.ShowDialog();
            SqlConnection connect = cn.getConnect();
            connect.Open();
            if (frm.DialogResult == DialogResult.OK)
            {
                int index = frm.cbLoaiHinh.SelectedIndex;
                string str1, str2;
                if (index == 0)
                {
                    str1 = "Lop";
                    str2 = "lcn";
                }
                else
                {
                    str1 = "LopHocPhan";
                    str2 = "ltc";
                }

                string sql = "insert into " + str1 + " values ( N'" + frm.txtMaLop.Text + "',N'" + frm.txtTenLop.Text + "','" + str2 + "')";
                SqlCommand cmd = new SqlCommand(sql, connect);
                try
                {
                    cmd.ExecuteNonQuery();
                    showTreeView();
                    frm.Dispose();
                    connect.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lớp đã tồn tại.");
                    goto lap;
                }
            }




        }


        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripContainer1_BottomToolStripPanel_Click(object sender, EventArgs e)
        {

        }
      
        private void Form1_Load(object sender, EventArgs e)
        {
            showTreeView();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TvDanhSachLop_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //   TvDanhSachLop.SelectedNode = e.Node;
        }

        private void thêmDanhSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thêmLớpToolStripMenuItem_Click(sender, e);
        }

        private void thêmSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {


            connect = cn.getConnect();
            connect.Open();
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.Connection = connect;
            string tmp = "";
            TreeNode theNode = this.TvDanhSachLop.SelectedNode;

            if (theNode.Tag == "1")
            {
                string str = "select MaLop,TenLop from Lop where TenLop= N'" + theNode.Text + "'";
                DataTable tb = cn.getTable(str);
                // frm.txtLop.Text = tb.Rows[0]["TenLop"].ToString();
                // frm.txtLop.Enabled = false;

               // string text = tb.Rows[0]["TenLop"].ToString();
                do
                {
                    connect = cn.getConnect();
                    connect.Open();
                    frmThemSinhVien frm = new frmThemSinhVien(theNode.Text);
                    frm.Text = "Thêm sinh viên vào lớp " + theNode.Text;

                    frm.ShowDialog();

                    if (frm.DialogResult == DialogResult.OK)
                    {
                        sql = " insert into SinhVien values(@ma,@hoten,@malop,@ngay,@gt,@image,@nghi)";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connect;
                        cmd.CommandText = sql;

                        MemoryStream stream = new MemoryStream();
                        int width1 = Convert.ToInt32(frm.imgTrain.Width);
                        int height1 = Convert.ToInt32(frm.imgTrain.Height);
                        int width = 148;
                        int height = 161;
                        Bitmap bmp = new Bitmap(width, height);
                        frm.imgTrain.DrawToBitmap(bmp, new Rectangle(0, 0, Width, Height));
                        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] pic = stream.ToArray();

                        cmd.Parameters.Add("@ma", SqlDbType.Char).Value = frm.txtMaSV.Text;
                        cmd.Parameters.Add("@hoten", SqlDbType.NVarChar).Value = frm.txtHoTen.Text;
                        //   cmd.Parameters.Add("@ngay ", SqlDbType.Date).Value = Convert.ToDateTime(frm.txtNgaySinh.Text, new CultureInfo("vi-VN")).Date;
                        cmd.Parameters.Add("@ngay ", SqlDbType.Date).Value = frm.getDate();

                        cmd.Parameters.Add("@gt", SqlDbType.NVarChar).Value = frm.radNam.Checked == true ? "Nam" : "Nữ";
                        cmd.Parameters.Add("@malop", SqlDbType.NVarChar).Value = tb.Rows[0]["MaLop"];
                        cmd.Parameters.Add("@image", SqlDbType.Image).Value = pic;
                        cmd.Parameters.Add("@nghi", SqlDbType.Int).Value = 0;
                        int ret = -1;
                        try
                        {
                            ret = cmd.ExecuteNonQuery();
                            dataGridView1.DataSource = null;
                            // kiểm tra lớp đã điểm danh buổi nào hay chưa
                            // nếu chưa điểm danh=> chỉ thêm vào bảng SinhVien
                            // nếu lớp đã điểm danh => thêm vào bảng SinhVien và bảng DH_SV_LCN
                            // chèn vào table DH_SV_LCN
                            sql = "select* from DH_SV_LCN where MaLop='" + tb.Rows[0]["MaLop"]+"' and MaSV ='"+ frm.txtMaSV.Text+"'";
                            SqlDataAdapter da = new SqlDataAdapter(sql, connect);
                            DataTable tb1 = new DataTable();
                            da.Fill(tb1);
                           
                            // if (tb1.Rows.Count==0)
                            int days = MaxDate(tb.Rows[0]["MaLop"].ToString(), 1);
                           if (days>0)
                            {
                                sql = "insert into DH_SV_LCN values(@malop,@masv,@ngay,@nghi)";
                                SqlCommand cmd2 = new SqlCommand();
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Connection = connect;                  
                                cmd2.CommandText = sql;
                                cmd2.Parameters.Add("@malop", SqlDbType.Char).Value = tb.Rows[0]["MaLop"];
                                cmd2.Parameters.Add("@masv", SqlDbType.Char).Value = frm.txtMaSV.Text;
                                cmd2.Parameters.Add("@ngay", SqlDbType.Date).Value = DateTime.Now;
                                cmd2.Parameters.Add("@nghi", SqlDbType.Int).Value = days;
                                try
                                {
                                    cmd2.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            showListSV();
                            frm.clear();

                        }
                        catch (Exception ex)
                        {
                            //  MessageBox.Show(ex.Message);
                              MessageBox.Show("Sinh viên đã tồn tại!");
                        }
                    }
                    
                    if (frm.DialogResult == DialogResult.Cancel)
                        break;
                } while (true);
            }
            else
                if (theNode.Tag == "2")
            {
                connect = cn.getConnect();
                connect.Open();
                string sql = "select MaLop,TenLop from LopHocPhan where TenLop= N'" + theNode.Text + "'";
                DataTable tb = new DataTable();// cn.getTable(str);
                SqlDataAdapter da = new SqlDataAdapter(sql, connect);
                da.Fill(tb);
                string text = tb.Rows[0]["MaLop"].ToString();
                do
                {
                    connect = cn.getConnect();
                    connect.Open();
                    frmThemSinhVienLHP frm = new frmThemSinhVienLHP(text); // truyen vao ma lop
                    frm.Text="Thêm sinh viên vào lớp "+tb.Rows[0]["TenLop"].ToString();
                    frm.ShowDialog();
                    // frm.add();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        //nhap vap lop
                        sql = " insert into SV_LHP values(@malop,@masv)";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connect;
                        cmd.CommandText = sql;
                        cmd.CommandText = sql;
                        cmd.Parameters.Add("@malop", SqlDbType.Char).Value = tb.Rows[0]["MaLop"].ToString();
                        cmd.Parameters.Add("@masv", SqlDbType.Char).Value = frm.txtMaSV.Text;
                        
                  
                        try
                        {
                            cmd.ExecuteNonQuery();
                            dataGridView1.DataSource = null;
                            // nhap vao table diem danh
                            sql = "select* from DH_SV_LHP where MaLop='" + tb.Rows[0]["MaLop"] + "' and MaSV ='" + frm.txtMaSV.Text + "'";
                            SqlDataAdapter da1 = new SqlDataAdapter(sql, connect);
                            DataTable tb1 = new DataTable();
                            da1.Fill(tb1);
                            int days = MaxDate(tb.Rows[0]["MaLop"].ToString(), 2);
                            if (days > 0)
                            {
                                SqlCommand cmd2 = new SqlCommand();
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Connection = connect;
                                sql = "insert into DH_SV_LHP values(@malop,@masv,@ngay,@nghi)";
                                cmd2.CommandText = sql;
                                cmd2.Parameters.Add("@malop", SqlDbType.Char).Value = tb.Rows[0]["MaLop"];
                                cmd2.Parameters.Add("@masv", SqlDbType.Char).Value = frm.txtMaSV.Text;
                                cmd2.Parameters.Add("@ngay", SqlDbType.Date).Value = DateTime.Now;
                                cmd2.Parameters.Add("@nghi", SqlDbType.Int).Value = days;
                                try
                                {
                                    cmd2.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }

                            showListSV();
                            frm.clear();
                            frm.txtMaSV.Text = "";
                        }
                        catch (Exception ex)
                        {
                          //    MessageBox.Show(ex.Message);
                           MessageBox.Show("Sinh viên đã tồn tại!");
                        }
                    }
                    if (frm.DialogResult == DialogResult.Cancel)
                        break;
                } while (true);
                connect.Close();
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn lớp cần thêm sinh viên!", "Thông báo");
                return;
            }
        }

        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {// update lại bang
            SqlDataAdapter adapter = null;
            adapter = new SqlDataAdapter("Select * from SinhVien", connect);
            DataSet ds = new DataSet();
            SqlCommandBuilder builer = new SqlCommandBuilder(adapter);
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void điểmDanhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connect = cn.getConnect();
            connect.Open();
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.Connection = connect;
            string text = "", sql = "";
            TreeNode theNode = this.TvDanhSachLop.SelectedNode;
            int tag = 0;
            if (theNode.Tag == "1")
            {
                sql = "select MaLop from Lop where TenLop= N'" + theNode.Text + "'";
                tag = 1;
            }
            else
                if (theNode.Tag == "2")
            {
                sql = "select MaLop from LopHocPhan where TenLop= N'" + theNode.Text + "'";
                tag = 2;
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn lớp cần điểm danh!", "Thông báo");
                return;
            }
            //string str = "select  from SinhVien sv, " + tmp + " lop where sv.MaLop = TenLop= N'" + theNode.Text + "'";
            SqlDataAdapter da = new SqlDataAdapter(sql, connect);
            DataTable tb = new DataTable();// cn.getTable(sql);
            da.Fill(tb);
            text = tb.Rows[0][0].ToString();
            connect.Close();
            frmDiemDanh frm = new frmDiemDanh(text,tag); // text ma lop
            frm.ShowDialog();
            dataGridView1.DataSource = getTable(theNode.Text, Convert.ToInt16(theNode.Tag));//cn.getTable(sql);
            color();
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void xóaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            

        }
        public DataTable getTable(string TenLop, int tag) //malop 
        {
            DataTable tb = new DataTable();
            connect = cn.getConnect();
            connect.Open();
            string sql = "";
            int fisrt = 0;
            if(tag==1)
            {
                sql = "select * from DH_SV_LCN r1, Lop l where r1.MaLop =l.MaLop and l.TenLop=N'" + TenLop + "'";
                SqlDataAdapter da1 = new SqlDataAdapter(sql,connect);
                DataTable tb1 = new DataTable();
                da1.Fill(tb1);
                if (tb1.Rows.Count > 0)
                    sql = " select sv.MaSV N'Mã sinh viên',HoTen N'Họ tên', TenLop N'Lớp',NgaySinh N'Ngày sinh',GioiTinh N'Giới tính',r1.SoNgayNghi N'Số ngày nghỉ'"
                            + " from SinhVien sv,Lop l, ("
                             + " select MaSV,r.MaLop, sum(r.Nghi) as 'SoNgayNghi'"
                             + " from DH_SV_LCN r, Lop l "
                             + " where r.MaLop=l.MaLop and l.TenLop=N'" + TenLop + "'"
                              + " group by MaSV,r.MaLop"
                            + " ) as r1"
                            + " where sv.MaSV=r1.MaSV and l.MaLop=sv.MaLop and l.MaLop=r1.MaLop";
                else
                {
                    sql = "select sv.MaSV N'Mã sinh viên',HoTen N'Họ tên', TenLop N'Lớp',NgaySinh N'Ngày sinh',GioiTinh N'Giới tính'"
                           + " from SinhVien sv,Lop l"
                            + " where sv.MaLop=l.MaLop and l.TenLop=N'" + TenLop + "'";
                    fisrt = 1;
                }
            }
            else
            {
                sql = "select * from DH_SV_LHP r1, LopHocPhan l where r1.MaLop =l.MaLop and l.TenLop=N'" + TenLop + "'";
                SqlDataAdapter da1 = new SqlDataAdapter(sql, connect);
                DataTable tb1 = new DataTable();
                da1.Fill(tb1);
                if(tb1.Rows.Count>0)
                    sql = " select sv.MaSV N'Mã sinh viên',HoTen N'Họ tên', l.TenLop N'Lớp',NgaySinh N'Ngày sinh',GioiTinh N'Giới tính',r1.SoNgayNghi N'Số ngày nghỉ'"
                           + " from SinhVien sv,Lop l, LopHocPhan lhp,("
                               + " select MaSV, r.MaLop, sum(r.Nghi) as 'SoNgayNghi'"
                               + " from DH_SV_LHP r, LopHocPhan l"
                               + " where r.MaLop = l.MaLop and l.TenLop = N'" + TenLop + "'"
                              + "  group by MaSV, r.MaLop "
                           + " ) as r1 "
                         + "   where r1.MaSV=sv.MaSV  and l.MaLop=sv.MaLop and lhp.MaLop=r1.MaLop";
                else
                {
                    fisrt = 1;
                    sql = "select sv.MaSV N'Mã sinh viên',HoTen N'Họ tên', l.TenLop N'Lớp',NgaySinh N'Ngày sinh',GioiTinh N'Giới tính'"
                           + " from SinhVien sv,Lop l,LopHocPhan lhp, SV_LHP r1"
                            + " where sv.MaLop=l.MaLop and sv.MaSV=r1.MaSV and lhp.MaLop=r1.MaLop and lhp.TenLop=N'" + TenLop + "'";
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(sql, connect);
            da.Fill(tb);
            if (fisrt == 1)
            {
                tb.Columns.Add("Số ngày nghỉ",typeof(System.Int16));
                foreach (DataRow row in tb.Rows)
                {
                    row["Số ngày nghỉ"] = 0;
                    fisrt = 0;
                }
                
            }
            connect.Close();
            return tb;
        }
        private void color()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                /*if (Convert.ToInt32(row.Cells[5].Value) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }*/
                if (Convert.ToInt32(row.Cells[5].Value) == 1)
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
                if (Convert.ToInt32(row.Cells[5].Value) == 2)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
                if (Convert.ToInt32(row.Cells[5].Value) == 3)
                {
                    row.DefaultCellStyle.BackColor = Color.Orange;
                }
                if (Convert.ToInt32(row.Cells[5].Value) > 3)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }
                
        }
        private int MaxDate(string MaLop, int tag)
        {
            int count = 0;
            string sql = "";
            if (tag == 1)
                sql = "select top 1 MaLop,MaSV, count(Ngay)"
                     + " from DH_SV_LCN"
                     + " where MaLop =N'" + MaLop + "'"
                      + " group by MaLop,MaSV "
                      +" order by count(Ngay)";
            else
                sql = "select top 1 MaLop,MaSV, count(Ngay)"
                    + " from DH_SV_LHP"
                    + " where MaLop =N'" + MaLop + "'"
                     + " group by MaLop,MaSV "
                     + " order by count(Ngay)";
            SqlDataAdapter da = new SqlDataAdapter(sql,connect);
            DataTable tb = new DataTable();
            da.Fill(tb);
            if(tb.Rows.Count>0)
                count =Convert.ToInt16( tb.Rows[0][2].ToString());
            return count;
        }
        private void thêmSinhViênToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            thêmSinhViênToolStripMenuItem_Click(sender, e);

        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        private void thuGọnTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TvDanhSachLop.SelectedNode != null)
            {
                TvDanhSachLop.SelectedNode.Collapse();
            }
        }

        private void mởRộngGrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TvDanhSachLop.SelectedNode != null)
            {
                TvDanhSachLop.SelectedNode.ExpandAll();
            }
        }
        private void HienThiChiTietSV(frmSuaSV frm)
        {
            frm = new frmSuaSV();
            frm.Show();
            connect.Close();
            connect.Open();
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            string str = " select * from SinhVien where MaSV = " + dataGridView1.Tag;
            command.CommandText = str;
            command.Connection = connect;
            SqlDataReader reader = command.ExecuteReader();
            // hien thi thong tin sv can su len form 
            /*if (reader.Read() == true)
            {
                frm.txtMacu.Text = reader.GetString(0);
                frm.txtSHoTen.Text = reader.GetString(1);
                frm.txtsNgaySinh.Text = reader.GetDateTime(3).ToString("dd/MM/yyyy") + "";
                if (reader.GetString(4) == " Nam ")
                {
                    frm.radNam.Checked = true;
                }
                else
                {
                    frm.radNu.Checked = true;
                }
                connect.Close();
            }*/
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connect.Close();
            connect.Open();
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            string str = " select * from SinhVien where MaSV = " + dataGridView1.Tag;
            command.CommandText = str;
            command.Connection = connect;
            SqlDataAdapter da = new SqlDataAdapter(str, connect);
            DataTable tb = new DataTable();
            da.Fill(tb);
            // SqlDataReader reader = command.ExecuteReader();
            frmSuaSV frm = new frmSuaSV(tb);
            frm.ShowDialog();
            //HienThiChiTietSV(frm);
            TreeNode theNode = TvDanhSachLop.SelectedNode;
            dataGridView1.DataSource = getTable(theNode.Text, Convert.ToInt16(theNode.Tag));//cn.getTable(sql);
            color();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];// lấy ra dòng dk chọn 
            int ma = Convert.ToInt32(row.Cells[0].Value + "");
            dataGridView1.Tag = ma;
        }

        private void xóaLớpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode theNode = this.TvDanhSachLop.SelectedNode;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /* if (theNode.Tag == "1" || theNode.Tag == "2")
                 {
                     string strLoai;
                     if (theNode.Tag == "1")
                         strLoai = "Lop";
                     else
                         strLoai = "LopHocPhan";
                     string sql = "delete from " + strLoai + " where TenLop= N'" + theNode.Text + "'";

                     connect.Open();
                     SqlCommand cmd = new SqlCommand(sql, connect);
                     try
                     {
                         cmd.ExecuteNonQuery();
                         showTreeView();
                         connect.Close();
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show(ex.Message);
                     }

                 }*/
                if (theNode.Tag == "1")
                {
                    if (connect.State == ConnectionState.Closed)
                        connect.Open();
                    string sql = "select MaLop from Lop l where TenLop=N'" + theNode.Text.ToString() + "'";
                    SqlDataAdapter da = new SqlDataAdapter(sql, connect);
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    string str = " delete from DH_SV_LCN  where MaLop = '" + tb.Rows[0]["MaLop"] + "'";
                    string str3 = " delete from Lop where MaLop = '" + tb.Rows[0]["MaLop"] + "'";
                    string str2 = " delete from SinhVien where MaLop = '" + tb.Rows[0]["MaLop"] + "'";
                    string str1 = " delete from DH_SV_LHP where MaLop = '" + tb.Rows[0]["MaLop"] + "'";

                    SqlCommand command = new SqlCommand(str, connect);
                    command.ExecuteNonQuery();
                    SqlCommand command1 = new SqlCommand(str1, connect);
                    command1.ExecuteNonQuery();
                    SqlCommand command2 = new SqlCommand(str2, connect);
                    command2.ExecuteNonQuery();
                    SqlCommand command3 = new SqlCommand(str3, connect);
                    command3.ExecuteNonQuery();
                    showListSV();
                    showTreeView();
                    connect.Close();
                }
                else
                {
                    if (connect.State == ConnectionState.Closed)
                        connect.Open();
                    string sql = "select MaLop from LopHocPhan where TenLop=N'" + theNode.Text.ToString() + "'";
                    SqlDataAdapter da = new SqlDataAdapter(sql, connect);
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                    // string str = " delete from DH_SV_LCN  where MaSV = " + dataGridView1.Tag;
                    string str2 = " delete from DH_SV_LHP  where MaLop = '" + tb.Rows[0]["MaLop"] + "'";
                    string str1 = " delete from SV_LHP  where MaLop = '" + tb.Rows[0]["MaLop"] + "'";
                    string str3 = " delete from LopHocPhan where MaLop = '" + tb.Rows[0]["MaLop"] + "'";
                    //  SqlCommand command = new SqlCommand(str, connect);
                    // command.ExecuteNonQuery();
                    // SqlCommand command1 = new SqlCommand(str1, connect);
                    // command1.ExecuteNonQuery();
                    SqlCommand command1 = new SqlCommand(str1, connect);
                    command1.ExecuteNonQuery();
                    SqlCommand command2 = new SqlCommand(str2, connect);
                    command2.ExecuteNonQuery();
                    SqlCommand command3 = new SqlCommand(str3, connect);
                    command3.ExecuteNonQuery();
                    showListSV();
                    showTreeView();
                    connect.Close();
                }
            }
        }
        private void convertDate()
        {
            DateTime date;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                /*if (Convert.ToInt32(row.Cells[5].Value) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }*/




                //date = Convert.ToDateTime(row.Cells[3].Value.ToString(), new CultureInfo("vi-VN")).Date;
               // row.Cells[3].Value = date.ToString();
            }
        }
        private void xóaSinhViênToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode theNode = TvDanhSachLop.SelectedNode;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa ?", " Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(theNode.Tag=="1")
                {
                    if (connect.State == ConnectionState.Closed)
                        connect.Open();

                    string str = " delete from DH_SV_LCN  where MaSV = " + dataGridView1.Tag;
                    string str2 = " delete from DH_SV_LHP  where MaSV = " + dataGridView1.Tag;
                    string str1 = " delete from SinhVien  where MaSV = " + dataGridView1.Tag;
                    string str3 = " delete from SV_LHP where MaSv = " + dataGridView1.Tag;
                    SqlCommand command = new SqlCommand(str, connect);
                    command.ExecuteNonQuery();
                    SqlCommand command1 = new SqlCommand(str1, connect);
                    command1.ExecuteNonQuery();
                    SqlCommand command2 = new SqlCommand(str2, connect);
                    command2.ExecuteNonQuery();
                    SqlCommand command3 = new SqlCommand(str3, connect);
                    command3.ExecuteNonQuery();
                    showListSV();
                    connect.Close();
                } else
                {
                    if (connect.State == ConnectionState.Closed)
                        connect.Open();
                    string sql = "select MaLop from LopHocPhan where TenLop=N'" + theNode.Text.ToString() + "'";
                    SqlDataAdapter da = new SqlDataAdapter(sql,connect);
                    DataTable tb = new DataTable();
                    da.Fill(tb);
                   // string str = " delete from DH_SV_LCN  where MaSV = " + dataGridView1.Tag;
                    string str2 = " delete from DH_SV_LHP  where MaSV = " + dataGridView1.Tag+" and MaLop='"+tb.Rows[0]["MaLop"]+"'";
                   // string str1 = " delete from SinhVien  where MaSV = " + dataGridView1.Tag;
                    string str3 = " delete from SV_LHP where MaSv = " + dataGridView1.Tag + " and MaLop='" + tb.Rows[0]["MaLop"] + "'";
                    //  SqlCommand command = new SqlCommand(str, connect);
                    // command.ExecuteNonQuery();
                    // SqlCommand command1 = new SqlCommand(str1, connect);
                    // command1.ExecuteNonQuery();
                    SqlCommand command2 = new SqlCommand(str2, connect);
                    command2.ExecuteNonQuery();
                    SqlCommand command3 = new SqlCommand(str3, connect);
                    command3.ExecuteNonQuery();
                    showListSV();
                    connect.Close();
                }
            }
        }

        private void xóaLớpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xóaLớpToolStripMenuItem1_Click(sender, e);
        }

        private void xóaSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xóaSinhViênToolStripMenuItem1_Click(sender, e);
        }

        private void sửaLớpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem_Click(sender, e);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode theNode = TvDanhSachLop.SelectedNode;
            string sql;
            if(theNode.Tag=="1")
                sql = "select MaLop,TenLop from Lop where TenLop=N'" + theNode.Text.ToString() + "'";
            else
                sql = "select MaLop,TenLop from LopHocPhan where TenLop=N'" + theNode.Text.ToString() + "'";

            SqlDataAdapter da = new SqlDataAdapter(sql, connect);
            DataTable tb = new DataTable();
            da.Fill(tb);
            frmSuaLop frm = new frmSuaLop(tb,Convert.ToInt16(theNode.Tag));
            frm.ShowDialog();
        }

        private void sửaThôngTinSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redoToolStripMenuItem_Click(sender, e);
        }

        private void tsmLucDSCamThi_Click(object sender, EventArgs e)
        {
            TreeNode theNode = TvDanhSachLop.SelectedNode;           
            if (theNode.Tag == "1")
            {
                dataGridView1.DataSource = getTable(theNode.Text, 1, false);
            }
            else
            {
                dataGridView1.DataSource = getTable(theNode.Text, 2, false);
            }
            color();
        }

        private void tsmLocDSThi_Click(object sender, EventArgs e)
        {
            TreeNode theNode = TvDanhSachLop.SelectedNode;
            if (theNode.Tag == "1")
            {
                 dataGridView1.DataSource = getTable(theNode.Text, 1, true);
            }
            else
            {
                 dataGridView1.DataSource = getTable(theNode.Text, 2, true);
            }
            color();

        }
        private DataTable getTable (string TenLop, int tag, bool thi )
        {
            string sql;
            string table1,table2,Compare;
            
            if (thi == true) 
                Compare = "<="; //được thi
            else
                Compare = ">"; //cấm thi
            if (tag == 1)
            {
                
                sql = " select sv.MaSV N'Mã sinh viên',HoTen N'Họ tên', l.TenLop N'Lớp',NgaySinh N'Ngày sinh',GioiTinh N'Giới tính',r.Nghi N'Số ngày nghỉ'"
                    + " from SinhVien sv, Lop l, ("
                    + " select MaSV,r1.MaLop,sum(Nghi) as 'Nghi'"
                    + " from DH_SV_LCN r1, Lop l where r1.MaLop = l.MaLop and l.TenLop = N'" + TenLop + "'"
                    + " group by MaSV,r1.MaLop"
                    + " having sum(Nghi)" + Compare + "3)  as r"
                    + " where sv.MaSV=r.MaSV and l.MaLop=r.MaLop";
            }
            else
            {
                  sql = " select sv.MaSV N'Mã sinh viên',HoTen N'Họ tên', l.TenLop N'Lớp',NgaySinh N'Ngày sinh',GioiTinh N'Giới tính',r.Nghi N'Số ngày nghỉ'"
                    + " from Lop l, SinhVien sv,  ("
                    + " select MaSV,r1.MaLop,sum(Nghi) as 'Nghi'"
                    + " from DH_SV_LHP r1, LopHocPhan l where r1.MaLop = l.MaLop and l.TenLop = N'" + TenLop + "'"
                    + " group by MaSV,r1.MaLop"
                    + " having sum(Nghi)" + Compare + "3)  as r"
                    + " where l.MaLop=sv.MaLop and sv.MaSV=r.MaSV ";
            }
            
            DataTable tb = new DataTable();
            if (connect == null)
            {
                connect = cn.getConnect();
                connect.Open();
            }

            SqlDataAdapter da = new SqlDataAdapter(sql,connect);
            try
            {
                da.Fill(tb);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connect.Close();
            return tb;
        }

        private void mnuTK_MSV_Click(object sender, EventArgs e)
        {
            frmTimKiem frm = new frmTimKiem();
            frm.Text = "Tìm kiếm Mã sinh viên";
            frm.labelTimKiem.Text = "Mã sinh viên";
            frm.ShowDialog();

            string key = frm.getKey();
            if (key == "")
                return;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ( key.Equals(Convert.ToString(row.Cells[0].Value).Trim())==true)
                {
                    clearColor();
                    row.DefaultCellStyle.BackColor = Color.Cyan;
                    //row.Selected = true;
                    return;
                }
            }
            MessageBox.Show("Không tìm thấy Mã sinh viên " + key,"Thông báo");
        }

        private void mnuTK_HoTen_Click(object sender, EventArgs e)
        {
            bool find = false;
            frmTimKiem frm = new frmTimKiem();
            frm.Text = "Tìm kiếm Họ tên";
            frm.labelTimKiem.Text = "Họ tên";
            frm.ShowDialog();
            string key = frm.getKey();
            if (key == "")
                return;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (key.Equals(Convert.ToString(row.Cells[1].Value).Trim()) == true)
                {
                    clearColor();
                    row.Selected = true;
                }
            }
            if(!find)
                MessageBox.Show("Không tìm thấy Họ tên " + key, "Thông báo");
        }
        private void mnuTK_Lop_Click(object sender, EventArgs e)
        {
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            bool find = false;
            frmTimKiem frm = new frmTimKiem();
            frm.Text = "Tìm kiếm Lớp";
            frm.labelTimKiem.Text = "Lớp";
            frm.ShowDialog();
            string key = frm.getKey();
            if (key == "")
                return;
            clearColor();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (key.Equals(Convert.ToString(row.Cells[2].Value).Trim()) == true)
                {
                    row.Selected = true;
                    find = true;
                }
            }
            if (!find)
                MessageBox.Show("Không tìm thấy Lớp " + key, "Thông báo");
        }
        private void mnuTK_NgaySinh_Click(object sender, EventArgs e)
        {
            
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            bool find = false;
            frmTimKiem frm = new frmTimKiem();
            frm.Text = "Tìm kiếm Ngày sinh";
            frm.labelTimKiem.Text = "Ngày sinh";
            frm.ShowDialog();
            string key = frm.getKey();
            if (key == "")
                return;
            clearColor();
            DateTime date = Convert.ToDateTime(key).Date;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                
                if (date.CompareTo(row.Cells[3].Value)==0)
                {
                    row.Selected = true;
                    find = true;
                }
            }
            if (!find)
                MessageBox.Show("Không tìm thấy Ngày sinh " + key, "Thông báo");
        }

        
        private void clearColor()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

    }
}
