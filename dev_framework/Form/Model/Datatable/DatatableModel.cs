using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Form.Model.Datatable
{
    public class DatatableModel
    {
        public object Data { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public int Start { get; set; }
        public int PageSize { get; set; }
    }
    public class DatatableViewModel
    {
        public string Columns { get; set; }
        public string Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DatatableViewModel(int start, int length)
        {
            Start = start;
            Length = length;
        }
        public DatatableViewModel(int start, int length, string columns, string order)
        {
            Start = start;
            Length = length;
            Columns = columns;
            Order = order;
        }
        public DatatableViewModel()
        {
            Start = 0;
            Length = 10;
        }
    }
}
