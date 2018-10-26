using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Formmain.GUI
{
    public partial class frmDemoImage : Form
    {
        public frmDemoImage()
        {
            InitializeComponent();
        }
        SqlConnection connect = null;
        // string strconnect = " Server= HUUMANH\\SQLEXPRESS; Database=CSDLDiemDanh; User Id=Diemdanh; pwd=123";
        string strconnect = " Server= MYPC\\SQLEXPRESS; Database=CSDLDiemDanh; User Id=diemdanh; pwd=123";
        private void OpenCSDL()
        {
            try
            {
                connect = new SqlConnection(strconnect);
                connect.Open();
                MessageBox.Show(" Ke noi thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void CloseCSDL()
        {
            if (connect != null && connect.State == ConnectionState.Open)
            {
                connect.Close();
                MessageBox.Show(" Da dong CSDl thanh cong");
            }
        }
        private void btnImg_Click(object sender, EventArgs e)
        {
            OpenCSDL();
            SqlCommand cmd = new SqlCommand("Select Image from SinhVien where MaSV='55555' ", connect);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if(ds.Tables[0].Rows.Count>0)
            {
                MemoryStream stream = new MemoryStream((byte[])ds.Tables[0].Rows[0]["Image"]);
                pictureBox1.Image = new Bitmap(stream);
                Image<Bgr, Byte> img = new Image<Bgr, byte>(new Bitmap(stream));
                imageBox1.Image = img;
            }

           /* SqlDataAdapter adapter = null;
            adapter = new SqlDataAdapter("Select Image from SinhVien where MaSV='16150323'", connect);
            DataSet ds = new DataSet();
            SqlCommandBuilder builer = new SqlCommandBuilder(adapter);
            adapter.Fill(ds);

            //  int count = ds.Tables["Images"].Rows.Count;
            //  if (count > 0)
            // {
            Byte[] data = (Byte[])ds.Tables["Images"].Rows[0]["Image"];
                MemoryStream stream = new MemoryStream(data);
                //Image<Bgr,Byte> imgObj
                pictureBox1.Image = Image.FromStream(stream);
               // imageBox1.Image = imgObj;
           // }*/
            

        }
    }
}
