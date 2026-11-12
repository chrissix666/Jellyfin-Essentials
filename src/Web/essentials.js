// Essentials: starts the chosen scripts once a user is signed in and hands each its settings. page.js, loaded before
// it, sets window.JellyfinEssentials = { scripts, headerOrder }; the header buttons read headerOrder themselves.
(function () {
  'use strict';

  const essentials = window.JellyfinEssentials;
  if (!essentials || essentials.started) return;
  essentials.started = true;
  essentials.settings = {};

  function whenSignedIn() {
    return new Promise(resolve => {
      const check = () => {
        const api = window.ApiClient;
        if (api && typeof api.getCurrentUserId === 'function' && api.getCurrentUserId()) resolve(api);
        else setTimeout(check, 500);
      };
      check();
    });
  }

  // Each script sets a global flag so it runs only once. If the flag is already set, another copy (usually from
  // JavaScript Injector) is running, and the settings here would not reach it. The same flags are in the admin page.
  const LOAD_FLAGS = {
    random: '__jfRandomButton', autoscroll: '__jfAutoscrollButton', fullscreen: '__jfFullscreenButton',
    detailsGroupItemsExtended: '__jfDetailsGroupItemsExtended', detailsGroupItemsSets: '__jfDetailsGroupItemsSets',
    mediaInfoRatings: '__jfMediaInfoRatings', hoverPreviewTiles: '__jfCollectionPreviewLoaded', keyboardLibraryNavigation: '__jfKeyboardLibraryNav'
  };

  function load(script) {
    if (window[LOAD_FLAGS[script.key]]) {
      console.warn('Essentials: ' + script.key + ' is already loaded by another copy, probably from JavaScript Injector; its settings here are ignored');
      return Promise.resolve();
    }
    return new Promise(resolve => {
      const tag = document.createElement('script');
      tag.src = script.url;
      tag.dataset.essentials = script.key;
      tag.onload = tag.onerror = () => resolve();
      document.head.appendChild(tag);
    });
  }

  whenSignedIn().then(async api => {
    try {
      const response = await api.getJSON(api.getUrl('Essentials/ClientConfiguration'));
      essentials.settings = response.settings || {};
    } catch (e) {
      console.warn('Essentials: settings could not be read, the scripts run with their own defaults', e);
    }
    for (const script of essentials.scripts) await load(script);
  });
})();
