using Newtonsoft.Json;
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
        public DataTableColumn[] Columns { get; set; }
        public DatatableOrder[] Order { get; set; }
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

            try { Columns = JsonConvert.DeserializeObject<DataTableColumn[]>(columns).Where(m=>!string.IsNullOrEmpty(m.name)).ToArray() ?? new DataTableColumn[0]; }
            catch (Exception) { Columns = new DataTableColumn[0]; }

            try { Order = JsonConvert.DeserializeObject<DatatableOrder[]>(order) ?? new DatatableOrder[0]; }
            catch (Exception) { Order = new DatatableOrder[0]; }
        }
        public DatatableViewModel()
        {
            Start = 0;
            Length = 10;
        }
    }
}
