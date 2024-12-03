using dev_framework.Components.Model.ChartJS;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dev_framework.Components
{
    public static class ChartExtensions
    {
        public static Chart Chart(this IHtmlHelper htmlHelper, string id, string type, string title, ChartData data) { return new Chart(htmlHelper, id, type, title, data); }
    }

    public class Chart : IDisposable
    {
        private readonly TextWriter _writer;

        public Chart() { }
        public Chart(IHtmlHelper htmlHelper, string id, string type, string title, ChartData data)
        {
            _writer = htmlHelper.ViewContext.Writer;
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var jsonData = JsonConvert.SerializeObject(data, settings);

            var script = $@"
            <div id=""{id}-canvas-container"">
            <canvas id=""{id}-canvas""></canvas>
            <script>
                const {id}Ctx = document.getElementById('{id}-canvas').getContext('2d');
                const {id}Chart = new Chart({id}Ctx, {{
                    type: '{type}',
                    data: {jsonData},
                    options: {{
                        responsive: true,
                        plugins: {{
                            legend: {{
                                position: 'top',
                            }},
                            title: {{
                                display: {(!string.IsNullOrEmpty(title) ? "true" : "false")},
                                text: '{title}'
                            }}
                        }}
                    }}
                }});
            </script>";

            _writer.Write(script);
        }

        public void Dispose()
        {
            _writer.Write("</div>");
        }
    }
}
