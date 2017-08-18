using System;
using System.Text;
using System.Threading.Tasks;
using User.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace User
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public static HttpClient client = new HttpClient();
        public static String URL = "http://192.168.1.187:3000/v1/";
		public Login()
        {
            InitializeComponent();
        }

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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string reponse = GET("authen_user_tokens?user_email=" + txt_id.Text + "&password=" + txt_pass.Text);
            Reponse<Staff> r = JsonConvert.DeserializeObject<Reponse<Staff>>(reponse);
            if (r.status == 200)
            {
                if (r.content.status == "active")
                {
                    Home home = new Home();
                    home.set_staff(r.content);
                    MessageBox.Show("Đăng nhập thành công", "Thông Báo");
                    home.Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Tài khoản của bạn đang bị khóa. Vui lòng liên hệ với Manager của bạn.", "Thông Báo");
            }
            else
                MessageBox.Show(r.message, "Lỗi");
        }

        private void Login_Load(object sender, EventArgs e)
        {
            pnl_loadding.Hide();
        }
    }
}