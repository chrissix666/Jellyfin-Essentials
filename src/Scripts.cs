namespace Jellyfin.Plugin.Essentials;

/// <summary>A script of the plugin and where it lives on GitHub.</summary>
public sealed record Script(string Key, string Repository, string File);

/// <summary>
/// A button in Jellyfin's header, by its element id. Globe, Cinema Project and Destroy add their own buttons; Essentials
/// only orders them.
/// </summary>
public sealed record HeaderButton(string Key, string ElementId);

public static class Scripts
{
    public const string GitHubUser = "chrissix666";

    public static readonly Script[] All =
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
        new("destroy", "jf-destroy-btn"),
        new("cinema", "jf-cinema-btn")
    };

    /// <summary>The address the browser loads a script from: jsDelivr, or in a test build the plugin itself.</summary>
    public static string Url(Script script)
    {
#if LOCAL_SCRIPTS
        return "../Essentials/local/" + script.Key + ".js";
#else
        return $"https://cdn.jsdelivr.net/gh/{GitHubUser}/{script.Repository}@main/{Uri.EscapeDataString(script.File)}";
#endif
    }
}
