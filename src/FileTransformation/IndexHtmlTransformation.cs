using System.Runtime.Loader;
using System.Text.Json;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Essentials.FileTransformation;

public class ServiceRegistrator : IPluginServiceRegistrator
{
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddHostedService<TransformationRegistration>();
    }
}

/// <summary>
/// Jellyfin Web loads no plugin scripts on its own, so Essentials adds itself to index.html through the File
/// Transformation plugin. It is found by reflection, so Essentials has no hard dependency on it.
/// </summary>
public class TransformationRegistration : IHostedService
{
    private readonly ILogger<TransformationRegistration> _logger;

    public TransformationRegistration(ILogger<TransformationRegistration> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var assemblies = AssemblyLoadContext.All.SelectMany(context => context.Assemblies).ToList();
            var register = assemblies
                .FirstOrDefault(a => a.GetName().Name == "Jellyfin.Plugin.FileTransformation")
                ?.GetType("Jellyfin.Plugin.FileTransformation.PluginInterface")
                ?.GetMethod("RegisterTransformation");
            // The JObject type File Transformation itself expects, whichever Newtonsoft.Json copy it was built against.
            var parse = register?.GetParameters().FirstOrDefault()?.ParameterType
                .GetMethod("Parse", new[] { typeof(string) });
            if (register is null || parse is null)
            {
                _logger.LogWarning("Essentials: File Transformation is not installed, the scripts will not load");
                return Task.CompletedTask;
            }

            var payload = JsonSerializer.Serialize(new
            {
                id = Plugin.Instance!.Id,
                fileNamePattern = "index.html",
                callbackAssembly = typeof(IndexHtml).Assembly.FullName,
                callbackClass = typeof(IndexHtml).FullName,
                callbackMethod = nameof(IndexHtml.Transform)
            });
            register.Invoke(null, new[] { parse.Invoke(null, new object?[] { payload }) });
            _logger.LogInformation("Essentials: index.html transformation registered");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Essentials: registering the index.html transformation failed");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public static class IndexHtml
{
    // Relative to /web/index.html, so it also works when the server runs under a base URL. page.js is loaded without
    // defer, so its settings are there before any deferred script runs; it comes from its own address because the
    // browser keeps index.html under Jellyfin's fixed ETag and would not see a changed setting there.
    private const string ScriptTags = "<script src=\"../Essentials/page.js\"></script><script defer src=\"../Essentials/essentials.js\"></script>";

    /// <summary>
    /// Called by File Transformation with a JObject; returns index.html with the two script tags before its one
    /// "&lt;/body&gt;". Anything else that matches the pattern (Jellyfin's *-index-html.*.chunk.js files) or a page that
    /// already has them comes back unchanged.
    /// </summary>
    public static string Transform(object payload)
    {
        var contents = payload.GetType().GetProperty("Item", new[] { typeof(string) })
            ?.GetValue(payload, new object[] { "contents" })?.ToString();
        if (string.IsNullOrEmpty(contents))
        {
            return contents ?? string.Empty;
        }

        var body = contents.IndexOf("</body>", StringComparison.Ordinal);
        if (body < 0
            || contents.IndexOf("</body>", body + 1, StringComparison.Ordinal) >= 0
            || !contents.TrimStart().StartsWith("<!doctype html", StringComparison.OrdinalIgnoreCase)
            || contents.Contains(ScriptTags, StringComparison.Ordinal))
        {
            return contents;
        }

        return contents.Insert(body, ScriptTags);
    }
}
