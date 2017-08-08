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
        String URL = "https://qlda-luan.herokuapp.com/v1/";
        public Login()
        {
            InitializeComponent();
        }
        static async Task RunAsync()
        {
            // New code:
            client.BaseAddress = new Uri("http://localhost:55268/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.ReadLine();
        }
        string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://qlda-luan.herokuapp.com/v1/" + url);
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
                    // log errorText
                }
                throw;
            }
        }

        private async Task<USER> GetUSer(string id)
        {
            USER u = new USER();
            HttpResponseMessage response = await client.GetAsync(URL + "USERs/GetUserById/" + id);
            if (response.IsSuccessStatusCode)
            {
                u = await response.Content.ReadAsAsync<USER>();
            }
            return u;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string reponse = GET("authen_user_tokens?user_email=" + txt_id.Text + "&password=" + txt_pass.Text);
            Reponse<Users> r = JsonConvert.DeserializeObject<Reponse<Users>>(reponse);
            if (r.status == 404)
                MessageBox.Show(r.message);
            else
            {
                MessageBox.Show(r.content.name);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            pnl_loadding.Hide();
        }
    }
}