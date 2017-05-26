using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STM_PDA_System.Entity
{

    public class msModel
    {
        [Key]
        public string Model_nm { get; set; }
        public int peole_line { get; set; }
        public Double cycle_time { get; set; }
        public int warning { get; set; }
        public int height_warning { get; set; }
        public int num_in_box { get; set; }

        public bool Barcode { get; set; }
        public string KiTuBarcode { get; set; }
    }

    public class ComPort
    {
       public string ma { get; set; }
       public string Ghi_Chu { get; set; }
       public string Cong_com { get; set; }
       public int BaudRate { get; set; }
       public int So_bit { get; set; }
       public string Parity { get; set; }
       public int StopBits { get; set; }

    }
}
