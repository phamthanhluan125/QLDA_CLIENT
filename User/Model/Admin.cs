using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    [Serializable]
    class Admin
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime birthday { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }
    }
}
