using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using User.Model;
using System.Net.Http;
using System.Threading;
using DevExpress.XtraEditors.Controls;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraReports.UI;

namespace User
{
    public partial class Home : DevExpress.XtraEditors.XtraForm
    {
        public static USER user;
        bool Working = false;
        PROJECT p;
        ADMIN manager;
        List<MESSANGE> ListMess_HTD, ListMess_D;
        List<TASK> ListTask;
        List<MANAGER_TASK> ListManagerTask;
        List<string> List_Id_Mail;
        HttpClient client = new HttpClient();
        String URL = "https://qlda-luan.herokuapp.com/v1/";
        WORK work;
        Thread couttime, workingupdate, SCR;
        private Home home;
        public Home()
        {
            InitializeComponent();
            Thread a = new Thread(new ThreadStart(LoadNewMail));
            a.Start();
        }

        public Home(Home home)
        {
            this.home = home;
        }

        private async void Home_Load(object sender, EventArgs e)
        {
            Loadding.Show();
            await LoadGroup();
            lb_today.Text = DateTime.Today.ToString("dd/MM/yyyy");
            await LoadCombobox();
            await LoadComboboxSendEmail();
            Loadding.PageVisible = false;
        }
        //----------CALL WEB API-----------------
        private async Task<PROJECT> GetProject(string id)
        {
            PROJECT p = new PROJECT();
            HttpResponseMessage response = await client.GetAsync(URL + "PROJECTs/GetProjectById/" + id);
            if (response.IsSuccessStatusCode)
            {
                p = await response.Content.ReadAsAsync<PROJECT>();
            }
            return p;
        }
        private async Task<List<WORK>> GetAllWork(string id)
        {
            List<WORK> li = new List<WORK>();
            HttpResponseMessage response = await client.GetAsync(URL + "WORKs/GetAllTWorkByIdUser/" + id);
            if (response.IsSuccessStatusCode)
            {
                li = await response.Content.ReadAsAsync<List<WORK>>();
            }
            return li;
        }
        private async Task<ADMIN> GetManager(string id)
        {
            ADMIN am = new ADMIN();
            HttpResponseMessage response = await client.GetAsync(URL + "ADMINs/GetAdminById/" + id);
            if (response.IsSuccessStatusCode)
            {
                am = await response.Content.ReadAsAsync<ADMIN>();
            }
            return am;
        }
        private async Task<List<ROLE>> GetAllRoleByIdManaer(string id)
        {
            List<ROLE> r = new List<ROLE>();
            HttpResponseMessage response = await client.GetAsync(URL + "ROLEs/GetAllRoleByIdManager/" + id);
            if (response.IsSuccessStatusCode)
            {
                r = await response.Content.ReadAsAsync<List<ROLE>>();
            }
            return r;
        }
        private async Task<List<USER>> GetAllUserByIdProject(string id)
        {
            List<USER> p = new List<USER>();
            HttpResponseMessage response = await client.GetAsync(URL + "USERs/GetAllUserByIdProject/" + id);
            if (response.IsSuccessStatusCode)
            {
                p = await response.Content.ReadAsAsync<List<USER>>();
            }
            return p;
        }
        private async Task<List<MESSANGE>> GetMail(String id, int style)
        {
            List<MESSANGE> lmess = new List<MESSANGE>();
            HttpResponseMessage response = await client.GetAsync(URL + "EMAILs/GetMailByIdFrom/" + id + "/" + style);
            if (response.IsSuccessStatusCode)
            {
                lmess = await response.Content.ReadAsAsync<List<MESSANGE>>();
                //MessageBox.Show(ListMess[0].Id +"");
            }
            return lmess;
        }
        private async Task<List<MANAGER_TASK>> GetALLManageTask(String id)
        {
            List<MANAGER_TASK> list_t = new List<MANAGER_TASK>();
            HttpResponseMessage response = await client.GetAsync(URL + "MANAGER_TAKSs/GetAllManageTaskByIdUser/" + id);
            if (response.IsSuccessStatusCode)
            {
                list_t = await response.Content.ReadAsAsync<List<MANAGER_TASK>>();
            }
            return list_t;
        }
        private async Task<List<TASK>> GetALLTask(String id)
        {
            List<TASK> list_t = new List<TASK>();
            HttpResponseMessage response = await client.GetAsync(URL + "TAKSs/GetAllTaskByIdUSer/" + id);
            if (response.IsSuccessStatusCode)
            {
                list_t = await response.Content.ReadAsAsync<List<TASK>>();
            }
            return list_t;
        }
        private async Task<int> AddWork(WORK work)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(URL + "WORKs/AddNewWork", work);
            response.EnsureSuccessStatusCode();

            // Return the URI of the created resource.
            int i = await response.Content.ReadAsAsync<int>();
            return i;
        }
        private async Task<int> AddNewMail(List<MESSANGE> mess)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(URL + "EMAILs/InsertMail", mess);
            response.EnsureSuccessStatusCode();

            // Return the URI of the created resource.
            int i = await response.Content.ReadAsAsync<int>();
            return i;
        }
        private async Task<int> UpdateMail(int id)
        {
            HttpResponseMessage response = await client.GetAsync(URL + "EMAILs/UpdateMail/" + id);
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
        //------------THREAD----------------------------
        delegate void SetTextCallback(string text);
        private void setButton(String textt)
        {
            if (this.btn_loadnewMail.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setButton);
                this.Invoke(d, new object[] { textt });
            }
            else
            {
                this.btn_loadnewMail.Text = textt;
            }
        }
	//=======upload image to server=================
        private async void Threa_SRC()
        {
            while (Working)
            {
                if (user.Time_ScreenShort > 0)
                {
                    Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    Graphics graphics = Graphics.FromImage(bitmap as Image);
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                    Image o = (Image)bitmap;
                    ImageConverter _imageConverter = new ImageConverter();
                    ScreenShotPicture s = new ScreenShotPicture();
                    s.Id_User = user.Id_User;
                    s.Time = DateTime.Now;
                    s.Name = user.Id_User + s.Time.Value.ToString("_dd_MM_yyyy_hh_mm_ss") + ".png";
                    byte[] xByte = (byte[])_imageConverter.ConvertTo(o, typeof(byte[]));
                    s.Picture = xByte;
                    HttpResponseMessage response = await client.PostAsJsonAsync(URL + "Picture/API", s);
                    response.EnsureSuccessStatusCode();
                    int i = await response.Content.ReadAsAsync<int>();
                    Thread.Sleep((int)user.Time_ScreenShort * 10000);
                }
            }
        }
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
            while (true)
            {
                if (s == 60)
                {
                    s = 0;
                    m++;
                }
                if (m == 60)
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
        private void ShowNotifyIcon(String Titte, String Text, int TimeShow)
        {
            notifyIcon1.BalloonTipTitle = Titte;
            notifyIcon1.BalloonTipText = Text;
            //Hiện message
            notifyIcon1.ShowBalloonTip(TimeShow * 1000);
        }
        private async void LoadNewMail()
        {
            ListMess_HTD = new List<MESSANGE>();
            ListMess_HTD = await GetMail(user.Id_User, 2);
            int i = ListMess_HTD.Count();
            while (true)
            {
                ListMess_HTD = new List<MESSANGE>();
                ListMess_HTD = await GetMail(user.Id_User, 2);
                if (i < ListMess_HTD.Count)
                {
                    i = ListMess_HTD.Count;
                    String messange = ListMess_HTD[ListMess_HTD.Count - 1].Id_From;
                    foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
                    {
                        if (c.Value.ToString() == messange)
                            messange = c.Description;
                    }
                    if (this.WindowState == FormWindowState.Minimized)
                        ShowNotifyIcon("Project Mananger", "E-Mail: Bạn có Email từ " + messange + ".", 15);
                    else
                    {
                        DialogResult d = MessageBox.Show("Bạn có thư mới từ" + messange + ".\nBạn có muốn mở Hộp Thư Đến?", "E-mail", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (d == DialogResult.Yes)
                        {
                            xtraTabPage4.Show();
                            xtraTabPage5.Show();
                        }
                    }
                }
                List<MESSANGE> m = (from x in ListMess_HTD
                                    where x.Seen == 0
                                    select x).ToList();
                setButton(m.Count + " Thư mới");
                Thread.Sleep(5000);
            }
        }
        private async void UpdateWorking()
        {
            while (Working)
            {
                work.TimeEnd = DateTime.Now;
                await InsertTimeEnd(work);
                Thread.Sleep(5 * 60000);
            }
        }
        //------------LOAD COMBOBOX-GROUP-GRID----------------
        private async Task LoadCombobox()
        {
            ListTask = await GetALLTask(user.Id_User);
            cbb_Task.DataSource = ListTask;
            cbb_Task.ValueMember = "Id_Task";
            cbb_Task.DisplayMember = "Name";
            cbb_Task.Refresh();
        }
        private async Task LoadGroup()
        {
            //Thong tin ca nhan
            lb_id.Text = user.Id_User;
            lb_name.Text = user.Name;
            lb_age.Text = user.Age.ToString();
            lb_rigDay.Text = user.Regiter_Day.Value.ToString("dd/MM/yyyy");
            //Thong tin du an
            p = new PROJECT();
            p = await GetProject(user.Id_Project);
            if (p == null)
            {
                lb_nameProject.Text = "Chưa được phân công dự án.";
            }
            else
            {
                lb_nameProject.Text = p.Id_Project;
                lb_dateStart.Text = p.Start_Day.Value.ToString("dd/MM/yyyy");
                lb_dateEnd.Text = p.Deadline_Day.Value.ToString("dd/MM/yyyy");

                if (p.Status_Now == 0)
                {
                    lb_statusNow.Text = "DỰ ÁN CHƯA BẮT ĐẦU";
                    lb_statusNow.ForeColor = Color.Violet;
                }
                else if(p.Status_Now == 1)
                {
                    lb_statusNow.Text = "DỰ ÁN ĐANG DIỄN RA";
                    lb_statusNow.ForeColor = Color.Green;
                }
                else if (p.Status_Now == 2)
                {
                    lb_statusNow.Text = "DỰ ÁN CHẬM TIẾN ĐỘ";
                    lb_statusNow.ForeColor = Color.Orange;
                }
                else if(p.Status_Now == 3)
                {
                    lb_statusNow.Text = "DỰ ÁN ĐÃ HOÀN THÀNH";
                    lb_statusNow.ForeColor = Color.Blue;
                }
                else
                {

                    lb_statusNow.Text = "DỰ ÁN THẤT BẠI";
                    lb_statusNow.ForeColor = Color.Red;
                }
            }
            //thong tin cong viẹc
            ListManagerTask = await GetALLManageTask(user.Id_User);
            int dem = 0;
            foreach (MANAGER_TASK t in ListManagerTask)
            {
                if (t.Finish_Day != null) dem++;
            }
            lb_numOfTask.Text = ListManagerTask.Count.ToString();
            lb_numOfFinshTask.Text = dem.ToString();
            lb_numOfTasked.Text = (ListManagerTask.Count - dem).ToString();
            //thong tin manager
            manager = await GetManager(user.Id_Admin);
            lb_manager_Name.Text = manager.Name;
            lb_manager_Company.Text = manager.Company_Name;
            lb_manager_Age.Text = manager.Age + "";
        }
        private async Task LoadMess_HTD()
        {
            ListMess_HTD = new List<MESSANGE>();
            List<MESSANGE> ListM = new List<MESSANGE>();
            ListM = await GetMail(user.Id_User, 2);
            if (check_mail_UnRead.Checked)
            {
                foreach (MESSANGE m in ListM)
                {
                    if (m.Seen == 1) ListMess_HTD.Add(m);
                }
            }
            if (check_mail_Read.Checked)
            {
                foreach (MESSANGE m in ListM)
                {
                    if (m.Seen == 0) ListMess_HTD.Add(m);
                }
            }
            gridControl1.DataSource = ListMess_HTD;
            gridControl1.Refresh();
        }
        private async Task LoadMess_D()
        {
            ListMess_D = new List<MESSANGE>();
            ListMess_D = await GetMail(user.Id_User, 1);
            gridControl2.DataSource = ListMess_D;
            gridControl2.Refresh();
        }
        private async Task LoadComboboxSendEmail()
        {
            List<USER> listUser = await GetAllUserByIdProject(p.Id_Project);
            List<ROLE> listRole = await GetAllRoleByIdManaer(manager.Id_Admin);
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Rows.Add(manager.Id_Admin, "Manager: " + manager.Name + "(" + manager.Id_Admin + ")");
            foreach (USER us in listUser)
            {
                String id = us.Id_User;
                String name = us.Name;
                foreach (ROLE rl in listRole)
                {
                    if (us.Id_Role == rl.Id_Role)
                        if (id == user.Id_User)
                            lb_role.Text = rl.Name;
                        else
                            name = rl.Name + ": " + name + "(" + us.Id_User + ")";
                }
                if (id != user.Id_User)
                    dt.Rows.Add(id, name);
            }
            cbb_main_ListTo.Properties.DataSource = dt;
            cbb_main_ListTo.Properties.ValueMember = "Id";
            cbb_main_ListTo.Properties.DisplayMember = "Name";
            cbb_main_ListTo.RefreshEditValue();
        }
        private void ClearNewMail()
        {
            foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
                c.CheckState = CheckState.Unchecked;
            txt_mail_Content_NewMail.Text = "";
        }

        //--------------EVENT-------------------
        private async void btn_workStart_Click_1(object sender, EventArgs e)
        {
            work = new WORK();
            work.TimeStart = DateTime.Now;
            work.Id_User = user.Id_User;
            work.TimeEnd = null;
            int i = await AddWork(work);
            if (i != 0)
            {
                work.Id = i;
                lb_timeStart.Text = work.TimeStart.Hour + " : " + work.TimeStart.Minute;
                couttime = new Thread(new ThreadStart(CountTime));
                couttime.Start();
                btn_workStop.Enabled = true;
                btn_workStart.Enabled = false;
                Working = true;
                workingupdate = new Thread(new ThreadStart(UpdateWorking));
                workingupdate.Start();
                SCR = new Thread(new ThreadStart(Threa_SRC));
                SCR.Start();
            }
            else MessageBox.Show("Server erro!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void btn_workStop_Click(object sender, EventArgs e)
        {
            SCR.Abort();
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
                Working = false;
                workingupdate.Abort();
            }
            else MessageBox.Show("Thất Bại");
        }

        private void cbb_Task_SelectedIndexChanged(object sender, EventArgs e)
        {
            String id_task = cbb_Task.SelectedValue.ToString();
            foreach (TASK t in ListTask)
            {
                if (t.Id_Task == id_task)
                {
                    lb_Tast_Name.Text = t.Name;
                    lb_Tast_Note.Text = t.Note;
                }
            }
            foreach (MANAGER_TASK m in ListManagerTask)
            {
                if (m.Id_Task == id_task)
                {
                    lb_Task_ReciveDay.Text = m.Receive_Day.Value.ToString("dd/MM/yyyy");
                    lb_Task_DeadlineDay.Text = m.Deadline_Day.Value.ToString("dd/MM/yyyy");
                    if (m.Finish_Day == null)
                    {
                        lb_Task_FinshDay.Text = "N/A";
                        lb_Task_StatusNow.Text = "Công việc chưa được hoàn thành.";
                        lb_Task_StatusNow.ForeColor = Color.Orange;
                    }
                    else
                    {
                        lb_Task_FinshDay.Text = m.Finish_Day.Value.ToString("dd/MM/yyyy");
                        lb_Task_StatusNow.Text = "Công việc đã được hoàn thành.";
                        lb_Task_StatusNow.ForeColor = Color.Blue;
                    }
                }
            }
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            // MessageBox.Show(e.RowHandle + "");
            if (e.RowHandle >= 0 && ListMess_HTD[e.RowHandle].Seen == 0)
            {
                e.Appearance.BackColor = Color.DarkOrange;
                e.Appearance.BackColor2 = Color.White;
            }
            else
            {
                e.Appearance.BackColor = Color.Aqua;
                e.Appearance.BackColor2 = Color.White;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            LoadMess_HTD();
            check_mail_Read.Enabled = check_mail_UnRead.Enabled = true;
            //MessageBox.Show("ABC");
        }

        private void btn_loadMail_D_Click(object sender, EventArgs e)
        {
            LoadMess_D();
        }

        private async void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                btn_repplyMail.Enabled = false;
                lb_mail_From_HTD.Text = lb_mail_Time_HTD.Text = lb_mail_Content.Text = "N/A";
                return;
            }
            MESSANGE mess = null;
            mess = ListMess_HTD[gridView1.FocusedRowHandle];
            lb_mail_From_HTD.Text = mess.Id_From;
            foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            {
                if (c.Value.ToString().ToUpper() == mess.Id_From.ToUpper())
                    lb_mail_From_HTD.Text = c.Description;
            }
            lb_mail_Time_HTD.Text = mess.Time.ToString("hh:mm dd/MM/yyyy");
            lb_mail_Content.Text = mess.Mess;
            btn_repplyMail.Enabled = true;
            if (mess.Seen != 1)
            {
                await UpdateMail(mess.Id);
                int i = gridView1.FocusedRowHandle;
                ListMess_HTD[i].Seen = 1;
                gridView1.FocusedRowHandle = i;
            }
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            if (gridView2.FocusedRowHandle < 0)
            {
                lb_mail_From_D.Text = lb_mail_Time_D.Text = lb_mail_Content_D.Text = "N/A";
                return;
            }
            MESSANGE mess = null;
            mess = ListMess_D[gridView2.FocusedRowHandle];
            lb_mail_From_D1.Text = mess.Id_To;
            foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            {
                if (c.Value.ToString() == mess.Id_To)
                    lb_mail_From_D1.Text = c.Description + "(" + mess.Id_To + ")";
            }
            lb_mail_Time_D.Text = mess.Time.ToString("hh:mm dd/MM/yyyy");
            lb_mail_Content_D.Text = mess.Mess;
        }

        private void cbb_main_ListTo_EditValueChanged(object sender, EventArgs e)
        {
            List_Id_Mail = new List<string>();
            String text = "";
            foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            {
                if (c.CheckState == CheckState.Checked)
                {
                    List_Id_Mail.Add(c.Value + "");
                    text = text + c.Value + ", ";
                }
            }
            if (text == "") lb_mail_To_NewMail.Text = "N/A";
            else lb_mail_To_NewMail.Text = text;
            lb_mail_Time_NewMail.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        private void btn_repplyMail_Click(object sender, EventArgs e)
        {
            if (lb_mail_From_HTD.Text == "N/A")
            {
                MessageBox.Show("Chưa chọn thư nào để trả lời.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ClearNewMail();
            xtraTabPage6.Show();
            lb_mail_To_NewMail.Text = lb_mail_From_HTD.Text;
            foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            {
                MessageBox.Show("/" + c.Description + "/" + lb_mail_To_NewMail.Text + "/");
                if (c.Description == lb_mail_To_NewMail.Text)
                    c.CheckState = CheckState.Checked;
            }
            lb_mail_Time_NewMail.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        private void btn_mail_Clear_NewMail_Click(object sender, EventArgs e)
        {
            ClearNewMail();
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            if (lb_mail_To_NewMail.Text == "N/A")
            {
                MessageBox.Show("Chưa có ai trong danh sách gửi.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_mail_Content_NewMail.Text.Length < 10)
            {
                MessageBox.Show("Nội dung thư quá ngăn.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (List_Id_Mail.Count == 0) List_Id_Mail.Add(lb_mail_To_NewMail.Text);
            List<MESSANGE> send = new List<MESSANGE>();
            foreach (String id in List_Id_Mail)
            {
                MESSANGE m = new MESSANGE();
                m.Id_From = user.Id_User;
                m.Id_To = id;
                m.Mess = txt_mail_Content_NewMail.Text;
                m.Time = DateTime.Now;
                send.Add(m);
                MessageBox.Show(m.Id_From + "/" + m.Id_To + "/" + m.Mess + "/" + m.Seen);
            }
            int i = await AddNewMail(send);
            MessageBox.Show("Gửi " + i + " thư thành công.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearNewMail();
        }

        private void btn_loadnewMail_Click(object sender, EventArgs e)
        {
            xtraTabPage4.Show();
        }

        private void Home_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                ShowNotifyIcon("Project Manager", "Chương trình vẫn chạy ở khay hệ thống.", 10);
            }
        }


        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private async void btn_loadReport_Click(object sender, EventArgs e)
        {
            List<WORK> li_work = new List<WORK>();
            li_work = await GetAllWork(user.Id_User);
            DataTable dt = new DataTable();
            dt.Columns.Add("Start");
            dt.Columns.Add("Stop");
            dt.Columns.Add("Delta");
            int couttime = 0, coutwwork = 0;
            foreach (WORK w in li_work)
            {
                if (w.TimeEnd != null)
                {
                    TimeSpan time = (w.TimeEnd.Value - w.TimeStart);
                    int t = (int)time.TotalMinutes;
                    couttime = couttime + t;
                    coutwwork++;
                    dt.Rows.Add(w.TimeStart, w.TimeEnd, t + "phút");
                }
            }
            lb_report_Count.Text = coutwwork + "";
            lb_report_CountTime.Text = couttime / 60 + "giờ, " + couttime % 60 + " phút.";
            grid_Report.DataSource = dt;
            grid_Report.Refresh();
        }


        private async void btn_CreatReport_Click(object sender, EventArgs e)
        {
            List<WORK> li_work = new List<WORK>();
            li_work = await GetAllWork(user.Id_User);
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("TimeStart");
            dt.Columns.Add("TimeEnd");
            dt.Columns.Add("Id_User");
            int couttime = 0, coutwwork = 0;
            foreach (WORK w in li_work)
            {
                if (w.TimeEnd != null)
                {
                    TimeSpan time = (w.TimeEnd.Value - w.TimeStart);
                    int t = (int)time.TotalMinutes;
                    couttime = couttime + t;
                    coutwwork++;
                    dt.Rows.Add(coutwwork, w.TimeStart.ToString("hh:mm dd/MM/yyyy"), w.TimeEnd.Value.ToString("hh:mm dd/MM/yyyy"), t + "phút");
                }
            }
            XtraReport1 x = new XtraReport1();
            x.lb_Name.Text = user.Name;
            x.lb_UserId.Text = user.Id_User;
            x.lb_Register.Text = user.Regiter_Day.Value.ToString("dd/MM/yyyy");
            x.lb_CountWork.Text = coutwwork.ToString();
            x.lb_SumTimeWork.Text = couttime.ToString();
            x.DataSource = dt;
            x.ShowPreviewDialog();
        }

        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            if (Working)
                MessageBox.Show("Vui lòng dừng phiên làm việc trước khi Đăng Xuất.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Login l = new Login();
                l.Show();
                this.Hide();
            }

        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void check_mail_UnRead_CheckedChanged(object sender, EventArgs e)
        {
            LoadMess_HTD();
        }
    }
}