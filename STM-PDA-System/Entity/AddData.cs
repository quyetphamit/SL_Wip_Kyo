using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace STM_PDA_System.Entity
{
   public static class AddData
    {
        
        public static List<Entity.msModel> msModel()
        {
            List<Entity.msModel> model = new List<Entity.msModel>();
            File.ReadAllLines("Setup\\ModelList.txt").ToList().ForEach(r =>
            {
                string[] col = r.Split(',');
                model.Add(new Entity.msModel
                {
                    Model_nm = col[0].Trim(),
                    peole_line = Convert.ToInt32(col[1]),
                    cycle_time = Convert.ToDouble(col[2]),
                    warning = Convert.ToInt32(col[3]),
                    height_warning = Convert.ToInt32(col[4]),
                    num_in_box = Convert.ToInt32(col[5]),
                    Barcode = Convert.ToBoolean(col[6]), 
                    KiTuBarcode = col[7]
                });
            });
            return model;
        }

        public static List<Entity.TimeInDay> TimeInDay()
        {
            List<Entity.TimeInDay> TimeDay = new List<Entity.TimeInDay>();
            File.ReadAllLines("Setup\\TimeInDay.txt").ToList().ForEach(r =>
            {
                string[] col = r.Split(',');
                TimeDay.Add(new Entity.TimeInDay { tenCa = col[0], stt = Convert.ToInt32(col[1]), TimeFrom = Convert.ToDateTime(col[2]), TimeTo = Convert.ToDateTime(col[3]) });
            });
            return TimeDay;
        }

        public static List<Entity.msUser> msUser()
        {
            List<Entity.msUser> dbuser = new List<Entity.msUser>();
            File.ReadAllLines("Setup\\User.txt").ToList().ForEach(r =>
            {
                string[] col = r.Split(',');
                dbuser.Add(new Entity.msUser { user_nm = col[0], full_name = col[1] });
            });
            return dbuser;
        }
        public static List<Entity.ComPort> ComPort()
        {
            List<Entity.ComPort> comport = new List<Entity.ComPort>();
            File.ReadAllLines("Setup\\Comport.txt").ToList().ForEach(r =>
            {
                string[] col = r.Split(',');
                comport.Add(new Entity.ComPort
                {
                    Ghi_Chu = col[0], Cong_com = col[1],BaudRate =Convert.ToInt32(col[2]),So_bit = Convert.ToInt32(col[3]),
                    Parity = col[4], StopBits = Convert.ToInt32(col[5]),ma = col[6]
                });
            });
            return comport;
        }
        public static List<Entity.History> History(string path)
        {
            List<Entity.History> historydata = new List<Entity.History>();           
            File.ReadAllLines(path).Skip(1).ToList().ForEach(r =>
            {
                string[] col = r.Split(',');
                historydata.Add(new Entity.History
                {
                    stt = Convert.ToInt32(col[0]),
                    Ca = col[1],
                    Thoi_Gian = col[2],
                    TimeFrom = Convert.ToDateTime(col[3]),
                    TimeTo = Convert.ToDateTime(col[4]),
                    Ke_Hoach_Theo_Gio = Convert.ToInt32(col[5]),
                    SL_Thuc_Te = Convert.ToInt32(col[6]),
                    chenh_lech  = Convert.ToInt32(col[7]),
                    Ghi_Chu = col[8]
                });
            });
            return historydata;
        }
    }
}
