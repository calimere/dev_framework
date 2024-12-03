using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components.Model.Datatble
{
    public class Column
    {
        public Column()
        {
            AutoWidth = false;
            Orderable = false;
            TargetBlank = false;
            IsKey = false;
            ETypeColumn = ETypeColumn.Normal;
        }
        public ETypeColumn ETypeColumn { get; set; }
        public bool Orderable { get; set; }
        public string Data { get; set; }
        public bool AutoWidth { get; set; }
        public string Render { get; set; }
        public string[] CssClass { get; set; }
        public bool TargetBlank { get; set; }
        public bool IsKey { get; set; }
    }
    public enum ETypeColumn
    {
        Normal,
        Link,
        Custom
    }
}
