using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
namespace STM_PDA_System.MasterInput
{
    /// <summary>
    /// Interaction logic for wMaster.xaml
    /// </summary>
    public partial class wMaster : Window
    {
        List<Entity.FileConfig> listFile = new List<Entity.FileConfig>();

        public wMaster()
        {
            InitializeComponent();
            listFile.Add(new Entity.FileConfig { values = 1, display = "Thông tin Model" });
            listFile.Add(new Entity.FileConfig { values = 2, display = "Thông tin User" });
            listFile.Add(new Entity.FileConfig { values = 3, display = "Thông tin múi giờ" });
            listFile.Add(new Entity.FileConfig { values = 4, display = "Thông tin COM" });
            cbTypeData.ItemsSource = listFile.ToList();
            cbTypeData.SelectionChanged += delegate
            {
                switch (Convert.ToUInt32(cbTypeData.SelectedValue))
                {
                    case 1:                       
                        dgUpdate.ItemsSource = Entity.AddData.msModel();
                        break;
                    case 2:                        
                        dgUpdate.ItemsSource = Entity.AddData.msUser();
                        break;
                    case 3:                       
                        dgUpdate.ItemsSource =Entity.AddData.TimeInDay().Select(f =>
                        new Entity.TimeInDay_temp
                        {
                            stt = f.stt, tenCa = f.tenCa, TimeFrom = f.TimeFrom.ToString("HH:mm:ss"),
                            TimeTo = f.TimeTo.ToString("HH:mm:ss")
                        }).ToList();
                        break;
                    case 4:
                        dgUpdate.ItemsSource = Entity.AddData.ComPort();
                        break;


                }
            };
            btUpdate.Click += delegate
            {
                bool check_update = false;
                switch (Convert.ToUInt32(cbTypeData.SelectedValue))
                {
                    case 1:
                        string listModel = "";
                        var check_duplicate = dgUpdate.Items.OfType<Entity.msModel>().GroupBy(f => f.Model_nm).
                                              Where(g => g.Count() >1).
                                              Select(k => k.Key); 
                        if(check_duplicate.FirstOrDefault() == null)
                        {
                        check_update = true;
                        using (StreamWriter sr = new StreamWriter("Setup\\ModelList.txt", false, Encoding.Unicode))
                        {
                            foreach (var r in dgUpdate.Items)
                            {
                                var item = r as Entity.msModel;
                                if (item != null)
                                    sr.WriteLine(item.Model_nm + "," + item.peole_line + "," +item.cycle_time+","+ item.warning + "," +
                                        item.height_warning+","+item.num_in_box+","+item.Barcode+","+item.KiTuBarcode);
                            }
                        };
                        }
                        else
                        {
                            check_update = false;
                            check_duplicate.ToList().ForEach(r => { listModel = listModel + r+","; });
                            System.Windows.MessageBox.Show("các model :" + listModel +" đã tồn tại");

                        }
                        break;
                    case 2:
                        string listUser = "";
                        var check_duplicate2 = dgUpdate.Items.OfType<Entity.msUser>().GroupBy(f => f.user_nm).
                                              Where(g => g.Count() > 1).
                                              Select(k => k.Key);
                        if (check_duplicate2.FirstOrDefault() == null)
                        {
                            check_update = true;
                            using (StreamWriter sr = new StreamWriter("Setup\\User.txt", false, Encoding.Unicode))
                            {
                                foreach (var r in dgUpdate.Items)
                                {
                                    var item = r as Entity.msUser;
                                    if (item != null)
                                        sr.WriteLine(item.user_nm + "," + item.full_name);
                                }
                            };
                        }
                        else
                        {
                            check_update = false;
                            check_duplicate2.ToList().ForEach(r => { listUser = listUser + r + ","; });
                            System.Windows.MessageBox.Show("các model :" + listUser + " đã tồn tại");

                        }

                        break;
                    case 3:                                               
                         using (StreamWriter sr = new StreamWriter("Setup\\TimeInDay.txt", false, Encoding.Unicode))
                        {
                            foreach (var r in dgUpdate.Items)
                            {
                                var item = r as Entity.TimeInDay_temp;
                                if (item != null)
                                    sr.WriteLine(item.tenCa+","+item.stt+","+item.TimeFrom+","+item.TimeTo);
                            }
                        };
                        break;
                    case 4:
                        string listCom = "";
                        var check_duplicate4 = dgUpdate.Items.OfType<Entity.ComPort>().GroupBy(f => f.ma).
                                              Where(g => g.Count() > 1).
                                              Select(k => k.Key);
                        if(check_duplicate4.FirstOrDefault() ==  null)
                        {
                        check_update = true;
                        using (StreamWriter sr = new StreamWriter("Setup\\Comport.txt", false, Encoding.Unicode))
                        {
                            foreach (var r in dgUpdate.Items)
                            {
                                var item = r as Entity.ComPort;
                                if (item != null)
                                    sr.WriteLine(item.Ghi_Chu + "," + item.Cong_com + "," + item.BaudRate + "," + item.So_bit+","
                                        + item.Parity + ","+item.StopBits+","+ item.ma);
                            }
                        };
                        }
                        else
                        {
                            check_update = false;
                            check_duplicate4.ToList().ForEach(r => { listCom = listCom + r + ","; });
                            System.Windows.MessageBox.Show("các model :" + listCom + " đã tồn tại");
                        }
                        break;
                };
                if(check_update)
                { 
                MessageBox.Show("Chương trình sẽ đóng để lưu file", "information");                            
                Application.Current.Shutdown();
                }
            };
        }
    }
}
