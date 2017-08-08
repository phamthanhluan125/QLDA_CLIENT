using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using User.Model;

namespace User
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        static HttpClient client = new HttpClient();
        String URL = "http://localhost/API/";
        WORK work;
        Thread couttime;
        public Form1()
        {
            InitializeComponent();
        }

        private async Task<int> AddWork(WORK work)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(URL + "WORKs/AddNewWork", work);
            response.EnsureSuccessStatusCode();

            // Return the URI of the created resource.
            int i = await response.Content.ReadAsAsync<int>();
            return i;
        }
        private async Task<int> InsertTimeEnd(WORK work)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(URL + "WORKs/InsertEndTime", work);
            response.EnsureSuccessStatusCode();

            // Return the URI of the created resource.
            int i = await response.Content.ReadAsAsync<int>();
            return i;
        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lb_timeworking.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lb_timeworking.Text = text;
            }
        }
        private void CountTime()
        {
            int h = 0, m = 0, s = 0;
            while(true)
            {
                if(s == 60)
                {
                    s = 0;
                    m++;
                }
                if(m == 60)
                {
                    m = 0;
                    h++;
                }
                s++;
                SetText(h + " : " + m + " : " + s);
                Thread.Sleep(1000);
                //lb_timeworking.Text = h + " : " + m + " : " + s; 
                
            }
        }
        private async void btn_workStart_Click(object sender, EventArgs e)
        {
            work = new WORK();
            work.TimeStart = DateTime.Now;
            work.Id_User = "ADMIN_US_01";
            work.TimeEnd = null;
            int i = await AddWork(work);
            if (i != 0)
            {
                work.Id = i;
                lb_timeStart.Text = work.TimeStart.Hour + " : " + work.TimeStart.Minute;
                couttime = new Thread( new ThreadStart(CountTime));
                couttime.Start();
                btn_workStop.Enabled = true;
                btn_workStart.Enabled = false;
            }
            else MessageBox.Show("Server erro!","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private async void btn_workStop_Click(object sender, EventArgs e)
        {
           work.TimeEnd = DateTime.Now;
           int i = await InsertTimeEnd(work);
            if (i == 1)
            {
                couttime.Abort();
                MessageBox.Show("Thời gian làm việc: " + lb_timeworking.Text, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_workStart.Enabled = true;
                btn_workStop.Enabled = false;
                lb_timeStart.Text = "HH : MM";
                lb_timeworking.Text = "0 : 0 : 0";
            }
            else MessageBox.Show("Thất Bại");
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lb_today.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        private void lb_timeStart_Click(object sender, EventArgs e)
        {

        }

        private void lb_timeworking_Click(object sender, EventArgs e)
        {

        }

        private void lb1_Click(object sender, EventArgs e)
        {

        }

        private void lb_3_Click(object sender, EventArgs e)
        {

        }

        private void lb_today_Click(object sender, EventArgs e)
        {

        }

        private void lb_2_Click(object sender, EventArgs e)
        {

        }
    }
}
