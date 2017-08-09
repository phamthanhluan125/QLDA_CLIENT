using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    [Serializable]
    public class Project
    {
        public int id { set; get; }
        public string name { set; get; }
        public string info { set; get; }
        public DateTime start_date { set; get; }
        public DateTime deadline { set; get; }
        public Nullable<DateTime> finish_date { set; get; }
        public string status { set; get; }
        public int admin_id { set; get; }

    }
}
