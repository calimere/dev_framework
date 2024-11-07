using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Extension.Model
{
    public abstract class TabViewModel
    {
        public string Id { private get; set; }
        public string NavBarId
        {
            get
            {
                return $"{Id}-tabs";
            }
        }
        public string ContentId
        {
            get
            {
                return $"{Id}-contents";
            }
        }
        public string[] CssClass { get; set; }
    }
}
