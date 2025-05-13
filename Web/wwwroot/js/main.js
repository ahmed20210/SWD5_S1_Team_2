(function ($) {
    "use strict";
    
    // Dropdown on mouse hover
    $(document).ready(function () {
        function toggleNavbarMethod() {
            if ($(window).width() > 992) {
                $('.navbar .dropdown').on('mouseover', function () {
                    $('.dropdown-toggle', this).trigger('click');
                }).on('mouseout', function () {
                    $('.dropdown-toggle', this).trigger('click').blur();
                });
            } else {
                $('.navbar .dropdown').off('mouseover').off('mouseout');
            }
        }
        toggleNavbarMethod();
        $(window).resize(toggleNavbarMethod);
    });
    
    
    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Vendor carousel
    $('.vendor-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0:{
                items:2
            },
            576:{
                items:3
            },
            768:{
                items:4
            },
            992:{
                items:5
            },
            1200:{
                items:6
            }
        }
    });


    // Related carousel
    $('.related-carousel').owlCarousel({
        loop: true,
        margin: 29,
        nav: false,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:2
            },
            768:{
                items:3
            },
            992:{
                items:4
            }
        }
    });


    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });
    
})(jQuery);


// Cart Management Functions

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
    
    form.action = url;
    
    // Submit form and update counts
    fetch(url, {
        method: 'POST',
        body: new FormData(form)
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
                    button.onclick = function() { toggleFavorite(productId, false); };
                } else {
                    button.innerHTML = '<i class="fas fa-heart text-danger"></i>';
                    button.onclick = function() { toggleFavorite(productId, true); };
                }
            }
            
            // If we're on the favorites page and removing items, consider refreshing
            if (isCurrentlyFavorite && window.location.pathname.includes('/FavouriteList')) {
                window.location.reload();
            }
        }
    })
    .catch(error => console.error('Error toggling favorite:', error));
    
    // Prevent default form submission
    return false;
}

// Store product data in memory cache to avoid multiple API calls
let productCache = {};

// Function to add an item to the cart
function addToCart(productId, productName, price, imageUrl) {
    // Get the cart from local storage or create a new one
    let cart = JSON.parse(localStorage.getItem('cart')) || [];

    // Check if the product is already in the cart
    const existingProductIndex = cart.findIndex(item => item.id === productId);

    if (existingProductIndex !== -1) {
        // If the product is already in the cart, increase its quantity
        cart[existingProductIndex].quantity += 1;
    } else {
        // If the product is not in the cart, add it with a quantity of 1
        const newProduct = { 
            id: productId, 
            name: productName, 
            price: price, 
            imageUrl: imageUrl,
            quantity: 1 
        };
        cart.push(newProduct);
    }

    // Save the updated cart back to local storage
    localStorage.setItem('cart', JSON.stringify(cart));
    
    // Update the UI
    updateCartCount();
    
    // Show a success message
    alert('Product added to cart!');
}

// Function to update the cart count in the navbar
function updateCartCount() {
    // Get the cart from local storage
    const cart = JSON.parse(localStorage.getItem('cart')) || [];

    // Calculate the total quantity of items in the cart
    const totalQuantity = cart.reduce((total, item) => total + item.quantity, 0);

    // Update the cart count in the UI (all elements with the class 'cart-count')
    const cartCountElements = document.querySelectorAll('.cart-count');
    cartCountElements.forEach(element => {
        element.textContent = totalQuantity;
    });
}

// Function to clear the cart
function clearCart() {
    // Clear the cart from local storage
    localStorage.removeItem('cart');

    // Update the cart count in the UI
    updateCartCount();
    
    // If we're on the cart page, reload it to reflect the changes
    if (window.location.pathname.includes('/Cart')) {
        loadCartItems();
    }
}

// Function to remove a specific item from the cart
function removeFromCart(productId) {
    // Get the cart from local storage
    let cart = JSON.parse(localStorage.getItem('cart')) || [];

    // Filter out the item with the specified productId
    cart = cart.filter(item => item.id !== productId);

    // Save the updated cart back to local storage
    localStorage.setItem('cart', JSON.stringify(cart));

    // Update the cart count in the UI
    updateCartCount();
    
    // If we're on the cart page, reload the cart items
    if (window.location.pathname.includes('/Cart')) {
        loadCartItems();
    }
}

// Function to update the quantity of an item in the cart
function updateCartItemQuantity(productId, quantity) {
    // Get the cart from local storage
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    
    // Find the item in the cart
    const itemIndex = cart.findIndex(item => item.id === productId);
    
    if (itemIndex !== -1) {
        if (quantity > 0) {
            // Update the quantity
            cart[itemIndex].quantity = quantity;
        } else {
            // If quantity is 0 or less, remove the item
            cart.splice(itemIndex, 1);
        }
        
        // Save the updated cart back to local storage
        localStorage.setItem('cart', JSON.stringify(cart));
        
        // Update the cart count in the UI
        updateCartCount();
        
        // If we're on the cart page, reload the cart items
        if (window.location.pathname.includes('/Cart')) {
            loadCartItems();
        }
    }
}

// Function to load and display cart items on the cart page
function loadCartItems() {
    const cartItemsContainer = document.getElementById('cart-items-container');
    if (!cartItemsContainer) return;
    
    // Get the cart from local storage
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    
    // Clear the existing items
    cartItemsContainer.innerHTML = '';
    
    // If the cart is empty
    if (cart.length === 0) {
        cartItemsContainer.innerHTML = '<tr><td colspan="5" class="text-center py-5">Your cart is empty</td></tr>';
        updateCartSummary(0);
        return;
    }
    
    // Variables for calculating total
    let subtotal = 0;
    
    // Loop through each item in the cart and add it to the table
    cart.forEach(item => {
        const total = item.price * item.quantity;
        subtotal += total;
        
        const row = document.createElement('tr');
        row.innerHTML = `
            <td class="align-middle">
                <img src="${item.imageUrl || '/img/product-placeholder.jpg'}" alt="${item.name}" style="width: 50px;">
                <span class="ml-2">${item.name}</span>
            </td>
            <td class="align-middle">$${item.price.toFixed(2)}</td>
            <td class="align-middle">
                <div class="input-group quantity mx-auto" style="width: 100px;">
                    <div class="input-group-btn">
                        <button class="btn btn-sm btn-primary btn-minus" onclick="decrementQuantity('${item.id}')">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                    <input type="text" class="form-control form-control-sm bg-secondary border-0 text-center" value="${item.quantity}" id="quantity-${item.id}" onchange="updateItemQuantity('${item.id}')">
                    <div class="input-group-btn">
                        <button class="btn btn-sm btn-primary btn-plus" onclick="incrementQuantity('${item.id}')">
                            <i class="fa fa-plus"></i>
                        </button>
                    </div>
                </div>
            </td>
            <td class="align-middle">$${total.toFixed(2)}</td>
            <td class="align-middle">
                <button class="btn btn-sm btn-danger" onclick="removeFromCart('${item.id}')">
                    <i class="fa fa-times"></i>
                </button>
            </td>
        `;
        
        cartItemsContainer.appendChild(row);
    });
    
    // Update the cart summary
    updateCartSummary(subtotal);
}

// Function to increment quantity
function incrementQuantity(productId) {
    const inputElement = document.getElementById(`quantity-${productId}`);
    const currentValue = parseInt(inputElement.value);
    inputElement.value = currentValue + 1;
    updateItemQuantity(productId);
}

// Function to decrement quantity
function decrementQuantity(productId) {
    const inputElement = document.getElementById(`quantity-${productId}`);
    const currentValue = parseInt(inputElement.value);
    if (currentValue > 1) {
        inputElement.value = currentValue - 1;
        updateItemQuantity(productId);
    }
}

// Function to update item quantity from input field
function updateItemQuantity(productId) {
    const inputElement = document.getElementById(`quantity-${productId}`);
    const quantity = parseInt(inputElement.value);
    if (quantity > 0) {
        updateCartItemQuantity(productId, quantity);
    } else {
        inputElement.value = 1;
        updateCartItemQuantity(productId, 1);
    }
}

// Function to update the cart summary section
function updateCartSummary(subtotal) {
    const subtotalElement = document.getElementById('cart-subtotal');
    const shippingElement = document.getElementById('shipping-cost');
    const totalElement = document.getElementById('cart-total');
    
    if (subtotalElement && shippingElement && totalElement) {
        // Display the subtotal
        subtotalElement.textContent = `$${subtotal.toFixed(2)}`;
        
        // Calculate shipping (free if subtotal is over $100, otherwise $10)
        const shipping = subtotal > 100 ? 0 : 10;
        shippingElement.textContent = shipping === 0 ? 'Free' : `$${shipping.toFixed(2)}`;
        
        // Calculate and display the total
        const total = subtotal + shipping;
        totalElement.textContent = `$${total.toFixed(2)}`;
    }
}

// Initialize cart on page load
document.addEventListener('DOMContentLoaded', function() {
    // Update cart count
    updateCartCount();
    
    // Update favorites count
    updateFavoritesCount();
    
    // If we're on the cart page, load the cart items
    if (window.location.pathname.includes('/Cart')) {
        loadCartItems();
    }
});

// Function to handle checkout process
function proceedToCheckout() {
    // Check if the cart is empty
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    if (cart.length === 0) {
        alert('Your cart is empty. Add some products before checking out.');
        return;
    }
    
    // Redirect to the checkout page
    window.location.href = '/Cart/Checkout';
}

// Function to load cart data on checkout page
function loadCheckoutSummary() {
    // Get the cart from local storage
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    
    // Get the product container element
    const productsContainer = document.querySelector('.border-bottom:has(h6.mb-3)');
    if (!productsContainer) return;
    
    // Clear existing product entries, keeping the header
    const productsHeader = productsContainer.querySelector('h6.mb-3');
    productsContainer.innerHTML = '';
    productsContainer.appendChild(productsHeader);
    
    // Variables for calculating total
    let subtotal = 0;
    
    // Loop through each item in the cart and add it to the summary
    cart.forEach(item => {
        const total = item.price * item.quantity;
        subtotal += total;
        
        const productDiv = document.createElement('div');
        productDiv.className = 'd-flex justify-content-between';
        productDiv.innerHTML = `
            <p>${item.name} (x${item.quantity})</p>
            <p>$${total.toFixed(2)}</p>
        `;
        
        productsContainer.appendChild(productDiv);
    });
    
    // Update the subtotal, shipping, and total
    const subtotalElement = document.querySelector('.d-flex.justify-content-between.mb-3 h6:last-child');
    const shippingElement = document.querySelector('.d-flex.justify-content-between:not(.mb-3) h6:last-child');
    const totalElement = document.querySelector('.d-flex.justify-content-between.mt-2 h5:last-child');
    
    if (subtotalElement && shippingElement && totalElement) {
        // Calculate shipping (free if subtotal is over $100, otherwise $10)
        const shipping = subtotal > 100 ? 0 : 10;
        
        // Update the elements
        subtotalElement.textContent = `$${subtotal.toFixed(2)}`;
        shippingElement.textContent = shipping === 0 ? 'Free' : `$${shipping.toFixed(2)}`;
        totalElement.textContent = `$${(subtotal + shipping).toFixed(2)}`;
    }
}
