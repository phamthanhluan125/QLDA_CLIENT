using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    [Serializable]
    public class Timesheet
    {
        public int id { set; get; }
        public DateTime start { set; get; }
        public DateTime end { get; set; }
        public int user_id { set; get; }
        public int project_id { set; get; }


    }
}
