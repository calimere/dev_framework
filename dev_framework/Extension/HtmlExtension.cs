using dev_framework.Form.Model.Html;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
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
}