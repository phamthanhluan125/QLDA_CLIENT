using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    class ScreenShotPicture
    {
        public String Id_User { get; set; }
        public String Name { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public String Link { get; set; }
        public byte[] Picture { get; set; }
    }
}
