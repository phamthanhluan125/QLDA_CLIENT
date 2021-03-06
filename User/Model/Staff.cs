﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    [Serializable]
    public class Staff
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int time_scr { get; set; }
        public DateTime birthday { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }
        public string authentication_token { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public Role role { get; set; }
    }

    public class Role
    {
        public string name { get; set; }
    }
}
