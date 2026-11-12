using System.Net.Mime;
using System.Reflection;
using Jellyfin.Plugin.Essentials.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.Essentials.Controllers;

[ApiController]
[Route("Essentials")]
public class EssentialsController : ControllerBase
{
    /// <summary>Serves the loader script; needs no sign-in, since it holds no settings.</summary>
    [HttpGet("essentials.js")]
    public ActionResult GetLoader()
    {
        var stream = typeof(Plugin).Assembly.GetManifestResourceStream("essentials.js");
        if (stream is null)
        {
            return NotFound();
        }

        Response.Headers.CacheControl = "no-cache";
        return File(stream, "text/javascript");
    }

    /// <summary>Serves page.js, the settings every page may know before sign-in.</summary>
    [HttpGet("page.js")]
    public ActionResult GetPageSettings()
    {
        Response.Headers.CacheControl = "no-store";
        var config = Plugin.Instance?.Configuration ?? new PluginConfiguration();
        return Content("window.JellyfinEssentials=" + ClientSettings.ForPage(config) + ";", "text/javascript");
    }

    /// <summary>The script settings, for every signed-in user; some hold keys, so not before sign-in.</summary>
    [HttpGet("ClientConfiguration")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    public ActionResult GetClientConfiguration()
    {
        var config = Plugin.Instance?.Configuration;
        if (config is null)
        {
            return NotFound();
        }

        Response.Headers.CacheControl = "no-store";
        return Ok(new { settings = ClientSettings.ForScripts(config) });
    }

#if LOCAL_SCRIPTS
    /// <summary>Test builds only: a script straight from the local repository folder, never cached.</summary>
    [HttpGet("local/{key}.js")]
    public ActionResult GetLocalScript(string key)
    {
        var script = Scripts.All.FirstOrDefault(s => s.Key == key);
        var folder = typeof(Plugin).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "LocalScripts")?.Value;
        if (script is null || string.IsNullOrEmpty(folder))
        {
            return NotFound();
        }

        var path = Path.Combine(folder, script.Repository, script.File);
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        Response.Headers.CacheControl = "no-store";
        return PhysicalFile(path, "text/javascript");
    }
#endif
}
