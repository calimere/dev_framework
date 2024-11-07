using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Extension.Model
{
    public class TabContentModel : TabModel
    {
        public string View { get; set; }
        public object Model { get; set; }

        public TabContentModel()
        {
        }

        public TabContentModel(TabModel tabModel, string view, object model)
        {
            Id = tabModel.Id;
            Title = tabModel.Title;
            IsActive = tabModel.IsActive;
            CssClass = tabModel.CssClass;
            DataAttribute = tabModel.DataAttribute;
            View = view;
            Model = model;
        }
    }
}
