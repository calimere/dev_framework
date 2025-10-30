using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.MQTT
{
    public class MqttMessageRouter
    {
        private readonly List<(string filter, Action<string, string> handler)> _routes = new();
        private readonly bool _callAllMatches;

        /// <param name="callAllMatches">Si true appelle tous les handlers qui matchent; si false s'arrête au premier match.</param>
        public MqttMessageRouter(bool callAllMatches = false)
        {
            _callAllMatches = callAllMatches;
        }

        public void Register(string topicFilter, Action<string, string> handler)
        {
            if (string.IsNullOrWhiteSpace(topicFilter)) throw new ArgumentException("topicFilter required", nameof(topicFilter));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _routes.Add((topicFilter, handler));
        }

        public void Dispatch(string topic, string payload)
        {
            bool matchedAny = false;

            foreach (var (filter, handler) in _routes)
            {
                if (TopicMatches(topic, filter))
                {
                    handler(topic, payload);
                    matchedAny = true;
                    if (!_callAllMatches) return; // si on ne veut qu'un seul handler, on sort
                }
            }

            if (!matchedAny)
            {
                // comportement par défaut : log
                Console.WriteLine($"(No handler) {topic} -> {payload}");
            }
        }

        // Implémentation robuste du matching MQTT (+ et #)
        public static bool TopicMatches(string topic, string topicFilter)
        {
            if (topic == null || topicFilter == null) return false;

            // Cas trivial
            if (topicFilter == "#") return true;

            var tLevels = topic.Split('/');
            var fLevels = topicFilter.Split('/');

            int ti = 0, fi = 0;
            while (fi < fLevels.Length && ti < tLevels.Length)
            {
                var f = fLevels[fi];
                var t = tLevels[ti];

                if (f == "#")
                {
                    // # doit être le dernier niveau du filtre pour être valide
                    return fi == fLevels.Length - 1;
                }
                else if (f == "+")
                {
                    // + match un niveau ; on continue
                    fi++; ti++;
                    continue;
                }
                else
                {
                    // correspondance littérale
                    if (!string.Equals(f, t, StringComparison.Ordinal)) return false;
                    fi++; ti++;
                    continue;
                }
            }

            // si on a parcouru tous les niveaux du filtre :
            if (fi == fLevels.Length)
            {
                // topic doit aussi être totalement consommé pour matcher (sauf si filtre se termine par "#" qui aurait été géré ci-dessus)
                return ti == tLevels.Length;
            }

            // si il reste un seul niveau dans le filtre et que c'est "#", ça matche (ce cas arrive quand topic est plus court)
            if (fi == fLevels.Length - 1 && fLevels[fi] == "#") return true;

            return false;
        }
    }

}
