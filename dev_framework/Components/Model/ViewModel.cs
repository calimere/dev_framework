using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components.Model
{
    public abstract class ViewModel
    {
        public string Id { get; set; }

        public string[] CssClass { get; set; }

        public ViewModel()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
