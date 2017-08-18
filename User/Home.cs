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
using System.Net;
using Newtonsoft.Json;

namespace User
{
    public partial class Home : DevExpress.XtraEditors.XtraForm
    {
        public static Staff staff;
        bool Working = false;
        Project project;
        Manager manager;
        Timesheet timesheet;
        //List<MESSANGE> ListMess_HTD, ListMess_D;
        List<TaskManager> tasks;
        //List<MANAGER_TASK> ListManagerTask;
        List<string> List_Id_Mail;
		String URL = Login.URL;
        Thread couttime, SCR;
        private Home home;
        public Home()
        {
            InitializeComponent();
            //Thread a = new Thread(new ThreadStart(LoadNewMail));
            //a.Start();
        }

        public Home(Home home)
        {
            this.home = home;
        }

        public void set_staff(Staff s)
        {
            staff = s;
        }

        private void Home_Load(object sender, EventArgs e)
        {
            Loadding.Show();
            LoadGroup();
            lb_today.Text = DateTime.Today.ToString("dd/MM/yyyy");
            //await LoadCombobox();
            //await LoadComboboxSendEmail();
            Loadding.PageVisible = false;
        }
        //CALL API
        string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                }
                throw;
            }
        }

        string POST(string uri, string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + uri);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        string PUT(string uri, string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + uri);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
        //THREAD
        delegate void SetTextCallback(string text);
        private async void Threa_SRC()
        {
            while (Working)
            {
                if (staff.time_scr > 0)
                {
                    Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                    Graphics graphics = Graphics.FromImage(bitmap as Image);
                    graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    Image o = (Image)bitmap;
                    ImageConverter _imageConverter = new ImageConverter();
                    byte[] xByte = (byte[])_imageConverter.ConvertTo(o, typeof(byte[]));
                    string base64String = Convert.ToBase64String(xByte);
                    string uri = "screenshots?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
                    string json = "{\"timesheet_id\":\"" + timesheet.id + "\",\"image\":\"data:image/png;base64,(" + base64String + ")\"}";
                    string reponse = POST(uri, json);
                    Reponse<Screenshoot> r = JsonConvert.DeserializeObject<Reponse<Screenshoot>>(reponse);
                    Thread.Sleep((int)staff.time_scr * 10000);
                }
            }
        }
        private void SetText(string text)
        {

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
            }
        }

        //LOAD

        private void LoadGroup()
        {
            //Thong tin ca nhan
            lb_id.Text = staff.name;
            lb_name.Text = staff.name;
            lb_age.Text = staff.birthday.ToString("dd/MM/yyyy");
            lb_rigDay.Text = staff.created_at.ToString("dd/MM/yyyy");
            lb_role.Text = staff.role.name;
            if (staff.gender)
                lb_gender.Text = "Nữ";
            else
                lb_gender.Text = "Nam";

            load_project();
            load_tasks();
            load_manager();
        }

        private void load_manager()
        {
            string uri = "managers?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
            string reponse = GET(uri);
            Reponse<Manager> r = JsonConvert.DeserializeObject<Reponse<Manager>>(reponse);
            if (r.status == 200)
            {
                manager = r.content;

                lb_manager_Name.Text = manager.name;
                lb_manager_address.Text = manager.address;
                lb_manager_Age.Text = manager.birthday.ToString("dd/MM/yyyy");
                if (manager.gender)
                    lb_manager_gender.Text = "Nữ";
                else
                    lb_manager_gender.Text = "Nam";
            }
            else
            {
                MessageBox.Show(r.message, "Lỗi");
            }
        }

        private void load_project()
        {
            string uri = "projects?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
            string reponse = GET(uri);
            Reponse<Project> r = JsonConvert.DeserializeObject<Reponse<Project>>(reponse);
            if (r.status == 200)
            {
                project = r.content;

                lb_nameProject.Text = project.name;
                lb_dateStart.Text = project.start_date.ToString("dd/MM/yyyy");
                lb_dateEnd.Text = project.deadline.ToString("dd/MM/yyyy");
                load_status(project.status, project.finish_date, lb_statusNow);
            }
            else
            {
                MessageBox.Show(r.message, "Lỗi");
            }
        }

        private void load_status(string status, Nullable<DateTime> finish, LabelControl label)
        {
            if (status == "pending")
            {
                label.Text = "CHƯA BẮT ĐẦU";
                label.ForeColor = Color.Violet;
            }
            else if (status == "running")
            {
                label.Text = " ĐANG DIỄN RA";
                label.ForeColor = Color.Green;
            }
            else if (status == "delay")
            {
                label.Text = "CHẬM TIẾN ĐỘ";
                label.ForeColor = Color.Orange;
            }
            else if (status == "finish")
            {
                label.Text = "ĐÃ HOÀN THÀNH " + finish;
                label.ForeColor = Color.Blue;
            }
            else
            {
                label.Text = "THẤT BẠI " + finish;
                label.ForeColor = Color.Red;
            }
        }

        private void load_tasks()
        {
            string uri = "tasks?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
            string reponse = GET(uri);
            ReponseList<TaskManager> r = JsonConvert.DeserializeObject<ReponseList<TaskManager>>(reponse);
            if (r.status == 200)
            {
                tasks = r.content;
                int pending = 0, running = 0, delay = 0, finish = 0, cancel = 0;
                foreach (TaskManager t in tasks)
                {
                    if (t.status == "pending")
                        pending++;
                    else if (t.status == "running")
                        running++;
                    else if (t.status == "delay")
                        delay++;
                    else if (t.status == "finish")
                        finish++;
                    else if (t.status == "cancel")
                        cancel++;
                }
                lb_numOfTask.Text = tasks.Count.ToString();
                lb_numOfFinshTask.Text = finish.ToString();
                lb_numOfTaskpending.Text = pending.ToString();
                lb_numOfTaskrunning.Text = running.ToString();
                lb_numOfTaskdelay.Text = delay.ToString();
                lb_numOfTaskcancel.Text = cancel.ToString();
                LoadCombobox();
            }
            else
            {
                MessageBox.Show(r.message, "Lỗi");
            }
        }

        private void LoadCombobox()
        {

            cbb_Task.DataSource = tasks;
            cbb_Task.ValueMember = "id";
            cbb_Task.DisplayMember = "name";
            cbb_Task.Refresh();
        }

        //       private async Task LoadMess_HTD()
        //       {
        //           ListMess_HTD = new List<MESSANGE>();
        //           List<MESSANGE> ListM = new List<MESSANGE>();
        //           ListM = await GetMail(user.Id_User, 2);
        //           if (check_mail_UnRead.Checked)
        //           {
        //               foreach (MESSANGE m in ListM)
        //               {
        //                   if (m.Seen == 1) ListMess_HTD.Add(m);
        //               }
        //           }
        //           if (check_mail_Read.Checked)
        //           {
        //               foreach (MESSANGE m in ListM)
        //               {
        //                   if (m.Seen == 0) ListMess_HTD.Add(m);
        //               }
        //           }
        //           gridControl1.DataSource = ListMess_HTD;
        //           gridControl1.Refresh();
        //       }
        //       private async Task LoadMess_D()
        //       {
        //           ListMess_D = new List<MESSANGE>();
        //           ListMess_D = await GetMail(user.Id_User, 1);
        //           gridControl2.DataSource = ListMess_D;
        //           gridControl2.Refresh();
        //       }
        //       private async Task LoadComboboxSendEmail()
        //       {
        //           List<USER> listUser = await GetAllUserByIdProject(p.Id_Project);
        //           List<ROLE> listRole = await GetAllRoleByIdManaer(manager.Id_Admin);
        //           DataTable dt = new DataTable();
        //           dt.Columns.Add("Id");
        //           dt.Columns.Add("Name");
        //           dt.Rows.Add(manager.Id_Admin, "Manager: " + manager.Name + "(" + manager.Id_Admin + ")");
        //           foreach (USER us in listUser)
        //           {
        //               String id = us.Id_User;
        //               String name = us.Name;
        //               foreach (ROLE rl in listRole)
        //               {
        //                   if (us.Id_Role == rl.Id_Role)
        //                       if (id == user.Id_User)
        //                           lb_role.Text = rl.Name;
        //                       else
        //                           name = rl.Name + ": " + name + "(" + us.Id_User + ")";
        //               }
        //               if (id != user.Id_User)
        //                   dt.Rows.Add(id, name);
        //           }
        //           cbb_main_ListTo.Properties.DataSource = dt;
        //           cbb_main_ListTo.Properties.ValueMember = "Id";
        //           cbb_main_ListTo.Properties.DisplayMember = "Name";
        //           cbb_main_ListTo.RefreshEditValue();
        //       }
        //       private void ClearNewMail()
        //       {
        //           foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
        //               c.CheckState = CheckState.Unchecked;
        //           txt_mail_Content_NewMail.Text = "";
        //       }

        //--------------EVENT-------------------
        private async void btn_workStart_Click_1(object sender, EventArgs e)
        {

            string uri = "timesheets?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
            string reponse = POST(uri, "{}");
            Reponse<Timesheet> r = JsonConvert.DeserializeObject<Reponse<Timesheet>>(reponse);
            if (r.status == 200)
            {
                MessageBox.Show(r.message, "Thông Báo");
                timesheet = r.content;
                lb_timeStart.Text = timesheet.start.Hour + " : " + timesheet.start.Minute;
                couttime = new Thread(new ThreadStart(CountTime));
                couttime.Start();
                btn_workStop.Enabled = true;
                btn_workStart.Enabled = false;
                Working = true;
                SCR = new Thread(new ThreadStart(Threa_SRC));
                SCR.Start();
            }
            else
            {
                MessageBox.Show(r.message, "Lỗi");
            }
        }

        private async void btn_workStop_Click(object sender, EventArgs e)
        {
           
            string uri = "timesheets/" + timesheet.id + "?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
            string reponse = PUT(uri, "{}");
            Reponse<Timesheet> r = JsonConvert.DeserializeObject<Reponse<Timesheet>>(reponse);
            if (r.status == 200)
            {
                couttime.Abort();
                SCR.Abort();
                btn_workStart.Enabled = true;
                btn_workStop.Enabled = false;
                lb_timeStart.Text = "HH : MM";
                lb_timeworking.Text = "0 : 0 : 0";
                Working = false;
                MessageBox.Show("Phiên làm viêc của bạn từ: " + r.content.start.ToString("hh:mm dd/MM/yyyy") + " - " + r.content.end.ToString("hh:mm dd/MM/yyyy"), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(r.message, "Lỗi");
            }
        }

        private void cbb_Task_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_task = cbb_Task.SelectedValue.ToString();
            foreach (TaskManager t in tasks)
            {
                if (t.id.ToString() == id_task)
                {
                    lb_Tast_Name.Text = t.name;
                    lb_Tast_Note.Text = t.info;
                    load_status(t.status, t.finish_date, lb_Task_StatusNow);
                    lb_Task_ReciveDay.Text = t.start_date.ToString("dd/MM/yyyy");
                    lb_Task_DeadlineDay.Text = t.deadline.ToString("dd/MM/yyyy");

                }
            }
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //    // MessageBox.Show(e.RowHandle + "");
            //    if (e.RowHandle >= 0 && ListMess_HTD[e.RowHandle].Seen == 0)
            //    {
            //        e.Appearance.BackColor = Color.DarkOrange;
            //        e.Appearance.BackColor2 = Color.White;
            //    }
            //    else
            //    {
            //        e.Appearance.BackColor = Color.Aqua;
            //        e.Appearance.BackColor2 = Color.White;
            //    }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //    LoadMess_HTD();
            //    check_mail_Read.Enabled = check_mail_UnRead.Enabled = true;
            //    //MessageBox.Show("ABC");
        }

        private void btn_loadMail_D_Click(object sender, EventArgs e)
        {
            //    LoadMess_D();
        }

        private async void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //    if (gridView1.FocusedRowHandle < 0)
            //    {
            //        btn_repplyMail.Enabled = false;
            //        lb_mail_From_HTD.Text = lb_mail_Time_HTD.Text = lb_mail_Content.Text = "N/A";
            //        return;
            //    }
            //    MESSANGE mess = null;
            //    mess = ListMess_HTD[gridView1.FocusedRowHandle];
            //    lb_mail_From_HTD.Text = mess.Id_From;
            //    foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            //    {
            //        if (c.Value.ToString().ToUpper() == mess.Id_From.ToUpper())
            //            lb_mail_From_HTD.Text = c.Description;
            //    }
            //    lb_mail_Time_HTD.Text = mess.Time.ToString("hh:mm dd/MM/yyyy");
            //    lb_mail_Content.Text = mess.Mess;
            //    btn_repplyMail.Enabled = true;
            //    if (mess.Seen != 1)
            //    {
            //        await UpdateMail(mess.Id);
            //        int i = gridView1.FocusedRowHandle;
            //        ListMess_HTD[i].Seen = 1;
            //        gridView1.FocusedRowHandle = i;
            //    }
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            //    if (gridView2.FocusedRowHandle < 0)
            //    {
            //        lb_mail_From_D.Text = lb_mail_Time_D.Text = lb_mail_Content_D.Text = "N/A";
            //        return;
            //    }
            //    MESSANGE mess = null;
            //    mess = ListMess_D[gridView2.FocusedRowHandle];
            //    lb_mail_From_D1.Text = mess.Id_To;
            //    foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            //    {
            //        if (c.Value.ToString() == mess.Id_To)
            //            lb_mail_From_D1.Text = c.Description + "(" + mess.Id_To + ")";
            //    }
            //    lb_mail_Time_D.Text = mess.Time.ToString("hh:mm dd/MM/yyyy");
            //    lb_mail_Content_D.Text = mess.Mess;
        }

        private void cbb_main_ListTo_EditValueChanged(object sender, EventArgs e)
        {
            //    List_Id_Mail = new List<string>();
            //    String text = "";
            //    foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            //    {
            //        if (c.CheckState == CheckState.Checked)
            //        {
            //            List_Id_Mail.Add(c.Value + "");
            //            text = text + c.Value + ", ";
            //        }
            //    }
            //    if (text == "") lb_mail_To_NewMail.Text = "N/A";
            //    else lb_mail_To_NewMail.Text = text;
            //    lb_mail_Time_NewMail.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        private void btn_repplyMail_Click(object sender, EventArgs e)
        {
            //    if (lb_mail_From_HTD.Text == "N/A")
            //    {
            //        MessageBox.Show("Chưa chọn thư nào để trả lời.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    ClearNewMail();
            //    xtraTabPage6.Show();
            //    lb_mail_To_NewMail.Text = lb_mail_From_HTD.Text;
            //    foreach (CheckedListBoxItem c in cbb_main_ListTo.Properties.Items)
            //    {
            //        MessageBox.Show("/" + c.Description + "/" + lb_mail_To_NewMail.Text + "/");
            //        if (c.Description == lb_mail_To_NewMail.Text)
            //            c.CheckState = CheckState.Checked;
            //    }
            //    lb_mail_Time_NewMail.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        private void btn_mail_Clear_NewMail_Click(object sender, EventArgs e)
        {
            //    ClearNewMail();
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            //    if (lb_mail_To_NewMail.Text == "N/A")
            //    {
            //        MessageBox.Show("Chưa có ai trong danh sách gửi.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    if (txt_mail_Content_NewMail.Text.Length < 10)
            //    {
            //        MessageBox.Show("Nội dung thư quá ngăn.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    if (List_Id_Mail.Count == 0) List_Id_Mail.Add(lb_mail_To_NewMail.Text);
            //    List<MESSANGE> send = new List<MESSANGE>();
            //    foreach (String id in List_Id_Mail)
            //    {
            //        MESSANGE m = new MESSANGE();
            //        m.Id_From = user.Id_User;
            //        m.Id_To = id;
            //        m.Mess = txt_mail_Content_NewMail.Text;
            //        m.Time = DateTime.Now;
            //        send.Add(m);
            //        MessageBox.Show(m.Id_From + "/" + m.Id_To + "/" + m.Mess + "/" + m.Seen);
            //    }
            //    int i = await AddNewMail(send);
            //    MessageBox.Show("Gửi " + i + " thư thành công.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    ClearNewMail();
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

        private void ShowNotifyIcon(String Titte, String Text, int TimeShow)
        {
            notifyIcon1.BalloonTipTitle = Titte;
            notifyIcon1.BalloonTipText = Text;
            //Hiện message
            notifyIcon1.ShowBalloonTip(TimeShow * 1000);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void btn_loadReport_Click(object sender, EventArgs e)
        {
			string uri = "timesheets?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
			string reponse = GET(uri);
			ReponseList<Timesheet> r = JsonConvert.DeserializeObject<ReponseList<Timesheet>>(reponse);
			if (r.status == 200)
			{
				List<Timesheet> timesheets = r.content;
				DataTable dt = new DataTable();
				dt.Columns.Add("Start");
				dt.Columns.Add("Stop");
				dt.Columns.Add("Delta");
				int couttime = 0, coutwwork = 0;
				foreach (Timesheet w in timesheets)
				{
					if (w.end != null)
					{
						TimeSpan time = (w.end - w.start);
						int t = (int)time.TotalMinutes;
						couttime = couttime + t;
						coutwwork++;
						dt.Rows.Add(w.start, w.end, t + "phút");
					}
				}
				lb_report_Count.Text = coutwwork + "";
				lb_report_CountTime.Text = couttime / 60 + "giờ, " + couttime % 60 + " phút.";
				grid_Report.DataSource = dt;
				grid_Report.Refresh();
			}
			else
			{
				MessageBox.Show(r.message, "Lỗi");
			}
		}


        private void btn_CreatReport_Click(object sender, EventArgs e)
        {
			string uri = "timesheets?user_email=" + staff.email + "&user_token=" + staff.authentication_token;
			string reponse = GET(uri);
			ReponseList<Timesheet> r = JsonConvert.DeserializeObject<ReponseList<Timesheet>>(reponse);
			if (r.status == 200)
			{
				List<Timesheet> timesheets = r.content;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("TimeStart");
				dt.Columns.Add("TimeEnd");
				dt.Columns.Add("Id_User");
				int couttime = 0, coutwwork = 0;
				foreach (Timesheet w in timesheets)
				{
					if (w.end != null)
					{
						TimeSpan time = (w.end - w.start);
						int t = (int)time.TotalMinutes;
						couttime = couttime + t;
						coutwwork++;
						dt.Rows.Add(coutwwork, w.start.ToString("hh:mm dd/MM/yyyy"), w.end.ToString("hh:mm dd/MM/yyyy"), t + "phút");
					}
				}
				XtraReport1 x = new XtraReport1();
				x.lb_Name.Text = staff.name;
				x.lb_UserId.Text = staff.email;
				x.lb_Register.Text = staff.created_at.ToString("dd/MM/yyyy");
				x.lb_CountWork.Text = coutwwork.ToString();
				x.lb_SumTimeWork.Text = couttime.ToString();
				x.DataSource = dt;
				x.ShowPreviewDialog();
			}
			else
			{
				MessageBox.Show(r.message, "Lỗi");
			}
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
            //    LoadMess_HTD();
        }
    }
}