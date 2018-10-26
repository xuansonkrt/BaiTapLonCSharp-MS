using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formmain
{
    class ConnectDB
    {
        SqlConnection con;
        SqlCommand sqlcom;
        SqlDataReader sqldr;
        public ConnectDB()
        {
            SqlConnection con = new SqlConnection("Data Source=MYPC\\SQLEXPRESS;Initial Catalog=CSDLDiemDanh;"
                                    + "Persist Security Info=True;User ID=diemdanh;Password=123");

        }
        public SqlConnection getConnect()
        {
            return new SqlConnection("Data Source=MYPC\\SQLEXPRESS;Initial Catalog=CSDLDiemDanh;"
                                    +"Persist Security Info=True;User ID=diemdanh;Password=123");
        }
        public DataTable getTable(string sql)
        {
            con = getConnect();
            SqlDataAdapter ad = new SqlDataAdapter(sql,con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }
    }
}
