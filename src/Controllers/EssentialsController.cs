using System.Net.Mime;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.Essentials.Controllers;

[ApiController]
[Route("Essentials")]
public class EssentialsController : ControllerBase
{
    /// <summary>The loader that starts the scripts; anonymous, it carries no settings.</summary>
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

    /// <summary>What every page may know before sign-in: which scripts run and the order of the header buttons.</summary>
    [HttpGet("page.js")]
    public ActionResult GetPageSettings()
    {
        Response.Headers.CacheControl = "no-store";
        return Content("window.JellyfinEssentials=" + FileTransformation.IndexHtml.PageSettings() + ";", "text/javascript");
    }

    /// <summary>The scripts' settings, for every signed-in user; some hold keys, so not before sign-in.</summary>
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
        return Ok(new { settings = Settings.ForClient(config) });
    }

#if LOCAL_SCRIPTS
    /// <summary>Test builds only: a script straight from the local repository folder, never cached.</summary>
    [HttpGet("local/{key}.js")]
    public ActionResult GetLocalScript(string key)
    {
        var part = Parts.All.FirstOrDefault(p => p.Key == key);
        var folder = typeof(Plugin).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "LocalScripts")?.Value;
        if (part is null || string.IsNullOrEmpty(folder))
        {
            return NotFound();
        }

        var path = Path.Combine(folder, part.Repository, part.File);
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        Response.Headers.CacheControl = "no-store";
        return PhysicalFile(path, "text/javascript");
    }
#endif
}
