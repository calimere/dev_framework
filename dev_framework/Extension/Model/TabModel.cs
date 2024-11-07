using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Extension.Model
{
    public class TabModel
    {
        public string Id { get; set; }
        public string TabNavbarId
        {
            get
            {
                return $"{Id}-tab";
            }
        }
        public string ContentId
        {
            get
            {
                return $"{Id}-content";
            }
        }
        public string Title { get; set; }
        public string[] CssClass { get; set; }
        public Dictionary<string, string> DataAttribute { get; set; }
        public bool IsActive { get; set; }
    }
}
