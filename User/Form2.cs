using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using User.Model;
using DevExpress.XtraReports.UI;

namespace User
{
    public partial class Form2 : Form
    {
        HttpClient client = new HttpClient();
        String URL = "http://localhost/API/";

        public object ScreenShotpicture { get; private set; }

        public Form2()
        {
            InitializeComponent();
        }
        private async Task<int> AddSreenShort()
        {
            Image IM = Image.FromFile("image.png");
            HttpResponseMessage response = await client.PostAsJsonAsync(URL + "SCREENSHOTs", IM);
            response.EnsureSuccessStatusCode();
            // Return the URI of the created resource.
            int i = await response.Content.ReadAsAsync<int>();
            MessageBox.Show("a:" + i);
            return i;
        }
        private async void ScreenShort()
        {
            while(true)
            {
                Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                Graphics graphics = Graphics.FromImage(bitmap as Image);

                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

                Image o = (Image)bitmap;
                ImageConverter _imageConverter = new ImageConverter();
                ScreenShotPicture s = new ScreenShotPicture();
                s.Id_User = "ADMIN_US_01";
                s.Name = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
                s.Time = DateTime.Now;
                byte[] xByte = (byte[])_imageConverter.ConvertTo(o, typeof(byte[]));
                s.Picture = xByte;
                HttpResponseMessage response = await client.PostAsJsonAsync(URL + "Picture/API", s);
                response.EnsureSuccessStatusCode();
                int i = await response.Content.ReadAsAsync<int>();
                Thread.Sleep(10000);
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime d = new DateTime(2017, 1, 1, 13, 30, 00);
            DateTime n = new DateTime(2017, 1, 1, 8, 00, 00);
            TimeSpan tp = d - n;
            double m = tp.TotalHours;
            MessageBox.Show(m+"");

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ScreenShort));
                t.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            USER us = new USER();
            us.Id_User = "US_01"; us.Name = "XUÂN"; us.Regiter_Day = DateTime.Today;
          //  XtraReport1 xtr = new XtraReport1();
            DataTable dt = new DataTable();
            dt.Columns.Add("STT");
            dt.Columns.Add("Date_Start");
            dt.Columns.Add("Date_End");
            dt.Columns.Add("Time");
            dt.Rows.Add("1", "13/12/2016", "31/12/2016", "32");
            dt.Rows.Add("1", "13/12/2016", "31/12/2016", "32");
          //  xtr.Load(us, "Coder", "TUAN ANH");
          //  xtr.DataSource = dt;
          //  xtr.ShowPreviewDialog();
        }
    }
}
