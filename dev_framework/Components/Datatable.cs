using dev_framework.Form.Model.Datatable;
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
        private static string RenderColumn(DataTableColumn column)
        {
            switch (column.ETypeColumn)
            {
                case ETypeColumn.Normal:
                    return $@"retour.push({{ ""data"": ""{column.data}"", ""autoWidth"": {column.AutoWidth.ToString().ToLower()}, title:'{column.Title}', name:""{column.data}"", orderable: {column.orderable.ToString().ToLower()}, searchable:{column.searchable.ToString().ToLower()} }});";
                case ETypeColumn.Custom:
                    return $@"retour.push({{""data"": ""{column.data}"", ""autoWidth"": {column.AutoWidth.ToString().ToLower()}, name:""{column.data}"", orderable: {column.orderable.ToString().ToLower()}, searchable:{column.searchable.ToString().ToLower()}, render: function (d, t, r) {{
                        return '{column.Render}';
                        }} }});";
                case ETypeColumn.Function:
                    return $@"retour.push({{""data"": ""{column.data}"", ""autoWidth"": {column.AutoWidth.ToString().ToLower()}, name:""{column.data}"", orderable: {column.orderable.ToString().ToLower()}, searchable:{column.searchable.ToString().ToLower()}, render: {column.Render} }});";
            }
            return "";
        }
        private static string RenderColumns(DataTableColumn[] columns, string id, string deleteUrl, string editUrl)
        {
            var sb = new StringBuilder($@"function get{id}Columns() {{ 
                var retour = new Array();");

            var key = "id";
            foreach (var column in columns)
            {
                if (column.IsKey)
                    key = column.data;
                sb.AppendLine(RenderColumn(column));
            }

            var delete = !string.IsNullOrEmpty(deleteUrl)
                ? $"<a href=\"{deleteUrl}?id=' + r.{key} + '\" class=\"btn btn-danger btn-delete\"><i class=\"bx bx-trash\"></i></a>"
                : "";

            sb.AppendLine($@"retour.push({{
                            ""data"":"""", ""autoWidth"": false, orderable: false, searchable:false, render: function (d, t, r) {{
                                return '<a href=""{editUrl}?id=' + r.{key} + '"" class=""btn btn-primary me-2 btn-edit"" data-content=""edit-{id}-container""><i class=""bx bx-edit""></i></a>' + '{delete}';
                            }}
                        }}); return retour; }}");

            return sb.ToString();
        }
        public static IHtmlContent DataTable(this IHtmlHelper htmlHelper, string id, string[] containerCssClass, int length, int start, string loadUrl, DataTableColumn[] columnsName, bool hasActionColumn = true)
        {
            var html = new StringBuilder();
            var str = new StringBuilder();
            var searchInputs = new StringBuilder();

            foreach (var item in columnsName)
            {
                str.AppendLine($@"<th>{item.Title}</th>");

                if (item.searchable)
                {
                    switch (item.ETagType)
                    {
                        case ETagType.Select:
                            searchInputs.AppendLine("<th>");
                            searchInputs.AppendLine($"<select name=\"select-{item.data}\" class=\"form-control\">");
                            searchInputs.AppendLine("<option value=\"\">Sélectionner</option>");
                            if (item.Options != null)
                            {
                                foreach (var option in item.Options)
                                {
                                    searchInputs.AppendLine($"<option value=\"{option.Value}\" {(option.Selected ? "selected" : "")}>{option.Text}</option>");
                                }
                            }
                            searchInputs.AppendLine("</select>");
                            searchInputs.AppendLine("</th>");
                            break;
                        case ETagType.Input: 
                            searchInputs.AppendLine($"<th><input type=\"{item.EInputType.ToString().ToLower()}\" placeholder=\"\" class=\"form-control\" name=\"input-{item.data}\" value=\"{item.search.value}\" /></th>");
                            break;
                    }
                }
                else
                    searchInputs.AppendLine("<th></th>");
            }

            if (hasActionColumn)
            {
                searchInputs.AppendLine($@"<th></th>");
                str.AppendLine($@"<th></th>");
            }

            html.AppendLine($@"<div id=""{id}-list"" class=""{(containerCssClass != null ? string.Join(" ", containerCssClass) : "")}"" >
                                <div class=""row"">
                                    <div class=""col-md-12"">
                                        <div id=""{id}-filters"" class=""dNone"">
                                            <input type=""hidden"" name=""length"" id=""hf-{id}-length"" value=""{length}"" />
                                            <input type=""hidden"" name=""start"" id=""hf-{id}-start"" value=""{start}""/>
                                        </div>
                                        <table class=""table"" id=""{id}-table"" data-url=""{loadUrl}"">
                                            <thead>
                                                <tr class=""search-inputs"">{searchInputs}</tr>
                                                <tr>
                                                    {str}
                                                </tr>
                                            </thead>
                                            <tfoot><tr>{searchInputs}</tr></tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>");

            return new HtmlString(html.ToString());
        }

        public static IHtmlContent RenderJS(this IHtmlHelper htmlHelper, string id, DataTableColumn[] columns, string confirmDeletePhrase, string deleteUrl, string editUrl, string loadCallback = null, string initCallback = null, bool withFilter = true)
        {
            var html = new StringBuilder();
            html.AppendLine($@"
<script type=""text/javascript"">
    (function (d, $, undefined) {{
    ""use strict"";
    var {id}Table = (function () {{
        var self = {{}};
        var $container;
        self.init = function (container) {{
            $container = container;
            datatableExtension.init($container, '#hf-{id}-start', '#hf-{id}-length');
            datatableExtension.drawDatatable(get{id}Columns(), '#{id}-filters', '#{id}-table', location.pathname, undefined, {withFilter.ToString().ToLower()} {(!string.IsNullOrEmpty(loadCallback) ? "," + loadCallback : "")} {(!string.IsNullOrEmpty(initCallback) ? "," + initCallback : "")});
            bindEvents();
        }};
                                
        {RenderColumns(columns, id, deleteUrl, editUrl)}

        function bindEvents() {{
            $container.on('click', '.btn-delete', function (e) {{
                    e.preventDefault();
                    var r = confirm('{confirmDeletePhrase}');
                    if (r) {{
                        global.ajaxPost($(this).prop('href'), {{}}, function (data) {{
                            datatableExtension.drawDatatable(get{id}Columns(), '#{id}-filters', '#{id}-table', location.pathname, undefined, {withFilter.ToString().ToLower()} {(!string.IsNullOrEmpty(loadCallback) ? "," + loadCallback : "")} {(!string.IsNullOrEmpty(initCallback) ? "," + initCallback : "")});
                        }});
                    }}
                }});
            }}

            return self;
        }})();

        $(d).ready(function () {{ 
            {id}Table.init($('#{id}-list')); 
        }});

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
