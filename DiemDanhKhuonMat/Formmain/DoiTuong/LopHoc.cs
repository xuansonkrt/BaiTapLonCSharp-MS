using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formmain.DoiTuong
{
    public class LopHoc
    {
        public string MaLp { get; set; }
        public string TenLp { get; set; }
        public Dictionary<string, SinhVien> DSsv;
        public LopHoc()
        {
            DSsv = new Dictionary<string, SinhVien>();
        }
    }
}
