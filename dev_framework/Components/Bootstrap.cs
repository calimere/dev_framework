using dev_framework.Components.Model.Card;
using dev_framework.Components.Model.Tabs.Content;
using dev_framework.Components.Model.Tabs.Nav;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;

namespace dev_framework.Components
{
    public static class BootstrapExtensions
    {

        public static IHtmlContent BootstrapTabNavBar(this IHtmlHelper htmlHelper, TabNavBarViewModel navBarViewModel)
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
                var dataAttribute = item.DataAttribute != null ? string.Join(" ", item.DataAttribute.Select(x => $"data-{x.Key}=\"{x.Value}\"")) : "";
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
        public static IHtmlContent BootstrapTabContent(
            this IHtmlHelper htmlHelper,
            TabContentViewModel tabContentViewModel)
        {
            // Utilisation de StringBuilder pour une meilleure performance
            var sb = new StringBuilder();

            // Conteneur principal des tabs
            string cssClass = "";
            if (tabContentViewModel.CssClass != null)
                cssClass = string.Join(" ", tabContentViewModel.CssClass);

            sb.Append($"<div class=\"tab-content {cssClass}\" id=\"{tabContentViewModel.ContentId}\">");

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
                if (item.DataAttribute != null && item.DataAttribute.Any())
                {
                    foreach (var dataAttr in item.DataAttribute)
                    {
                        tabPane.MergeAttribute($"data-{dataAttr.Key}", dataAttr.Value);
                    }
                }

                if (!string.IsNullOrEmpty(item.View))
                {
                    // Ajout du contenu de la vue partielle
                    var partialViewContent = htmlHelper.Partial(item.View, item.Model);
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
        public static Card BootstrapCard(this IHtmlHelper htmlHelper, CardViewModel cardViewModel)
        {
            return new Card(htmlHelper, cardViewModel);
        }

        public static Card BootstrapCard(this IHtmlHelper htmlHelper, string id, string[] cssClasses, string title)
        {
            return new Card(htmlHelper, id, cssClasses, title);
        }

    }
    public class Card : IDisposable
    {
        private readonly TextWriter _writer;

        public Card() { }
        public Card(IHtmlHelper htmlHelper, CardViewModel cardViewModel)
        {
            _writer = htmlHelper.ViewContext.Writer;

            string cssClass = "";
            if (cardViewModel.CssClass != null)
                cssClass = string.Join(" ", cardViewModel.CssClass);

            _writer.Write($"<div class=\"card {string.Join(" ", cssClass)}\" id=\"{cardViewModel.Id}\">");
            new CardHeader(htmlHelper, cardViewModel.Header);
            new CardBody(htmlHelper, cardViewModel.Body);
        }
        public Card(IHtmlHelper htmlHelper, string id, string[] cssClasses, string title)
        {
            _writer = htmlHelper.ViewContext.Writer;

            string cssClass = "";
            if (cssClasses != null)
                cssClass = string.Join(" ", cssClasses);

            _writer.Write($"<div class=\"card {string.Join(" ", cssClass)}\" id=\"{id}\">");
            new CardHeader(htmlHelper, new CardHeaderModel() { Id = id, Title = title });
            new CardBody(htmlHelper, new CardBodyModel() { Id = id });
        }

        public void Dispose()
        {
            _writer.Write("</div>");
            _writer.Write("</div>");
            _writer.Write("</div>");
        }
    }
    public class CardHeader : IDisposable
    {
        private readonly TextWriter _writer;
        public CardHeader() { }
        public void Dispose() { }
        public CardHeader(IHtmlHelper htmlHelper, CardHeaderModel cardModel)
        {
            if (cardModel != null)
            {
                _writer = htmlHelper.ViewContext.Writer;

                var cssClass = "";
                if (cardModel.CssClass != null)
                    cssClass = string.Join(" ", cardModel.CssClass);

                var html = new StringBuilder();
                html.Append($"<div class=\"card-header {cssClass} {(cardModel.IsCollapsed != null && cardModel.IsCollapsed.Value ? "collapsed" : "")}\" id=\"{cardModel.HeaderId}\"")
                    .Append(cardModel.IsCollapsable ? $"data-bs-toggle=\"collapse\" data-bs-target=\"#{cardModel.BodyId}\" aria-expanded=\"false\" aria-controls=\"#{cardModel.BodyId}\"" : "")
                    .Append(">")
                    .Append($"<h5 class=\"card-title\">");

                if (cardModel.Icons != null)
                    html.Append($"<i class=\"{string.Join(" ", cardModel.Icons)}\"></i>");

                html.Append($" {cardModel.Title}")
                .Append("</h5>")
                .Append("</div>");

                _writer.Write(html.ToString());
            }

        }
    }
    public class CardBody : IDisposable
    {
        private readonly TextWriter _writer;
        public CardBody() { }
        public CardBody(IHtmlHelper htmlHelper, CardBodyModel cardModel)
        {
            _writer = htmlHelper.ViewContext.Writer;

            string cssClass = "";
            if (cardModel.CssClass != null)
                cssClass = string.Join(" ", cardModel.CssClass);

            var sb = new StringBuilder($"<div id=\"{cardModel.BodyId}\" class=\"{(cardModel.IsCollapsed.HasValue && cardModel.IsCollapsed.Value ? "collapse" : "collapse show")} {cssClass}\">")
                .Append($"<div class=\"card-body\">");


            if (!string.IsNullOrEmpty(cardModel.View))
            {
                using (var writer = new System.IO.StringWriter())
                {
                    htmlHelper.Partial(cardModel.View, cardModel.Model).WriteTo(writer, HtmlEncoder.Default);
                    sb.Append(writer.ToString());
                }
            }
            _writer.Write(new HtmlString(sb.ToString()));
        }
        public void Dispose()
        {
        }
    }
}
