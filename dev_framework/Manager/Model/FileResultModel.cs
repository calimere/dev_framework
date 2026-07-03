using ui.utils.Manager.Model;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui.utils.Manager.Model
{
    public class FileResultModel
    {
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IOMessage IOMessage { get; set; }
    }

    
}
