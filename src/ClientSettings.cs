using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using Jellyfin.Plugin.Essentials.Configuration;

namespace Jellyfin.Plugin.Essentials;

/// <summary>What the browser gets: the page settings before sign-in, the script settings after it.</summary>
public static class ClientSettings
{
    /// <summary>
    /// What every page may know before sign-in: which scripts run and the order of the header buttons. The script
    /// settings (some hold keys) come later from the signed-in endpoint.
    /// </summary>
    public static string ForPage(PluginConfiguration config) =>
        JsonSerializer.Serialize(new
        {
            scripts = Scripts.All
                .Where(s => config.EnabledScripts.Contains(s.Key))
                .Select(s => new { key = s.Key, url = Scripts.Url(s) + HourMark() }),
            headerOrder = HeaderOrder(config)
        });

    /// <summary>
    /// Builds the settings for the scripts that run. Each saved path becomes a nested object that the script merges over
    /// its own defaults: "SETTINGS.movies.rowOrder" becomes { "SETTINGS": { "movies": { "rowOrder": ... } } }.
    /// </summary>
    public static JsonObject ForScripts(PluginConfiguration config)
    {
        var result = new JsonObject();
        JsonObject? saved;
        try
        {
            saved = JsonNode.Parse(string.IsNullOrWhiteSpace(config.Settings) ? "{}" : config.Settings) as JsonObject;
        }
        catch (Exception ex) when (ex is JsonException or InvalidOperationException)
        {
            saved = null;
        }

        foreach (var script in Scripts.All.Where(s => config.EnabledScripts.Contains(s.Key)))
        {
            var nested = new JsonObject();
            if (saved?[script.Key] is JsonObject paths)
            {
                foreach (var (path, value) in paths)
                {
                    Put(nested, path.Split('.'), value?.DeepClone());
                }
            }

            result[script.Key] = nested;
        }

        return result;
    }

    /// <summary>The element ids of the header buttons in the chosen order; buttons it does not name come last.</summary>
    private static string[] HeaderOrder(PluginConfiguration config) =>
        config.HeaderOrder
            .Concat(Scripts.HeaderButtons.Select(b => b.Key))
            .Distinct()
            .Select(key => Scripts.HeaderButtons.FirstOrDefault(b => b.Key == key))
            .OfType<HeaderButton>()
            .Select(b => b.ElementId)
            .ToArray();

    // A browser may keep a jsDelivr script for 7 days; the hour (UTC) in the address makes it ask again at most once an
    // hour, so a pushed script arrives within about an hour (jsDelivr's own cache permitting).
    private static string HourMark() => "?h=" + DateTime.UtcNow.ToString("yyyyMMddHH", CultureInfo.InvariantCulture);

    private static void Put(JsonObject target, string[] keys, JsonNode? value)
    {
        for (var i = 0; i < keys.Length - 1; i++)
        {
            if (target[keys[i]] is not JsonObject next)
            {
                next = new JsonObject();
                target[keys[i]] = next;
            }

            target = next;
        }

        target[keys[^1]] = value;
    }
}
