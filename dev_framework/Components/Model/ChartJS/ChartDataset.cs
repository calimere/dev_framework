using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui.utils.Components.Model.ChartJS
{
    public class ChartDataset
    {
        public string Label { get; set; }
        public object[] Data { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool Fill { get; set; } = false;
    }
}
