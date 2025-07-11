/**
 * Helper function to ensure favicon is shown
 */
export function injectFavicon(): void {
  try {
    // Try to force the favicon to update by removing any existing ones
    const existingFavicons = document.querySelectorAll('link[rel*="icon"]');
    console.log(`Found ${existingFavicons.length} existing favicon links`);
    existingFavicons.forEach(el => el.parentNode?.removeChild(el));

    // Add the ICO favicon from assets - this is the one we specifically want to use
    const linkIco = document.createElement('link');
    linkIco.rel = 'icon';
    linkIco.type = 'image/x-icon';
    linkIco.href = 'assets/favicon.ico?' + new Date().getTime(); // Add cache-busting
    document.head.appendChild(linkIco);

    // Add a shortcut icon reference to the same file
    const linkShortcut = document.createElement('link');
    linkShortcut.rel = 'shortcut icon';
    linkShortcut.type = 'image/x-icon';
    linkShortcut.href = 'assets/favicon.ico?' + new Date().getTime(); // Add cache-busting
    document.head.appendChild(linkShortcut);

    console.log('Favicon injection complete - specifically using assets/favicon.ico');
  } catch (error) {
    console.error('Error during favicon injection:', error);
  }
}
