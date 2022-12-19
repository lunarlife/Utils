using Newtonsoft.Json;
using Utils.Exceptions;

namespace Utils;

public abstract class Configuration
{
    [JsonIgnore] private static List<Configuration> _configurations = new();

    [JsonIgnore] public static IReadOnlyList<Configuration> Configurations => _configurations;

    [JsonIgnore] public abstract string FilePath { get; }
    public static async void SaveAll()
    {
        try
        {
            for (var i = 0; i < _configurations.Count; i++)
            {
                var configuration = _configurations[i];
                await configuration.Save();
            }
        }
        catch (Exception e)
        {
            throw new ConfigurationException(e.Message);
        }
    }

    public async Task Save()
    {
        await File.WriteAllTextAsync(FilePath,
            JsonConvert.SerializeObject(this, Formatting.Indented));
    }
    public static T? LoadConfiguration<T>() where T : Configuration, new()
    {
        try
        {
            var configuration = new T();
            if (!File.Exists(configuration.FilePath))
                return null;
            var loadConfiguration = JsonConvert.DeserializeObject<T>(File.ReadAllText(configuration.FilePath));
            if (loadConfiguration is null) return null;
            _configurations.Add(loadConfiguration);
            return loadConfiguration;
        }
        catch (Exception e)
        {
            throw new ConfigurationException(e.Message);
        }
    }
}