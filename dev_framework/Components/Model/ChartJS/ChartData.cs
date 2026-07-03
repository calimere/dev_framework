using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui.utils.Components.Model.ChartJS
{
    public class ChartData
    {
        public string[] Labels { get; set; }
        public List<ChartDataset> Datasets { get; set; }
    }
}
