// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Initialize UI counts
document.addEventListener('DOMContentLoaded', function() {
    // Update cart and favorites counts
    if (typeof updateCartCount === 'function') {
        updateCartCount();
    }
    
    if (typeof updateFavoritesCount === 'function') {
        updateFavoritesCount();
    }
});
