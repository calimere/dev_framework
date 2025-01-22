using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Form.Model.Datatable
{
    public class DatatableSearch
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }

    public class DataTableColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public DatatableSearch search { get; set; }
    }

    public class DatatableOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class DatatableFilter
    {
        public int Length { get; set; }
        public int Start { get; set; }
        public string OrderBy { get; set; }
        public string OrderDir { get; set; }
    }
}
