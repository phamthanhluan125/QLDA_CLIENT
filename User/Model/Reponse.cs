using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace User.Model
{
    [Serializable]
    public class Reponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T content { get; set; }
    }
}
