using HybridLifecycle;

public class HybridConfigurationService : IDisposable
{
    private ConfigurationService _singletonInstance;
    private readonly Dictionary<Guid, (ConfigurationService Service, DateTime CreatedAt)> _scopedInstances = new();
    private readonly object _lock = new();
    private bool _disposed;
    private readonly TimeSpan _scopedLifetime = TimeSpan.FromSeconds(10);
    private Timer _cleanupTimer;

    public HybridConfigurationService()
    {
        _cleanupTimer = new Timer(CleanupExpiredScopedInstances, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    public ConfigurationService GetService(Guid? scopeId)
    {
        lock (_lock)
        {
            if (scopeId == null || scopeId == Guid.Empty)
            {
                if (_singletonInstance == null)
                {
                    _singletonInstance = new ConfigurationService();
                }
                return _singletonInstance;
            }
            else
            {
                if (!_scopedInstances.ContainsKey(scopeId.Value))
                {
                    _scopedInstances[scopeId.Value] = (new ConfigurationService(), DateTime.UtcNow);
                }
                return _scopedInstances[scopeId.Value].Service;
            }
        }
    }

    private void CleanupExpiredScopedInstances(object state)
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _scopedInstances
                .Where(kvp => now - kvp.Value.CreatedAt > _scopedLifetime)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _scopedInstances[key].Service.Dispose();
                _scopedInstances.Remove(key);
            }
        }
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
            lock (_lock)
            {
                foreach (var instance in _scopedInstances.Values)
                {
                    instance.Service.Dispose();
                }
                _scopedInstances.Clear();

                _singletonInstance?.Dispose();
                _cleanupTimer?.Dispose();
            }
        }

        _disposed = true;
    }

    ~HybridConfigurationService()
    {
        Dispose(false);
    }
}
