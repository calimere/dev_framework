using dev_framework.Components.Model.Datatble;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components
{
    public static class DatatableExtensions
    {
        private static string RenderColumn(Column column)
        {
            switch (column.ETypeColumn)
            {
                case ETypeColumn.Normal:
                    return $@"retour.push({{ ""data"": ""{column.Data}"", ""autoWidth"": {column.AutoWidth.ToString().ToLower()}, orderable: {column.Orderable.ToString().ToLower()} }});";
                case ETypeColumn.Link:
                    return $@"retour.push({{ ""data"": ""{column.Data}"", ""autoWidth"": {column.AutoWidth.ToString().ToLower()}, orderable: {column.Orderable.ToString().ToLower()} }});";
                case ETypeColumn.Custom:
                    return $@"retour.push({{""autoWidth"": {column.AutoWidth.ToString().ToLower()}, orderable: {column.Orderable.ToString().ToLower()}, render: function (d, t, r) {{
                        return '{column.Render}';
                        }} }});";
            }
            return "";
        }
        private static string RenderColumns(Column[] columns, string id, string deleteUrl, string editUrl)
        {
            var sb = new StringBuilder($@"function get{id}Columns() {{ 
                var retour = new Array();");

            var key = "";
            foreach (var column in columns)
            {
                if (column.IsKey)
                    key = column.Data;
                sb.Append(RenderColumn(column));
            }

            sb.Append($@"retour.push({{
                            ""autoWidth"": false, orderable: false, render: function (d, t, r) {{
                                return '<button data-url=""{editUrl}?id=' + r.{key} + '"" class=""btn btn-primary me-2 btn-edit"" data-content=""edit-{id}-container""><i class=""bx bx-edit""></i></button>' +
                                    '<a href=""{deleteUrl}?id=' + r.{key} + '"" class=""btn btn-danger btn-delete""><i class=""bx bx-trash""></i></a>';
                            }}
                        }}); return retour; }}");

            return sb.ToString();
        }
        public static IHtmlContent DataTable(this IHtmlHelper htmlHelper, string id, string[] containerCssClass, int length, int start, string loadUrl, string[] columnsName, bool hasActionColumn = true)
        {
            var html = new StringBuilder();
            var str = new StringBuilder();

            foreach (var item in columnsName)
            {
                str.Append($@"<th>{item}</th>");
            }
            if (hasActionColumn)
                str.Append($@"<th></th>");


            html.Append($@"<div id=""{id}-list"" class=""{(containerCssClass != null ? string.Join(" ", containerCssClass) : "")}"" >
                                <div class=""row"">
                                    <div class=""col-md-12"">
                                        <div id=""{id}-filters"">
                                            <input type=""hidden"" id=""hf-{id}-length"" value=""{length}"" />
                                            <input type=""hidden"" id=""hf-{id}-start"" value=""{start}""/>
                                        </div>
                                        <table class=""table"" id=""{id}-table"" data-url=""{loadUrl}"">
                                            <thead><tr>
                                                {str}
                                            </tr></thead>
                                        </table>
                                    </div>
                                </div>
                            </div>");

            return new HtmlString(html.ToString());
        }

        public static IHtmlContent RenderJS(this IHtmlHelper htmlHelper, string id, Column[] columns, string confirmDeletePhrase, string deleteUrl, string editUrl)
        {
            var html = new StringBuilder();
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
                                }};
                                
                                {RenderColumns(columns, id, deleteUrl, editUrl)}

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
            return new HtmlString(html.ToString());


        }
        public static IHtmlContent IncludeDatatableScript(this IHtmlHelper htmlHelper)
        {
            return new HtmlString($@"<link href=""/lib/datatables/datatables.css"" rel=""stylesheet"" />
                    <script type=""text/javascript"" src=""/lib/datatables/datatables.min.js""></script>
                    <script type=""text/javascript"" src=""//cdn.datatables.net/buttons/1.2.1/js/buttons.print.min.js""></script>
                    <script type=""text/javascript"" src=""/lib/csk-libs/datatable.extension.js""></script>");
        }
    }
}
