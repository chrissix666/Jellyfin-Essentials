namespace Jellyfin.Plugin.Essentials;

/// <summary>A script of the plugin: where it lives on GitHub.</summary>
public sealed record Part(string Key, string Repository, string File);

/// <summary>A button in Jellyfin's header and its element id; Globe, Cinema Project and Destroy bring their own.</summary>
public sealed record HeaderButton(string Key, string ElementId);

public static class Parts
{
    public const string GitHubUser = "chrissix666";

    public static readonly Part[] All =
    {
        new("random", "Jellyfin-Random-Button-Extended", "Jellyfin-Random-Button-v3+automode+tripleclicktimer.js"),
        new("autoscroll", "Jellyfin-Autoscroll-Button", "Jellyfin-Autoscroll-Button-v1.js"),
        new("fullscreen", "Jellyfin-Fullscreen-Button", "Jellyfin-Fullscreen-Button-v1.js"),
        new("detailsGroupItemsExtended", "Jellyfin-DetailsGroupItems-Extended", "Jellyfin-DetailsGroupItems-Extended-v1.js"),
        new("detailsGroupItemsSets", "Jellyfin-DetailsGroupItems-Sets", "Jellyfin-DetailsGroupItems-Sets-v1.js"),
        new("mediaInfoRatings", "Jellyfin-MediaInfo-Ratings-Enhanced", "Jellyfin-MediaInfo-Ratings-Enhanced-v1.js"),
        new("hoverPreviewTiles", "Jellyfin-HoverPreviewTiles-Extended", "Jellyfin-HoverPreviewTiles-Extended-v1.js"),
        new("keyboardLibraryNavigation", "Jellyfin-Keyboard-LibraryNavigation", "Jellyfin-Keyboard-LibraryNavigation-v1.js")
    };

    public static readonly HeaderButton[] HeaderButtons =
    {
        new("random", "randomMovieButton"),
        new("autoscroll", "jf-scroll-btn"),
        new("fullscreen", "jf-fullscreen-btn"),
        new("globe", "jf-globe-btn"),
        new("cinema", "jf-cinema-btn"),
        new("destroy", "jf-destroy-btn")
    };

    /// <summary>The address the browser loads a script from: jsDelivr, or in a test build the plugin itself.</summary>
    public static string ScriptUrl(Part part)
    {
#if LOCAL_SCRIPTS
        return "../Essentials/local/" + part.Key + ".js";
#else
        return $"https://cdn.jsdelivr.net/gh/{GitHubUser}/{part.Repository}@main/{Uri.EscapeDataString(part.File)}";
#endif
    }
}
