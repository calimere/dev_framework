using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components.Model.Tabs
{
    public abstract class TabViewModel : ViewModel
    {
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
        
    }
}
