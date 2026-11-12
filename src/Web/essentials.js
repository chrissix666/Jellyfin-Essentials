// Essentials: starts the chosen scripts once a user is signed in and hands each its settings. page.js, loaded before
// it, sets window.JellyfinEssentials = { parts, headerOrder }; the header buttons read headerOrder themselves.
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

  // The one-copy marks of the scripts: a part whose mark is set already runs from elsewhere (an Injector copy) and
  // would ignore the settings here.
  const MARKS = {
    random: '__jfRandomButton', autoscroll: '__jfAutoscrollButton', fullscreen: '__jfFullscreenButton',
    detailsGroupItemsExtended: '__jfDetailsGroupItemsExtended', detailsGroupItemsSets: '__jfDetailsGroupItemsSets',
    mediaInfoRatings: '__jfMediaInfoRatings', hoverPreviewTiles: '__jfCollectionPreviewLoaded', keyboardLibraryNavigation: '__jfKeyboardLibraryNav'
  };

  function load(part) {
    if (window[MARKS[part.key]]) {
      console.warn('Essentials: ' + part.key + ' already runs from another copy (JavaScript Injector?); its settings here are not used');
      return Promise.resolve();
    }
    return new Promise(resolve => {
      const script = document.createElement('script');
      script.src = part.url;
      script.dataset.essentials = part.key;
      script.onload = script.onerror = () => resolve();
      document.head.appendChild(script);
    });
  }

  whenSignedIn().then(async api => {
    try {
      const response = await api.getJSON(api.getUrl('Essentials/ClientConfiguration'));
      essentials.settings = response.settings || {};
    } catch (e) {
      console.warn('Essentials: settings could not be read, the scripts run with their own defaults', e);
    }
    for (const part of essentials.parts) await load(part);
  });
})();
