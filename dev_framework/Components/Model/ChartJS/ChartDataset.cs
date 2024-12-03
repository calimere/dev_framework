using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components.Model.ChartJS
{
    public class ChartDataset
    {
        public string Label { get; set; }
        public double[] Data { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool Fill { get; set; } = false;
    }
}
