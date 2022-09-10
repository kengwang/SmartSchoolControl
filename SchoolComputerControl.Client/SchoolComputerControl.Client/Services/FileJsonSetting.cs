using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SchoolComputerControl.Client.Interfaces;

namespace SchoolComputerControl.Client.Services;

public class FileJsonSetting<TSetting> : ISetting<TSetting> where TSetting : INotifyPropertyChanged, new()
{
    private readonly ILogger<FileJsonSetting<TSetting>> _logger;

    private TSetting? _inMemorySetting;
    private readonly string _settingName;

    public TSetting? Setting => _inMemorySetting;

    public FileJsonSetting(ILogger<FileJsonSetting<TSetting>> logger)
    {
        _logger = logger;
        _settingName = typeof(TSetting).Name;

        // Pre Load Settings
        LoadSettingsFromFile();

        /*
        // No More Monitor
        // Add FileSystem Monitor
        var fileSystemWatcher = new FileSystemWatcher();
        fileSystemWatcher.Path = $"{Directory.GetCurrentDirectory()}/settings";
        fileSystemWatcher.Changed += FileMonitorOnChanged;
        fileSystemWatcher.Created += FileMonitorOnChanged;
        fileSystemWatcher.Deleted += FileMonitorOnChanged;
        fileSystemWatcher.Renamed += FileMonitorOnChanged;
        fileSystemWatcher.Filters.Add("*.json");
        fileSystemWatcher.EnableRaisingEvents = true;
        */
        // Add Option Monitor
        if (_inMemorySetting != null) _inMemorySetting.PropertyChanged += InMemorySettingOnPropertyChanged;
    }


    private void InMemorySettingOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // This will automatically fire FileSystemWatcher
        if (_inMemorySetting != null)
        {
            File.WriteAllText($"settings/{_settingName}.json", JsonSerializer.Serialize(_inMemorySetting));
            return;
        }

        File.Delete($"settings/{_settingName}.json");
    }


    /*
    private void FileMonitorOnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.FullPath.EndsWith($"{_settingName}.json"))
        {
            LoadSettingsFromFile();
        }
    }
    */

    private void LoadSettingsFromFile()
    {
        _logger.LogTrace("Loading Configuration of {SettingName}", nameof(TSetting));
        try
        {
            if (!Directory.Exists("settings")) Directory.CreateDirectory("settings");
            if (File.Exists($"settings/{_settingName}.json"))
                _inMemorySetting =
                    JsonSerializer.Deserialize<TSetting>(
                        File.ReadAllText($"settings/{_settingName}.json"));
            else
            {
                File.WriteAllText($"settings/{_settingName}.json", "{}");
                _inMemorySetting = new TSetting();
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e,"Configuration Load Failed");
            _inMemorySetting = new TSetting();
        }
    }
}