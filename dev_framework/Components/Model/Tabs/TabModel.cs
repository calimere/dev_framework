using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components.Model.Tabs
{
    public class TabModel : Model
    {
        public string TabNavbarId { get { return $"{Id}-tab"; } }
        public string ContentId { get { return $"{Id}-content"; } }
        public bool IsActive { get; set; }
    }
}
