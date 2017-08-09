using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    [Serializable]
    public class TaskManager
    {
        public int id { set; get; }
        public string name { get; set; }
        public string info { get; set; }
        public DateTime start_date { get; set; }
        public DateTime deadline { get; set; }
        public Nullable<DateTime> finish_date { get; set; }
        public string status { get; set; }
        public DateTime created_at { set; get; }
    }
}
