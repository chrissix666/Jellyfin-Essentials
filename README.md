[Jellyfin Projects](https://linktr.ee/JellyfinProjects) | [Kodi Projects](https://linktr.ee/KodiProjects)

---

<img src="logo.png" width="100%">

*Not affiliated with or endorsed by Jellyfin.*

---

Please also have a look at my other plugins:

- [ArtworkPlus](https://github.com/chrissix666/Jellyfin-ArtworkPlus): shows the artwork Jellyfin leaves unused
- [Cinema Project](https://github.com/chrissix666/Jellyfin-Cinema-Project): a virtual cinema environment based on three.js that gives your movies an ambient feel
- [VideoOSD Tweaks and Candy](https://github.com/chrissix666/Jellyfin-VideoOSD-Tweaks-Candy): puts you in full control of the Jellyfin video player OSD
- [Globe](https://github.com/chrissix666/Jellyfin-Globe): the artwork of your libraries on a globe you can turn and zoom
- [Destroy](https://github.com/chrissix666/Jellyfin-Destroy): turns the Jellyfin page you are looking at into a game level
- [VisoMaster Bridge](https://github.com/chrissix666/Jellyfin-VisoMaster-Bridge): swaps faces in the film you are watching, with [VisoMaster Fusion](https://github.com/VisoMasterFusion/VisoMaster-Fusion)

---

# Jellyfin Essentials

- [What This Is](#what-this-is)
- [The Additions](#the-additions)
- [Header Button Order](#header-button-order)
- [Requirements](#requirements)
- [Installation](#installation)
- [Coming from JavaScript Injector](#coming-from-javascript-injector)
- [Settings](#settings)
- [Settings Management](#settings-management)
- [Settings Not Applying?](#settings-not-applying)
- [Limits](#limits)
- [Developed For & Tested On](#developed-for--tested-on)
- [Credits](#credits)
- [License](#license)

---

**Essentials brings eight additions for Jellyfin Web into one plugin: three header buttons, richer detail pages, ratings, hover previews and keyboard navigation. Each one has its own switch and settings in the dashboard, and the header buttons, those of Globe, Cinema Project and Destroy included, stand in the order you choose.**

---

## What This Is

Until now these additions were separate scripts for the JavaScript Injector plugin, each set up by editing values at the top of its code. Essentials loads the same scripts and hands each its settings from a normal plugin page. No code to edit, no library ids to copy, and every addition can be turned on or off on its own.

The scripts still live in their own repositories and keep working on their own with the JavaScript Injector. Essentials loads the current version of each from GitHub, so improvements to a script reach you without a new plugin version.

---

## The Additions

| Addition | What it does | Script |
|---|---|---|
| Random Button Extended | A die in the header opens a random title of the library you are in; a double-click starts auto mode, a triple-click changes its interval | [Jellyfin-Random-Button-Extended](https://github.com/chrissix666/Jellyfin-Random-Button-Extended) |
| Autoscroll Button | Scrolls the page down slowly and starts again at the top; a double-click changes the speed, a triple-click the pause at the top | [Jellyfin-Autoscroll-Button](https://github.com/chrissix666/Jellyfin-Autoscroll-Button) |
| Fullscreen Button | Switches the browser to fullscreen and back, like F11 | [Jellyfin-Fullscreen-Button](https://github.com/chrissix666/Jellyfin-Fullscreen-Button) |
| Details Group Items Extended | Country, awards and box office from OMDb in the details block of movies and series | [Jellyfin-DetailsGroupItems-Extended](https://github.com/chrissix666/Jellyfin-DetailsGroupItems-Extended) |
| Details Group Items Sets | A row in the details block of a movie that links to its collection | [Jellyfin-DetailsGroupItems-Sets](https://github.com/chrissix666/Jellyfin-DetailsGroupItems-Sets) |
| MediaInfo Ratings Enhanced | Ratings from up to eleven sources by MDBList in the media info line of movies and series | [Jellyfin-MediaInfo-Ratings-Enhanced](https://github.com/chrissix666/Jellyfin-MediaInfo-Ratings-Enhanced) |
| Hover Preview Tiles Extended | Resting the mouse on a card fans out its contents or chapters; a click opens or plays | [Jellyfin-HoverPreviewTiles-Extended](https://github.com/chrissix666/Jellyfin-HoverPreviewTiles-Extended) |
| Keyboard Library Navigation | Type a command anywhere and press Enter to open, search, filter, sort or play | [Jellyfin-Keyboard-LibraryNavigation](https://github.com/chrissix666/Jellyfin-Keyboard-LibraryNavigation) |

Each script's own README explains it in full.

---

## Header Button Order

Random, Autoscroll and Fullscreen put a button into Jellyfin's header, and so do Globe, Cinema Project and Destroy. On the General tab you sort all six with the arrows; they stand in that order, left of Jellyfin's own buttons, on every page and in every layout. A button whose plugin is not installed, or an addition that is turned off, simply takes no place.

Without Essentials the buttons keep their fixed order: Random, Autoscroll, Fullscreen, Globe, Cinema Project, Destroy.

---

## Requirements

- Jellyfin 10.10.7 or 12.0+
- The [File Transformation Plugin](https://github.com/IAmParadox27/jellyfin-plugin-file-transformation)
- For Details Group Items Extended: an [OMDb API key](https://www.omdbapi.com/apikey.aspx)
- For MediaInfo Ratings Enhanced: an [MDBList API key](https://mdblist.com)

A word on Jellyfin versions: the plugin comes as two builds, 1.x for 10.10.7 and 2.x for 12.0+. It is one and the same source, and Jellyfin's plugin catalog automatically offers the one that matches your server. Versions in between (like 10.11) are untested.

---

## Installation

Requires the [File Transformation Plugin](https://github.com/IAmParadox27/jellyfin-plugin-file-transformation) to be installed first.

**Via Plugin Catalog (recommended)**

1. In Jellyfin, go to Dashboard > Plugins > Repositories
2. Add a new repository:
   - **Name:** anything you like, for example `Essentials`
   - **URL:**

```
https://raw.githubusercontent.com/chrissix666/Jellyfin-Essentials/main/manifest.json
```

3. Go to the Catalog tab, find Essentials (category: General), and install it
4. Restart Jellyfin
5. Open Dashboard > Plugins > Essentials, pick your libraries for the Random button and add your API keys

**Manual Installation**

1. Download the latest release ZIP from the [Releases page](https://github.com/chrissix666/Jellyfin-Essentials/releases), the one for your Jellyfin version: 1.x for Jellyfin 10.10.7, 2.x for Jellyfin 12.0+
2. Extract the whole ZIP into its own folder inside your Jellyfin plugins folder, for example `plugins/Essentials`
3. Restart Jellyfin

---

## Coming from JavaScript Injector

If you used these scripts with the JavaScript Injector, turn those entries off there. A copy loaded by the Injector does not know the settings in Essentials or your header order, and each script runs only once per page, so whichever copy comes first wins. The General tab of Essentials names the Injector entries that still run one of the eight scripts.

---

## Settings

Every setting lives under Dashboard > Plugins > Essentials:

- **General:** which additions run, the header button order, backup and restore.
- **One tab per addition**, with the values you used to edit at the top of each script, each with a short description. Hover Preview Tiles keeps the look and motion of each card type in a collapsed section.

Settings take effect on the next page load, no restart needed.

The OMDb and MDBList keys are used by the scripts in the browser, so like any key a browser script uses, they are sent to every signed-in user.

---

## Settings Management

### Restore Defaults

Every tab has a **Restore Defaults** button in the top right corner. It resets that tab to the scripts' default values. **Restore All** on the General tab turns every addition back on, restores the default header order and resets every tab. Click **Save** afterwards to keep the result.

### Backup and Restore (All)

The General tab also includes a code-based backup system. **Generate** creates a compact code that represents your complete current settings, **Copy to Clipboard** copies it. Store it somewhere safe. **Import** lets you paste that code back at any time to restore your full configuration, for example after a reinstall or when moving to a new server. After importing, click **Save All Settings**.

---

## Settings Not Applying?

Every once in a while, a saved setting doesn't seem to take effect right away, even after saving and reloading. To be explicit about this: **this is not a bug in Essentials**, it's normal browser caching behavior. Your browser can hang onto an old cached copy of the page or script instead of fetching the new one, and this same thing can happen with other Jellyfin plugins and addons too, not just this one; it's just how browsers work, not something specific to this plugin.

If that happens: open your browser's DevTools (right-click anywhere, **Inspect**), go to the **Network** tab, and check **Disable cache**. Leave DevTools open, don't close it, then refresh the page and open the plugin settings again. With DevTools open and that box checked, the browser is forced to fetch everything fresh instead of reusing anything cached.

**Fixing settings that aren't applying: disable cache workaround**

<img src="https://raw.githubusercontent.com/chrissix666/Jellyfin-Cinema-Project/main/screenshots/settings-cache-workaround.png" width="700" alt="Settings not applying, disable cache workaround">

---

## Limits

- **Jellyfin Web only.** The additions are scripts in Jellyfin Web. Apps that do not load the server's Jellyfin Web page do not show them.
- **Some additions are for desktop browsers.** Autoscroll, Fullscreen, MediaInfo Ratings and Hover Preview Tiles run only in Windows browsers, as their scripts always did.
- **Updates of a script take up to an hour.** The scripts come from GitHub through jsDelivr; a changed script reaches your browser within about an hour.
- **Header order covers six buttons.** Buttons of other plugins or scripts stand right of these six, in their own order.

---

## Developed For & Tested On

- Designed and written for Jellyfin Web 10.10.7 and 12.0+
- Google Chrome
- Windows 11

Other versions may work but are not tested and could lead to unexpected behavior.

---

## Credits

- MediaInfo Ratings Enhanced is based on [jellyfin_ratings](https://github.com/Druidblack/jellyfin_ratings) by [Druidblack](https://github.com/Druidblack), whose rating logos it also uses
- Hover Preview Tiles Extended is based on [Jellyfin – Collection Preview](https://gist.github.com/malte9799/691a52da31f703d176d4f913c5de5fe4) by [malte9799](https://github.com/malte9799)
- Ratings data by [MDBList](https://mdblist.com), country, awards and box office by [OMDb](https://www.omdbapi.com)

---

## License

MIT License

Forking and further development strongly encouraged.
Feedback and bug reports welcome, feel free to open an issue.

---
