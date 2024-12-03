using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dev_framework.Components
{
    public static class DatatableExtensions
    {
        public static Datatable Datatable(this IHtmlHelper htmlHelper)
        {
            return new Datatable();
        }
        public static IHtmlContent IncludeDatatableScript()
        {
            return DatatableScript.Get();
        }
    }

    public class DatatableScript
    {
        public DatatableScript() { }

        public static IHtmlContent Get()
        {
            return new HtmlString($@"<link href=""~/lib/datatables/datatables.css"" rel=""stylesheet"" />
                    <script type=""text/javascript"" src=""~/lib/datatables/datatables.min.js""></script>
                    <script type=""text/javascript"" asp-append-version=""true"" src=""//cdn.datatables.net/buttons/1.2.1/js/buttons.print.min.js""></script>
                    <script type=""text/javascript"" src=""~/lib/csk-libs/datatable.extension.js""></script>");
        }
    }

    public class Datatable : IDisposable
    {
        private readonly TextWriter _writer;
        public Datatable() { }


        public enum ETypeColumn
        {
            Normal,
            Link,
            Custom
        }
        public class Column
        {
            public ETypeColumn ETypeColumn { get; set; }
            public bool Orderable { get; set; }
            public string Data { get; set; }
            public bool AutoWidth { get; set; }
            public string Render { get; set; }
            public string[] CssClass { get; set; }
            public bool TargetBlank { get; set; }
        }

        private string RenderColumn(Column column)
        {
            switch (column.ETypeColumn)
            {
                case ETypeColumn.Normal:
                    return $@"retour.push({{ ""data"": ""{column.Data}"", ""autoWidth"": {column.AutoWidth}, orderable: {column.Orderable} }})";
                case ETypeColumn.Link:
                    return $@"retour.push({{ ""data"": ""{column.Data}"", ""autoWidth"": {column.AutoWidth}, orderable: {column.Orderable} }})";
                case ETypeColumn.Custom:
                    return "";
            }

            // lien cliquable
            // custom full js

            //retour.push({ { ""data"": ""pay_label"", ""autoWidth"": false, orderable: false } });
            //retour.push({ { ""autoWidth"": false, orderable: false, render: function(d, t, r) { { return new Date(r.created).toLocaleString() } } } });
            return "";
        }
        private string RenderColumns(Column[] columns, string id, string deleteUrl, string editUrl)
        {
            var sb = new StringBuilder($@"function get{id}Columns() {{ 
                var retour = new Array();");

            foreach (var column in columns)
                sb.Append(RenderColumn(column));

            sb.Append($@"retour.push({{
                            ""autoWidth"": false, orderable: false, render: function (d, t, r) {{
                                return '<button data-url=""{editUrl}?id=' + r.pay_id + '"" class=""btn btn-primary me-2 btn-edit"" data-content=""edit-pays-container""><i class=""bx bx-edit""></i></button>' +
                                    '<a href=""{deleteUrl}?id=' + r.pay_id + '"" class=""btn btn-danger btn-delete""><i class=""bx bx-trash""></i></a>';
                            }}
                        }}); return retour; }}");

            return sb.ToString();
        }

        public Datatable(IHtmlHelper htmlHelper, string id, string cssClass, int length, int start, string loadUrl, string[] columnsName, Column[] columns, string confirmDeletePhrase, string deleteUrl, string editUrl, bool hasToolsColumns = true, bool isOnly = true)
        {

            //TODO : gestion de l'edit => nouvelle page, modal ou ajax
            //TODO : gestion des colonnes

            _writer = htmlHelper.ViewContext.Writer;

            var html = new StringBuilder();
            var str = string.Join("</th><th>", columnsName);

            html.Append($@"<div id=""{id}-list"">
                                <div class=""row"">
                                    <div class=""col-md-12"">
                                        <div id=""{id}-filters"">
                                            <input type=""hidden"" ""hf-{id}-length"" />
                                            <input type=""hidden"" ""hf-{id}-start"" />
                                        </div>
                                        <table class=""table"" id=""{id}-table"" data-url=""{loadUrl}"">
                                            <thead><tr><th>
                                                {(hasToolsColumns ? str : str.Remove(str.Length - 5, 4))}
                                            </th></tr></thead>
                                        </table>
                                    </div>
                                </div>
                            </div>");
            _writer.Write(html.ToString());

            html = new StringBuilder();
            html.Append($@"<script type=""text/javascript"">
                            (function (d, $, undefined) {{
                            ""use strict"";
                            var {id}Table = (function () {{
                                var self = {{}};
                                var $container;
                                self.init = function (container) {{
                                    $container = container;
                                    datatableExtension.init($container, '#hf-{id}-start', '#hf-{id}-length');
                                    datatableExtension.drawDatatable(get{id}Columns(), '#{id}-filters', '#{id}-table', location.pathname, false);
                                    bindEvents();
                                }}}};
                                
                                {RenderColumn(columns)}

                                function bindEvents() {{
                                    $container.on('click', '.btn-delete', function (e) {{
                                            e.preventDefault();
                                            var r = confirm('{confirmDeletePhrase}');
                                            if (r) {{
                                                global.ajaxPost($(this).prop('href'), {{}}, function (data) {{
                                                    datatableExtension.drawDatatable(get{id}Columns(), '#{id}-filters', '#{id}-table', location.pathname);
                                                }});
                                            }}
                                        }});
                                    }}

                                    return self;
                                }})();

                                $(d).ready(function () {{ {id}Table.init($('#{id}-list')); }});

                            }})(window.document, jQuery);
                        </script>");



            if (isOnly)
                _writer.Write(DatatableScript.Get());
        }


        public void Dispose()
        {
        }
    }
}
