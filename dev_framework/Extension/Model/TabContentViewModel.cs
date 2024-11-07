using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Extension.Model
{
    public abstract class TabContentViewModel : TabViewModel
    {
        public TabContentModel[] TabContentModels { get; set; }
    }
}
