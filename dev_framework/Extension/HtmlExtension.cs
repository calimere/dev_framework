using dev_framework.Extension.Model;
using dev_framework.Form.Model.Html;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;

public static class HtmlHelperExtension
{
    public static IHtmlContent Button(this IHtmlHelper helper, string label, object htmlAttribute, object dataAttribute)
    {
        var attributes = string.Empty;
        var dataString = string.Empty;

        var dic = htmlAttribute.PropertyToDictionary();
        foreach (var item in dic)
            attributes += string.Format("{0}=\"{1}\" ", item.Key, item.Value);

        var datas = dataAttribute.PropertyToDictionary();
        foreach (var item in datas)
            dataString += string.Format("{0}=\"{1}\" ", item.Key, item.Value);

        var str = string.Format("<button {0} {2}>{1}</button>", attributes, label, dataString);
        return new HtmlString(str);
    }
    public static IHtmlContent DisableIf(this IHtmlContent htmlString, Func<bool> expression)
    {
        if (expression.Invoke())
        {
            var html = GetString(htmlString);
            const string disabled = "\"disabled\"";
            html = html.Insert(html.IndexOf(">",
              StringComparison.Ordinal), " disabled= " + disabled);
            return new HtmlString(html);
        }
        return htmlString;
    }
    private static string GetString(IHtmlContent content)
    {
        var writer = new System.IO.StringWriter();
        content.WriteTo(writer, HtmlEncoder.Default);
        return writer.ToString();
    }
    public static IHtmlContent Title(this IHtmlHelper helper, HtmlTitleComponent obj)
    {
        var str = string.Format("<{0} class=\"{1}\" id=\"\">{2}</{0}>", obj.HtmlTitleType.ToString(), obj.CssClass, obj.CssId, obj.Text);
        return new HtmlString(str);
    }
    public static IHtmlContent Section(this IHtmlHelper helper, HtmlSection section)
    {
        var str = string.Format(@"<div class='{0}' id='{1}' data-aos='fade-up' data-aos-offset='10'><div class='container'>{2}<div class='row'>{3}</div></div>", section.Class, section.Id, Title(helper, section.Title), section.Html);
        return new HtmlString(str);
    }
    public static IHtmlContent RenderViewData(this IHtmlHelper helper, object viewdata, string beforeText = null, string afterText = null)
    {
        if (viewdata != null)
        {
            var str = viewdata.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                if (!string.IsNullOrEmpty(beforeText))
                    str = string.Format("{0}{1}", beforeText, str);

                if (!string.IsNullOrEmpty(afterText))
                    str = string.Format("{0}{1}", str, afterText);
            }
            return new HtmlString(str);
        }
        return new HtmlString("");
    }
    public static IHtmlContent TabNavBar(this IHtmlHelper htmlHelper, TabNavBarViewModel navBarViewModel)
    {
        var html = new StringBuilder();
        html.Append("<nav class=\"nav\">")
            .Append("<div class=\"\"")
            .Append($" id=\"{navBarViewModel.NavBarId}\" role=\"tablist\">")
            .Append($"<ul class=\"nav {(navBarViewModel.CssClass != null ? string.Join(" ", navBarViewModel.CssClass) : "")}\">");

        foreach (var item in navBarViewModel.TabNavBarModels)
        {
            var cssClass = item.CssClass != null ? string.Join(" ", item.CssClass) : "";
            var isActive = item.IsActive ? "active" : "";
            var dataAttribute = string.Join(" ", item.DataAttribute.Select(x => $"data-{x.Key}=\"{x.Value}\""));
            var icon = item.IconClass != null ? string.Join(" ", item.IconClass) : "";

            html.Append("<li class=\"nav-item\">")
                .Append("<button class=\"nav-link ")
                .Append($"{cssClass} {isActive}\" type=\"button\" data-bs-toggle=\"tab\"")
                .Append($" data-bs-target=\"#{item.ContentId}\" role=\"tab\"")
                .Append($" aria-controls=\"{item.ContentId}\" id=\"{item.TabNavbarId}\"")
                .Append($" aria-selected=\"true\" {dataAttribute}>")
                .Append($"<i class=\"{icon} me-2\"></i>")
                .Append($"{item.Title}")
                .Append("</button>")
                .Append("</li>");
        }

        html.Append("</ul></div></nav>");

        return new HtmlString(html.ToString());
    }
    public static IHtmlContent TabContent(
        this IHtmlHelper htmlHelper,
        TabContentViewModel tabContentViewModel)
    {
        // Utilisation de StringBuilder pour une meilleure performance
        var sb = new StringBuilder();

        // Conteneur principal des tabs
        sb.Append($"<div class=\"tab-content {string.Join(" ", tabContentViewModel.CssClass)}\" id=\"{tabContentViewModel.ContentId}\">");

        // Ajout de chaque tab contenu
        foreach (var item in tabContentViewModel.TabContentModels)
        {
            // Crée un tag div pour chaque tab-pane
            var tabPane = new TagBuilder("div");
            tabPane.AddCssClass("tab-pane fade");

            if (item.IsActive)
                tabPane.AddCssClass("show active");

            tabPane.Attributes["id"] = item.ContentId;
            tabPane.Attributes["role"] = "tabpanel";
            tabPane.Attributes["aria-labelledby"] = item.TabNavbarId;

            // Ajout des data-attributes
            foreach (var dataAttr in item.DataAttribute)
            {
                tabPane.MergeAttribute($"data-{dataAttr.Key}", dataAttr.Value);
            }

            if (!string.IsNullOrEmpty(item.View))
            {
                // Ajout du contenu de la vue partielle
                var partialViewContent = htmlHelper.PartialAsync(item.View, item.Model).Result;
                tabPane.InnerHtml.AppendHtml(partialViewContent);
            }


            // Conversion en HTML
            using (var writer = new StringWriter())
            {
                tabPane.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer.ToString());
            }
        }

        // Fermeture du conteneur principal
        sb.Append("</div>");

        return new HtmlString(sb.ToString());
    }
    private static string GetFromDic(Dictionary<string, string> dic)
    {
        return string.Join(" ", dic.Select(x => $"data-{x.Key}=\"{x.Value}\""));
    }
}
