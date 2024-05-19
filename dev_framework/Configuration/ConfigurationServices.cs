using Microsoft.Extensions.Configuration;

namespace dev_framework.Configuration
{
    public class ConfigurationServices
    {
        private IConfiguration _configuration;

        public ConfigurationServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public T GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }

        public string GetTrashFolder()
        {
            var folder = string.Format("{0}{1}", GetDataFolder(), _configuration.GetValue<string>("trashFolder"));
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        public string GetLogProjectFolder(string project)
        {
            var folder = string.Format("{0}{1}", GetDataProjectFolder(project), _configuration.GetValue<string>("logFolder"));
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }
        public string GetLogProjectFolder(string project, int? deviceId)
        {
            if (!deviceId.HasValue)
                return GetLogProjectFolder(project);

            var folder = string.Format("{0}{1}{2}/", GetDataProjectFolder(project), _configuration.GetValue<string>("logFolder"), deviceId.Value);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        public string GetUploadProjectFolder(string project, int deviceId)
        {
            var folder = string.Format("{0}{1}{2}/", GetDataProjectFolder(project), _configuration.GetValue<string>("uploadFolder"), deviceId);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        private string GetDataFolder()
        {
            var folder = _configuration.GetValue<string>("dataFolder");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }
        private string GetDataProjectFolder(string project)
        {
            var folder = string.Format("{0}{1}/", GetDataFolder(), project);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }
    }
}
