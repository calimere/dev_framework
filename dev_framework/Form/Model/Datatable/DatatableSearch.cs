using Microsoft.AspNetCore.Mvc.Rendering;
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
        // côté js
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public DatatableSearch search { get; set; }

        //côté razor / C#
        public IEnumerable<SelectListItem> Options { get; set; }
        public ETypeColumn ETypeColumn { get; set; }
        public bool Orderable { get; set; }
        public bool AutoWidth { get; set; }
        public string Render { get; set; }
        public string[] CssClass { get; set; }
        public bool TargetBlank { get; set; }
        public bool IsKey { get; set; }
        public string Title { get; set; }

        public DataTableColumn()
        {
            AutoWidth = false;
            Orderable = false;
            TargetBlank = false;
            IsKey = false;
            ETypeColumn = ETypeColumn.Normal;
        }
    }
    public enum ETypeColumn
    {
        Normal,
        Link,
        Custom
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

        public List<KeyValuePair<string, object>> SearchTerms { get; set; }
        public DatatableOrder[] DatatableOrders { get; set; }
    }
}
