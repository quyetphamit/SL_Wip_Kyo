using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO.Ports;
using System.ComponentModel;
using System.Windows.Forms;
using System.SupportTool4._0;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace STM_PDA_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
 

   
    public class BindingVariable
    {
       public int _total { get; set; }
       public int _check_time_work { get; set; }
       public int _count_s { get; set; }
       public bool _StartProduct { get; set; }
       public int _checkWarning { get; set; }
       public bool _checkBarcode { get; set; }
    }

    public partial class MainWindow : Window
    {
        bool chkBarcode { get { return (bool)GetValue(chkBarcodeProperty); } set { SetValue(chkBarcodeProperty, value); } }
        public static readonly DependencyProperty chkBarcodeProperty = DependencyProperty.Register("chkBarcode", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        bool BitPress = false;
        bool TangSL = false;
        int total, checkWarning, check_time_work, count_s;
        bool StartProduct = false;
        DateTime time_Now;
        double time_run = 0;
        Entity.msModel modelSetting;
        SerialPort ledPort, pressPort;
        int CountProduct, NoPeople, ProductPlan, BalanceProduction;
        string ca1;
        int count = 0;
       
        public string FindApplication(string NameSoft)
        {

            string astring = null;
            foreach (Process p in Process.GetProcesses())
            {
                string h = p.MainWindowTitle.ToString(); //lay tung title cua tung process
                if (h.Length > 0)
                {
                    if (h.Contains(NameSoft))
                    {
                        astring = h;
                    }
                }
            }
            return astring;

        }
        //=========================================================
        DispatcherTimer timer1 = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        DispatcherTimer timer2 = new DispatcherTimer()
        {            
            Interval = TimeSpan.FromSeconds(1)
        };       
        DispatcherTimer timer4 = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        IEnumerable<Entity.History> data  ;       
        SerialPort setupComPort(string com)
        { 
            var portSetting = Entity.AddData.ComPort().FirstOrDefault(f => f.ma==com);
            if (portSetting != null)
            {
                SerialPort comPort1 = new SerialPort()
                {
                    PortName = portSetting.Cong_com,
                    BaudRate = portSetting.BaudRate,
                    DataBits = portSetting.So_bit,
                    Parity = portSetting.Parity == "None" ? Parity.None :
                             portSetting.Parity == "Even" ? Parity.Even :
                             portSetting.Parity == "Odd" ? Parity.Odd :
                             portSetting.Parity == "Mark" ? Parity.Mark : Parity.Space,
                    StopBits = portSetting.StopBits == 0 ? StopBits.None :
                               portSetting.StopBits == 1 ? StopBits.One : StopBits.Two
                };
                try
                {
                    comPort1.Open();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return comPort1;
            }
            else return null;
        }

        void  getHistoryData(string model,DateTime date)
        {           
            StartProduct = false;              
            modelSetting = Entity.AddData.msModel().FirstOrDefault(f => f.Model_nm == cbModel.SelectedValue.ToString().Trim());           
            if (Directory.Exists("LogFile\\" + model) == false) Directory.CreateDirectory("LogFile\\" + model);
            string ca = (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) ? "_HC" : "_D";
            ca1 = (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) ? "HC" : "D";
            string path = System.IO.Path.Combine("LogFile\\" + model, date.ToString("dd-MM-yyyy")+ca+ ".txt");
            if(File.Exists(path))
            {
            List<Entity.History> getUpdateHis;
            string[] row1 = File.ReadAllLines(path).FirstOrDefault().ToString().Split(',');
            time_run = Convert.ToDouble(row1[0]); CountProduct = Convert.ToInt32(row1[1]);
            getUpdateHis = Entity.AddData.History(path);
            dgHistory.ItemsSource = getUpdateHis;           
            }
            else
            {                               
                data = Entity.AddData.TimeInDay().Where(f => f.tenCa == ca1).Select(f => new Entity.History
                {
                    stt = f.stt,
                    Ca = f.tenCa,
                    Thoi_Gian = f.TimeFrom.ToString("HH:mm:ss") + " - " + f.TimeTo.ToString("HH:mm:ss"),
                    TimeFrom = (f.TimeFrom.Hour >= 0 && f.TimeFrom.Hour < 8) ? Convert.ToDateTime(date.AddDays(1).ToString("yyy-MM-dd") + " " + f.TimeFrom.ToString("HH:mm:ss")) : f.TimeFrom,
                    TimeTo = (f.TimeTo.Hour >= 0 && f.TimeTo.Hour < 8) ? Convert.ToDateTime(date.AddDays(1).ToString("yyy-MM-dd") + " " + f.TimeTo.ToString("HH:mm:ss")) : f.TimeTo,
                    Ke_Hoach_Theo_Gio = Convert.ToInt32(Math.Round((f.TimeTo - f.TimeFrom).TotalSeconds / modelSetting.cycle_time, 0, MidpointRounding.AwayFromZero)),
                    SL_Thuc_Te = 0, 
                    chenh_lech = 0,
                    Ghi_Chu = ""
                }
                ).ToList();
                dgHistory.ItemsSource = data;
                time_run = 0; CountProduct = 0;                
            }
            total = dgHistory.Items.OfType<Entity.History>().Sum(r => r.SL_Thuc_Te);
            txtPeople.Text = modelSetting.peole_line.ToString();
            txtCycleTime.Text = modelSetting.cycle_time.ToString();
            ProductPlan =Convert.ToInt32( Math.Round(time_run / modelSetting.cycle_time, 0, MidpointRounding.AwayFromZero));
            txtKehoach.Text = ProductPlan.ToString();
            txtChenhLenh.Text = (total - ProductPlan).ToString();
            txtCycleTT.Text =Math.Round(time_run / total,1,MidpointRounding.AwayFromZero).ToString();          
                     
            DataContext = new BindingVariable() { _checkBarcode = modelSetting.Barcode,  _total = total, _check_time_work = check_time_work, _count_s = count_s, _StartProduct = StartProduct, _checkWarning = checkWarning };
            //wirteTextfile(model, date);
        }
        void wirteTextfile(string model, DateTime date)
        {
            var autoUpdateHis1 = dgHistory.Items.OfType<Entity.History>();
            if (Directory.Exists("LogFile\\" + model) == false) Directory.CreateDirectory("LogFile\\" + model);
            string ca = (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) ? "_HC" : "_D";
            string path = System.IO.Path.Combine("LogFile\\" + model, date.ToString("dd-MM-yyyy") + ca + ".txt");
            using (StreamWriter sr = new StreamWriter(path, false, Encoding.Unicode))
            {
                sr.WriteLine(time_run + "," + CountProduct);
                autoUpdateHis1.ToList().ForEach(r =>
                {
                    sr.WriteLine(r.stt + "," + r.Ca + "," + r.Thoi_Gian + "," + r.TimeFrom + "," + r.TimeTo + "," + r.Ke_Hoach_Theo_Gio + "," +
                                 r.SL_Thuc_Te + "," + r.chenh_lech + "," + r.Ghi_Chu);
                });
            }
        }
        bool  check_barcode_dupplicate(string input)
        {
            if (File.Exists("Log.txt") == false) return false;
            else
            { 
            var Log = File.ReadAllLines("Log.txt").FirstOrDefault(f => f.ToString().Trim() == input);
            if (Log == null) return false;
            else   return true;
            }
        }

        void TangSoLuong()
        {
            
                if (cbModel.SelectedValue != null)
                {
                    if (check_time_work != 0)
                    {
                        txtCycleTT.Text = Math.Round(time_run / total, 1, MidpointRounding.AwayFromZero).ToString();
                        List<Entity.History> autoUpdateHis = new List<Entity.History>();
                        foreach (var r in dgHistory.Items)
                        {
                            var item = r as Entity.History;
                            autoUpdateHis.Add(new Entity.History
                            {
                                stt = item.stt,
                                Ca = item.Ca,
                                Thoi_Gian = item.Thoi_Gian,
                                TimeFrom = item.TimeFrom,
                                TimeTo = item.TimeTo,
                                Ke_Hoach_Theo_Gio = item.Ke_Hoach_Theo_Gio,
                                SL_Thuc_Te = (DateTime.Now >= item.TimeFrom && DateTime.Now <= item.TimeTo) ?
                                         item.SL_Thuc_Te + 1 : item.SL_Thuc_Te,
                                chenh_lech = (DateTime.Now >= item.TimeFrom && DateTime.Now <= item.TimeTo) ?
                                         item.SL_Thuc_Te + 1 - item.Ke_Hoach_Theo_Gio : item.SL_Thuc_Te == 0 ? 0 : item.SL_Thuc_Te - item.Ke_Hoach_Theo_Gio,
                                Ghi_Chu = item.Ghi_Chu
                            });
                        }
                        dgHistory.ItemsSource = autoUpdateHis;
                    }
                    txtThucTe.Text = total.ToString();
                }
        }
        
    public MainWindow()
        {
            InitializeComponent();
            if (File.Exists("Log.txt")) File.Delete("Log.txt");     
            dpDate.SelectedDate = (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8) ? DateTime.Now.AddDays(-1) : DateTime.Now;
            dpDate.DisplayDateEnd = (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8) ? DateTime.Now.AddDays(-1) : DateTime.Now;
            string chuoi = "Design by: LCA - Phạm Ngọc Ánh";
            int dem_chuoi = 0;
            ca1 = (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) ? "HC" : "D";
            txtDay.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
            cbModel.ItemsSource = Entity.AddData.msModel().OrderBy(f => f.Model_nm);
            var check = Entity.AddData.TimeInDay().FirstOrDefault(r => DateTime.Now >= r.TimeFrom && DateTime.Now <= r.TimeTo
                             && r.tenCa == ca1);
            check_time_work = check == null ? 0 : 1;
            timer1.Start();
            ledPort = setupComPort("com01");
            pressPort = setupComPort("com02");
            this.Closed += delegate
            {
                if(ledPort!=null)
                {
                    if (ledPort.IsOpen)  ledPort.Write("C");                    
                }
                if(cbModel.SelectedValue!=null)
                wirteTextfile(cbModel.SelectedValue.ToString(), dpDate.SelectedDate.Value);
            };
            btStart.Click += delegate
             {
                 if (cbModel.Text != "")
                 {
                     txtBarcode.Focus();
                     DateTime dateNow = (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8) ? DateTime.Now.AddDays(-1) : DateTime.Now;
                     if (dateNow.ToString("dd-MM-yyyy") == dpDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                     {
                         StartProduct = true;
                         time_Now = DateTime.Now;
                         timer2.Start(); timer4.Start();
                         chkBarcode = modelSetting.Barcode;
                     }
                 }
                 else
                 {
                     System.Windows.Forms.MessageBox.Show("Bạn cần chọn model bản mạch", "Thông tin lỗi",MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             };
            btEnd.Click += delegate
            {
                chkBarcode = false;
                wirteTextfile(cbModel.SelectedValue.ToString(), dpDate.SelectedDate.Value);
                StartProduct = false;               
                timer2.Stop();  timer4.Stop();

                DataContext = new BindingVariable() { _total = total, _check_time_work = check_time_work, _count_s = count_s, _StartProduct = StartProduct, _checkWarning = checkWarning };
                if (ledPort.IsOpen) ledPort.Write("C");
            };
            timer1.Tick += delegate
            {
                if (dem_chuoi > 50) { chuoi = "Design by: LCA - Phạm Ngọc Ánh"; dem_chuoi = 0; }
                this.Title = chuoi;
                chuoi = "     " + chuoi;
                dem_chuoi++;

                txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
                var check1 = Entity.AddData.TimeInDay().FirstOrDefault(r => DateTime.Now >= r.TimeFrom && DateTime.Now <= r.TimeTo
                            && r.tenCa == ca1);
                check_time_work = check1 == null ? 0 : 1;
                count = check_time_work == 0 ? count + 1 : 0;
                count_s = count % 2;
            };
            timer2.Tick += delegate
            {
                txtChenhLenh.Text = (total - ProductPlan).ToString();
                int perBalanceProduction = ProductPlan == 0 ? 0 : 100 * (total - ProductPlan) / ProductPlan;
                if (ledPort != null)
                {
                    CountProduct = 100; NoPeople = modelSetting.peole_line;
                    BalanceProduction = total - ProductPlan;
                    string ArraySend = total.ToString("0000") + ProductPlan.ToString("0000") + NoPeople.ToString("00") + "*";
                    if (BalanceProduction <= -1000)
                        ArraySend = "S-999" + ArraySend;
                    else if (BalanceProduction > -1000 && BalanceProduction < 0)
                        ArraySend = "S" + BalanceProduction.ToString("000") + ArraySend;
                    else
                        ArraySend = "S+" + BalanceProduction.ToString("000") + ArraySend;
                    if (ledPort.IsOpen) ledPort.WriteLine(ArraySend);

                    if (perBalanceProduction < modelSetting.height_warning)
                    {
                        if (ledPort.IsOpen) ledPort.Write("D");
                    }
                    else if (perBalanceProduction < modelSetting.warning)
                    {
                        if (ledPort.IsOpen) ledPort.Write("V");
                    }
                    else
                    {
                        if (ledPort.IsOpen) ledPort.Write("X");
                    }
                    if (check_time_work == 0)
                        if (ledPort.IsOpen) ledPort.Write("R");
                }
                if (perBalanceProduction < modelSetting.height_warning) checkWarning = 3;
                else if (perBalanceProduction < modelSetting.warning) checkWarning = 2;
                else checkWarning = 1;              
                total = dgHistory.Items.OfType<Entity.History>().Sum(r => r.SL_Thuc_Te);
                DataContext = new BindingVariable() { _checkBarcode = modelSetting.Barcode, _total = total, _check_time_work = check_time_work, _count_s = count_s, _StartProduct = StartProduct, _checkWarning = checkWarning };
            };
           timer4.Tick += delegate
            {
                if (cbModel.SelectedValue != null)
                {
                    if (check_time_work != 0)
                    {
                        time_run = time_run + (DateTime.Now - time_Now).TotalSeconds;
                        ProductPlan = Convert.ToInt32(Math.Round(time_run / modelSetting.cycle_time, 0, MidpointRounding.AwayFromZero));
                        txtKehoach.Text = ProductPlan.ToString();
                    }
                    time_Now = DateTime.Now;                   
                }                
            };
 
            
            cbModel.SelectionChanged += delegate 
            {
                getHistoryData(cbModel.SelectedValue.ToString(), dpDate.SelectedDate.Value);

            };
            dpDate.SelectedDateChanged += delegate
            {
                if(cbModel.SelectedValue != null)
                    getHistoryData(cbModel.SelectedValue.ToString(), dpDate.SelectedDate.Value);
            };
            pbSetting.KeyDown += (o,e) =>
            {
                if(e.Key == Key.Enter)
                {
                    var pass = Entity.AddData.msUser().FirstOrDefault(r => r.user_nm == pbSetting.Password.Trim());
                    if (pass != null)
                    {
                        MasterInput.wMaster wmaster = new MasterInput.wMaster();
                        wmaster.Show();
                        pbSetting.Password="";
                    }
                    else
                        System.Windows.MessageBox.Show("User không tồn tại!!!", "information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    pbSetting.SelectAll();
                }
            };
            tbsetting.MouseDown += delegate 
            {
                if(pbSetting.Password!="")
                {
                txtSetting.Visibility = Visibility.Visible;
                txtSetting.Text = pbSetting.Password;
                }
            };
            tbsetting.MouseLeave += delegate
             {
                 if (pbSetting.Password != "")
                 {
                     pbSetting.Visibility = Visibility.Visible;
                     txtSetting.Visibility = Visibility.Hidden;
                 }
             };
            txtBarcode.KeyDown += (o, e) =>
            {
                if(e.Key == Key.Enter)
                {
                    var check_bacode = modelSetting.KiTuBarcode;
                    if (check_bacode == "" || (check_bacode != "" && txtBarcode.Text.ToUpper().Contains(check_bacode.ToUpper())))
                    {
                        int wipHandle = 0;
                        //string namesoft = FindApplication("Notepad");
                        string namesoft = FindApplication("Board Inspector");
                        //wipHandle = NativeWin32.FindWindow(null, "Board Inspector - [3000][FAT][VI2_KYD][KD01]");
                        wipHandle = NativeWin32.FindWindow(null, namesoft);
                        if (wipHandle == 0)
                            System.Windows.MessageBox.Show("Chương trình WIP chưa  bật !!!");
                        else
                        {
                            if (check_barcode_dupplicate(txtBarcode.Text))
                                System.Windows.MessageBox.Show("Mã barcod đã được soi", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            else
                            {
                                NativeWin32.SetForegroundWindow(wipHandle);
                                SendKeys.SendWait(txtBarcode.Text);
                                System.Threading.Thread.Sleep(300);
                                SendKeys.SendWait("{ENTER}");
                                System.Threading.Thread.Sleep(200);
                               SendKeys.SendWait("%{TAB}");

                                TangSoLuong();

                                wirteTextfile(cbModel.SelectedValue.ToString(), dpDate.SelectedDate.Value);
                                using (StreamWriter sr = new StreamWriter("Log.txt", true, Encoding.Unicode))
                                {
                                    sr.WriteLine(txtBarcode.Text);
                                };
                                txtBarcode.Clear();
                                txtBarcode.Focus();
                            }
                        }
                    }
                    else
                        System.Windows.MessageBox.Show("Không đúng kí tự nhận dạng Barcode","Information",MessageBoxButton.OK,MessageBoxImage.Warning);
                    //txtBarcode.SelectAll();
                }
            };
            btTangSL.Click += delegate
            {
                TangSoLuong();
            };
        }
    }
}
