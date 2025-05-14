/**
 * Main JavaScript Entry Point
 * This file serves as the main entry point for the TechXpress application
 * It imports and initializes all modules
 */

// Note: Each module handles its own initialization via DOMContentLoaded event
// This file only needs to make sure the modules are loaded in the correct order

// Initialize on page load
document.addEventListener('DOMContentLoaded', function() {
    console.log('TechXpress application initialized');
    
    // All functionality is now modularized into:
    // - ui.js: UI interactions (dropdowns, carousels, scroll events)
    // - favorites.js: Favorites management
    // - cart.js: Shopping cart management
});
