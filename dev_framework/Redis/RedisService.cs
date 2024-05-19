using Newtonsoft.Json;
using StackExchange.Redis;

namespace dev_framework.Services
{
    public class RedisService : IDisposable
    {
        private bool _disposed;
        private int _tryError;
        private string _url;

        public void Dispose()
        {
            _disposed = true;
            if(_connectionMultiplexer != null)
                _connectionMultiplexer.Dispose();
        }

        public bool Available { get { return _disposed; } }
        private ConnectionMultiplexer _connectionMultiplexer;
        public ISubscriber Subscriber { get { return _connectionMultiplexer.GetSubscriber(); } }
        public async Task InitAsync()
        {
            try
            {
                _connectionMultiplexer = ConnectionMultiplexer.Connect(_url);
                _tryError = 0;
                _disposed = true;
            }
            catch (Exception ex)
            {
                _disposed = false;
                _tryError++;
                Thread.Sleep((_tryError * 500) * (_tryError - 1));
                throw ex;
            }
        }
        public void Init()
        {
            try
            {
                _connectionMultiplexer = ConnectionMultiplexer.Connect(_url);
                _tryError = 0;
                _disposed = true;
            }
            catch (Exception ex)
            {
                _disposed = false;
                _tryError++;
                Thread.Sleep((_tryError * 500) * (_tryError - 1));
                throw ex;
            }
        }
        public void Ping()
        {
            try { Subscriber.Publish("PING", ""); }
            catch (Exception ex)
            {
                _disposed = false;
                _tryError++;
                throw ex;
            }
        }
        public RedisService(string url, bool init = false)
        {
            _disposed = false;
            _tryError = 0;
            _url = url;

            if (init)
                Init();
        }
        public long Publish(string channel, object message)
        {
            if (_disposed)
                return Subscriber.Publish(channel, JsonConvert.SerializeObject(message));
            return -1;
        }

        public void Set(string key, RedisValue value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            db.StringSet(key, value);
        }
        public string? GetString(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return db.StringGet(key);
        }
    }

    public class RedisConnectorHelper
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        public RedisConnectorHelper(string url) { _lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect(url); }); }
        public ConnectionMultiplexer Connection { get { return _lazyConnection.Value; } }
    }
}
