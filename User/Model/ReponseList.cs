using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Model
{
    [Serializable]
    public class ReponseList<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<T> content { get; set; }
    }
}
