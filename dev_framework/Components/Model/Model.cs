using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui.utils.Components.Model
{
    public abstract class Model
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string[] CssClass { get; set; }
        public Dictionary<string, string> DataAttribute { get; set; }

        public Model()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
        }
    }

}
