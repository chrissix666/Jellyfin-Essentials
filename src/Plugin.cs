using Jellyfin.Plugin.Essentials.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Essentials;

public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    public static Plugin? Instance { get; private set; }

    public override string Name => "Essentials";

    public override string Description => "Eight additions for Jellyfin Web in one plugin, each with its own switch and settings, and one order for the header buttons.";

    public override Guid Id => Guid.Parse("55e8663a-5672-4656-a3ec-a900d634b73b");

    public override string ConfigurationFileName => "Jellyfin.Plugin.Essentials.xml";

    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = Name,
            EmbeddedResourcePath = GetType().Namespace + ".Configuration.configPage.html"
        };
    }
}
