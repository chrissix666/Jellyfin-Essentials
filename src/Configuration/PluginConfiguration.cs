using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Essentials.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>The scripts that run, by their keys (<see cref="Parts.All"/>).</summary>
    public string[] EnabledParts { get; set; } = Parts.All.Select(p => p.Key).ToArray();

    /// <summary>The order of the header buttons, by their keys (<see cref="Parts.HeaderButtons"/>), left to right.</summary>
    public string[] HeaderOrder { get; set; } = Parts.HeaderButtons.Select(b => b.Key).ToArray();

    /// <summary>
    /// The settings that differ from the scripts' own defaults, as JSON: part key, then the setting's path in the script
    /// ("SETTINGS.movies.rowOrder"), then its value.
    /// </summary>
    public string Settings { get; set; } = "{}";
}
