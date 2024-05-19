using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Manager.Model
{
    public class ElasticSearchConfig
    {
        public ElasticSearchConfig() { }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Index { get; set; }
    }
}
