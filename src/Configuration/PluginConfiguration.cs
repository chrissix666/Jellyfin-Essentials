using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Essentials.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>The scripts that run, by their keys (<see cref="Scripts.All"/>).</summary>
    public string[] EnabledScripts { get; set; } = Scripts.All.Select(s => s.Key).ToArray();

    /// <summary>The order of the header buttons, by their keys (<see cref="Scripts.HeaderButtons"/>), left to right.</summary>
    public string[] HeaderOrder { get; set; } = Scripts.HeaderButtons.Select(b => b.Key).ToArray();

    /// <summary>
    /// The settings that differ from the scripts' own defaults, as JSON: script key, then the setting's path in the script
    /// ("SETTINGS.movies.rowOrder"), then its value.
    /// </summary>
    public string Settings { get; set; } = "{}";
}
