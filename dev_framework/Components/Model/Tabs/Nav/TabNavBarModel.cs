using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components.Model.Tabs.Nav
{
    public class TabNavBarModel : TabModel
    {
        public string[] IconClass { get; set; }

        public TabNavBarModel()
        {
        }

        public TabNavBarModel(TabModel tabModel, string[] iconClass)
        {
            Id = tabModel.Id;
            Title = tabModel.Title;
            IsActive = tabModel.IsActive;
            CssClass = tabModel.CssClass;
            DataAttribute = tabModel.DataAttribute;
            IconClass = iconClass;
        }
    }
}
