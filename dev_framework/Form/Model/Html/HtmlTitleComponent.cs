using System;
using System.Collections.Generic;
using System.Text;

namespace dev_framework.Form.Model.Html
{
    public class HtmlTitleComponent
    {
        public HtmlTitleType HtmlTitleType { get; set; }
        public string CssClass { get; set; }
        public string CssId { get; set; }
        public string Text { get; set; }
    }

    public enum HtmlTitleType
    {
        h1,
        h2,
        h3,
        h4,
        h5
    }

    public class HtmlSection
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public HtmlTitleComponent Title { get; set; }
        public string Html { get; set; }
    }
}
