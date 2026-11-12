using System.Text.Json;
using System.Text.Json.Nodes;
using Jellyfin.Plugin.Essentials.Configuration;

namespace Jellyfin.Plugin.Essentials;

public static class Settings
{
    /// <summary>
    /// The settings of the scripts that run, each as the nested object its script merges over its defaults:
    /// "SETTINGS.movies.rowOrder" becomes { "SETTINGS": { "movies": { "rowOrder": ... } } }.
    /// </summary>
    public static JsonObject ForClient(PluginConfiguration config)
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

        foreach (var part in Parts.All.Where(p => config.EnabledParts.Contains(p.Key)))
        {
            var nested = new JsonObject();
            if (saved?[part.Key] is JsonObject paths)
            {
                foreach (var (path, value) in paths)
                {
                    Put(nested, path.Split('.'), value?.DeepClone());
                }
            }

            result[part.Key] = nested;
        }

        return result;
    }

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
