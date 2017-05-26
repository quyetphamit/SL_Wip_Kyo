using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM_PDA_System.Entity
{
   public  class TimeInDay
    {
        public string tenCa { get; set; }
        public int    stt { get; set; }
        public DateTime TimeFrom { get; set; }

        public DateTime TimeTo { get; set; }
    }

    public class TimeInDay_temp
    {
        public string tenCa { get; set; }
        public int stt { get; set; }
        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }
    }

   public class History
    {
        public int stt { get; set; }
        public string Ca { get; set; }
        public string Thoi_Gian { get; set; }
        public int Ke_Hoach_Theo_Gio { get; set; }
        public int  SL_Thuc_Te { get; set; }
        public string Ghi_Chu { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int chenh_lech { get; set; }
    }
}
