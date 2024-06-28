namespace HybridLifecycle.Interfaces;

public interface IConfigurationService
{
    string GetSetting(string key);
    void UpdateSetting(string key, string value);
}