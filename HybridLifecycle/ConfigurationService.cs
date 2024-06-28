using HybridLifecycle.Interfaces;
using System.Runtime;

namespace HybridLifecycle
{
    public class ConfigurationService : IConfigurationService, IDisposable
    {
        private readonly Dictionary<string, string> _settings; 
        private bool _disposed = false;

        public ConfigurationService()
        {
            _settings = LoadDefaultSettings();
        }

        public string GetSetting(string key)
        {
            return _settings.TryGetValue(key, out var value) ? value : null;
        }

        public void UpdateSetting(string key, string value)
        {
            _settings[key] = value;
        }

        private Dictionary<string, string> LoadDefaultSettings()
        {
            var id = Guid.NewGuid().ToString();

            return new Dictionary<string, string>
            {
                { "Setting1", "DefaultValue1" },
                { "Setting2", "DefaultValue2" },
                { "ID", id }
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed resources
            }

            _disposed = true;
        }

        ~ConfigurationService()
        {
            Dispose(false);
        }
    }

}
