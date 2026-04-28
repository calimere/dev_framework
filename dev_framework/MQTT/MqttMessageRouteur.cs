using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_framework.MQTT
{
    public class MqttMessageRouter
    {
        private readonly List<(string filter, Func<string, int, string, Task> handler)> _routes = new();
        private readonly bool _callAllMatches;

        /// <param name="callAllMatches">
        /// Si true, appelle tous les handlers qui matchent.
        /// Si false, s'arrête au premier match.
        /// </param>
        public MqttMessageRouter(bool callAllMatches = false)
        {
            _callAllMatches = callAllMatches;
        }

        // ── Enregistrement ────────────────────────────────────────────────

        /// <summary>Enregistre un handler synchrone.</summary>
        public void Register(string topicFilter, Action<string, int, string> handler)
        {
            if (string.IsNullOrWhiteSpace(topicFilter)) throw new ArgumentException("topicFilter required", nameof(topicFilter));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _routes.Add((topicFilter, (topic, id, payload) =>
            {
                handler(topic, id, payload);
                return Task.CompletedTask;
            }
            ));
        }

        /// <summary>Enregistre un handler asynchrone.</summary>
        public void Register(string topicFilter, Func<string, int, string, Task> handler)
        {
            if (string.IsNullOrWhiteSpace(topicFilter)) throw new ArgumentException("topicFilter required", nameof(topicFilter));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _routes.Add((topicFilter, handler));
        }

        // ── Dispatch ──────────────────────────────────────────────────────

        /// <summary>
        /// Dispatch synchrone (rétrocompatibilité).
        /// Préférer DispatchAsync dans un contexte async.
        /// </summary>
        public void Dispatch(string topic, int id, string payload)
            => DispatchAsync(topic, id, payload).GetAwaiter().GetResult();

        /// <summary>Dispatch asynchrone.</summary>
        public async Task DispatchAsync(string topic, int id, string payload)
        {
            bool matchedAny = false;

            foreach (var (filter, handler) in _routes)
            {
                if (TopicMatches(topic, filter))
                {
                    await handler(topic, id, payload);
                    matchedAny = true;
                    if (!_callAllMatches) return;
                }
            }

            if (!matchedAny)
                Console.WriteLine($"(No handler) {topic} -> {payload}");
        }

        // ── Pattern matching ──────────────────────────────────────────────

        /// <summary>Vérifie si un topic correspond à un filtre MQTT (supporte + et #).</summary>
        public static bool TopicMatches(string topic, string topicFilter)
        {
            if (topic == null || topicFilter == null) return false;
            if (topicFilter == "#") return true;

            var tLevels = topic.Split('/');
            var fLevels = topicFilter.Split('/');

            int ti = 0, fi = 0;
            while (fi < fLevels.Length && ti < tLevels.Length)
            {
                var f = fLevels[fi];
                var t = tLevels[ti];

                if (f == "#")
                    return fi == fLevels.Length - 1;

                if (f == "+") { fi++; ti++; continue; }

                if (!string.Equals(f, t, StringComparison.Ordinal)) return false;

                fi++; ti++;
            }

            if (fi == fLevels.Length)
                return ti == tLevels.Length;

            if (fi == fLevels.Length - 1 && fLevels[fi] == "#") return true;

            return false;
        }
    }
}