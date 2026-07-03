using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui.utils.Extension.Model
{
    public abstract class TabContentViewModel : TabViewModel
    {
        public TabContentModel[] TabContentModels { get; set; }
    }
}
