/**
 * Search Functionality
 * Manages search operations and localStorage integration for search history
 */

// document.addEventListener('DOMContentLoaded', function() {
//     // Get search form and input elements
//     const searchForm = document.getElementById('searchForm');
//     const searchInput = document.getElementById('searchInput');
    
//     // Load previous search term if exists
//     if (localStorage.getItem('lastSearchTerm')) {
//         searchInput.value = localStorage.getItem('lastSearchTerm');
//     }
    
//     // Save search term to localStorage when form is submitted
//     if (searchForm) {
//         searchForm.addEventListener('submit', function() {
//             if (searchInput.value.trim() !== '') {
//                 localStorage.setItem('lastSearchTerm', searchInput.value.trim());
//             }
//         });
//     }
    
//     // Clear button functionality could be added here
    
//     console.log('Search functionality initialized');
// });
