/**
 * Favorites Module
 * Handles functionality related to user favorites
 */

// Function to update the favorites count in the navbar
function updateFavoritesCount() {
    fetch('/FavouriteList/GetCount', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {
        // Update the favorites count in the navbar
        const favCountElements = document.querySelectorAll('.fav-count');
        favCountElements.forEach(element => {
            element.textContent = data.count;
        });
    })
    .catch(error => console.error('Error fetching favorites count:', error));
}

// Function to toggle favorites
function toggleFavorite(productId, isCurrentlyFavorite) {
    const form = document.getElementById('favForm-' + productId);
    const url = isCurrentlyFavorite ? '/FavouriteList/RemoveFromFavorites' : '/FavouriteList/AddToFavorites';
    
    // Get form data for CSRF token
    const formData = new FormData(form);
    
    // Submit form using AJAX instead of regular form submission
    fetch(url, {
        method: 'POST',
        body: formData,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
    .then(response => {
        if (response.ok) {
            // Update the favorites count
            updateFavoritesCount();
            
            // Update UI for this specific button
            const button = form.querySelector('button');
            if (button) {
                if (isCurrentlyFavorite) {
                    button.innerHTML = '<i class="far fa-heart"></i>';
                    button.onclick = function() { toggleFavorite(productId, false); return false; };
                } else {
                    button.innerHTML = '<i class="fas fa-heart text-danger"></i>';
                    button.onclick = function() { toggleFavorite(productId, true); return false; };
                }
            }
            
            // If we're on the favorites page and removing items, remove the item from the DOM
            if (isCurrentlyFavorite && window.location.pathname.includes('/FavouriteList')) {
                const productCard = document.querySelector(`.col-md-4.col-sm-6[data-product-id="${productId}"]`);
                if (productCard) {
                    // Animate the removal
                    productCard.style.transition = 'opacity 0.3s ease';
                    productCard.style.opacity = '0';
                    
                    // Remove after animation finishes
                    setTimeout(() => {
                        productCard.remove();
                        
                        // If no favorites left, show empty state
                        const remainingFavs = document.querySelectorAll('.favourite-items .col-md-4');
                        if (remainingFavs.length === 0) {
                            const emptyState = `
                                <div class="col-12 text-center py-5">
                                    <i class="fas fa-heart text-primary mb-4" style="font-size: 5rem;"></i>
                                    <h3 class="text-muted">Your favourites list is empty</h3>
                                    <p class="text-muted mb-4">Products you save to your favourites list will appear here</p>
                                    <a href="/Products" class="btn btn-primary rounded py-2 px-4">Browse Products</a>
                                </div>
                            `;
                            document.querySelector('.favourite-items').innerHTML = emptyState;
                        }
                    }, 300);
                }
            }
        }
    })
    .catch(error => console.error('Error toggling favorite:', error));
    
    // Prevent default form submission
    return false;
}

function toggleFavoriteJS(productId, isCurrentlyFavorite) {
  // Get the csrf token from any form on the page
  const token = document.querySelector(
    'input[name="__RequestVerificationToken"]'
  ).value;
  const url = isCurrentlyFavorite
    ? "/FavouriteList/RemoveFromFavorites"
    : "/FavouriteList/AddToFavorites";

  // Create form data with the necessary information
  const formData = new FormData();
  formData.append("productId", productId);
  formData.append(
    "returnUrl",
    window.location.pathname + window.location.search
  );
  formData.append("__RequestVerificationToken", token);

  // Submit form using fetch API
  fetch(url, {
    method: "POST",
    body: formData,
    headers: {
      "X-Requested-With": "XMLHttpRequest",
    },
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.success) {
        // Update the favorites count if function exists
        if (typeof updateFavoritesCount === "function") {
          updateFavoritesCount();
        }

        // Update UI for this specific button
        const favControl = document.getElementById("favControl-" + productId);
        const button = favControl.querySelector("button");

        if (button) {
          if (isCurrentlyFavorite) {
            // Change to non-favorite state
            button.innerHTML = '<i class="far fa-heart"></i>';
            button.onclick = function () {
              toggleFavoriteJS(productId, false);
              return false;
            };
          } else {
            // Change to favorite state
            button.innerHTML = '<i class="fas fa-heart text-danger"></i>';
            button.onclick = function () {
              toggleFavoriteJS(productId, true);
              return false;
            };
          }
        }
      }
    })
    .catch((error) => console.error("Error toggling favorite:", error));
}


// Initialize on page load
document.addEventListener('DOMContentLoaded', function() {
    // Update favorites count
    updateFavoritesCount();
});
