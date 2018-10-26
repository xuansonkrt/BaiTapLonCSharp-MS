using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.SqlServer.Server;
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
    public partial class frmDiemDanh : Form
    {
        private Capture capture;         // camera input
        private bool captureInProcess = false;
        private bool picProcess = false;
        private HaarCascade haar; // detector faces
        private Image<Gray, Byte> objFace;
        Image<Bgr, Byte> ImageFrame;
        Image<Gray, Byte> img;
        int tag;
        //List<SinhVien> listSV = new List<SinhVien>();
        List<Image<Gray, byte>> listImg = new List<Image<Gray, byte>>();
        List<string> listID = new List<string>();
        Image<Bgr, Byte>[] EXFace;
        int FaceNum = 0;
        int stt = 1;
        DsDevice[] multiCam;
        string MaLop;
        DateTime date;
        DateTime today = DateTime.Now.Date;
        SqlConnection connect;
        ConnectDB cn = new ConnectDB();
        SqlCommand command= new SqlCommand();
        string sql;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5d, 0.5d);
        SqlDataAdapter da;
        DataTable tb;
        public frmDiemDanh(string MaLop, int tag) //text ma lop
        {
            InitializeComponent();
            this.MaLop = MaLop;
            this.tag = tag;
            command.CommandType = CommandType.Text;
            command.Connection = connect;
            multiCam = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            string name = "";
            int i = 1;
            foreach (DsDevice cam in multiCam)
            {
                name = i + ":" + cam.Name;
                cbCamIndex.Items.Add(name);
            }
            //  MemoryStream stream = new MemoryStream((byte[])ds.Tables[0].Rows[0]["Image"]);
            // pictureBox1.Image = new Bitmap(stream);
            //img = new Image<Bgr, byte>(new Bitmap(stream));
            // imageBox1.Image = img;
            if (tag == 1)
                sql = "Select MaSV,Image from SinhVien sv, Lop l where sv.MaLop = l.MaLop and l.MaLop=N'" + MaLop + "'";
            else
                sql = "select sv.MaSV,sv.Image from SinhVien sv, SV_LHP r1 where sv.MaSV=r1.MaSV and r1.MaLop=N'" + MaLop + "'";
            DataTable tb1 = cn.getTable(sql);
            foreach (DataRow row in tb1.Rows)
            {
                listID.Add(row[0].ToString());
                MemoryStream stream = new MemoryStream((byte[])row[1]);
                img = new Image<Gray, byte>(new Bitmap(stream));
                listImg.Add(img);
            }
            connect = cn.getConnect();
            connect.Open();
            if (tag == 1)
            {
                insertDate("DH_SV_LCN");
            }
            else
            {
                insertDate("DH_SV_LHP");
            }
        }
        public void ProcessFrame(object sender, EventArgs arg)
        {
            if (!picProcess)
            {
                ImageFrame = capture.QueryFrame();
                imgCamera.Image = ImageFrame;
                RecognitionFace();
            }

        }
        public void RecognitionFace()
        {
            if (ImageFrame != null)
            {
                Image<Gray, Byte> grayFrame = ImageFrame.Convert<Gray, Byte>();
                var faces = grayFrame.DetectHaarCascade(haar, 1.3, 4,
                                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                new Size(25, 25))[0];   // mang chua cac khuon mat phat hien duoc
                                                        /*
                                                            haar: trained data
                                                            1.1: Scale Increase Rate (1.1,1.2,1.3,1.4) càng nhỏ: phát hiện được nhiều khuôn mặt -> chậm hơn. 
                                                            4:  Minimum Neighbors Threshold 0->4  giá trị cao phát hiện chặt chẽ hơn
                                                            CANNY_PRUNING:  (0)
                                                            new Size(25, 25): size of the smallest face to search for. mặc định 25x25
                                                         */

                if (faces.Length == 0)
                    clear();
                else
              //  foreach(var f in faces)
                {

                    var f = faces[0];
                    imgTrain.Image = ImageFrame.Copy(f.rect).Convert<Bgr, Byte>().Resize(148,
                                                        161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); // hien thi 1 khuon mat len imgTrain => nhap thong tin
                    objFace = ImageFrame.Copy(f.rect).Convert<Gray, Byte>().Resize(148,
                                                       161, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    ImageFrame.Draw(f.rect, new Bgr(Color.Red), 3); // danh dau khuon mat phat hien
                    if (listID.Count != 0)
                    {
                        //Console.Beep();
                        MCvTermCriteria term = new MCvTermCriteria(listID.Count, 0.001);
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(listImg.ToArray(), listID.ToArray(), ref term);
                        string id;
                        id = recognizer.Recognize(objFace);
                        //showInfo(sv);
                        ImageFrame.Draw(id, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                        
                        sql = "select sv.MaSV,HoTen,l.TenLop,NgaySinh,GioiTinh from SinhVien sv, Lop l where sv.MaLop=l.MaLop and MaSV='" + id + "'";
                        SqlDataAdapter da = new SqlDataAdapter(sql, connect);
                        DataTable tb = new DataTable();
                        da.Fill(tb);
                        txtMaSV.Text = tb.Rows[0][0].ToString();
                        txtHoTen.Text = tb.Rows[0][1].ToString();
                        txtLop.Text = tb.Rows[0][2].ToString();
                        txtNgaySinh.Text = tb.Rows[0][3].ToString();
                        string gt = tb.Rows[0][4].ToString();
                        if (gt.Equals("Nam"))
                            radNam.Checked = true;
                        else
                            radNu.Checked = true;
                        update(id);

                    }

                }
            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            captureInProcess = false;
            if (capture != null)
            {

                capture = null;
            }

            try
            {
                capture = new Capture(cbCamIndex.SelectedIndex);
                Application.Idle += ProcessFrame;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmDiemDanh_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade_frontalface_alt_tree.xml");
            
            
        }
        public void addColumn()
        {
            
            
        }
        public void insertDate(string lop)
        {
            
            sql = "select top 1 Ngay "
                 + " from "+lop
                + " where MaLop =N'" + this.MaLop + "'"
                 + " order by Ngay desc";

           SqlDataAdapter da = new SqlDataAdapter(sql, connect);
            DataTable tb = new DataTable();
            da.Fill(tb);
            if (tb.Rows.Count > 0)
            {
                date = Convert.ToDateTime(tb.Rows[0][0], new CultureInfo("vi-VN")).Date;
                if (date.CompareTo(today) < 0)
                {
                    foreach (string id in listID)
                    {
                        sql = " insert into " + lop + " values(@malop,@masv,@ngay,@nghi)";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connect;
                        cmd.CommandText = sql;
                        //command.Connection = connect;

                        cmd.Parameters.Add("@malop", SqlDbType.Char).Value = MaLop;
                        cmd.Parameters.Add("@masv", SqlDbType.Char).Value = id;
                        cmd.Parameters.Add("@ngay", SqlDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("@nghi", SqlDbType.Int).Value = 1;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            } else
            {
                foreach (string id in listID)
                {
                    sql = " insert into " + lop + " values(@malop,@masv,@ngay,@nghi)";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connect;
                    cmd.CommandText = sql;
                    //command.Connection = connect;

                    cmd.Parameters.Add("@malop", SqlDbType.Char).Value = MaLop;
                    cmd.Parameters.Add("@masv", SqlDbType.Char).Value = id;
                    cmd.Parameters.Add("@ngay", SqlDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("@nghi", SqlDbType.Int).Value = 1;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            
        }
        public void update(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connect;
            if(tag==1)
            {
                sql = "update DH_SV_LCN set nghi =0 where MaSV='" + id + "'";
            }
            else
            {
                sql = "update DH_SV_LHP set nghi =0 where MaSV='" + id + "'";
            }
            cmd.CommandText = sql;
           // command.Connection = connect;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void frmDiemDanh_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (capture != null)
                capture.Dispose();
        }
        private void clear()
        {
            imgTrain.Image = null;
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            txtLop.Text = "";
            txtNgaySinh.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;

        }

        private void frmDiemDanh_FormClosing(object sender, FormClosingEventArgs e)
        {
            connect.Close();
        }
    }
}
