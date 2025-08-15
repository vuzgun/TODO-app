// Authentication management for Todo MVC Application with Cookie Authentication

// Check if user is logged in by checking if user menu exists
function checkAuthStatus() {
    const userMenu = document.querySelector('.dropdown-toggle');
    const authButtons = document.querySelectorAll('.nav-link[asp-controller="User"]');
    
    if (userMenu && userMenu.textContent.trim() !== '') {
        // User is logged in
        updateUIForAuthenticatedUser();
        console.log('User authenticated via cookie');
    } else {
        // User is not logged in
        updateUIForGuestUser();
        console.log('User not authenticated');
    }
}

// Update UI for authenticated users
function updateUIForAuthenticatedUser() {
    // Show todo management links
    const todoLinks = document.querySelectorAll('.nav-link[asp-controller="Todo"]');
    todoLinks.forEach(link => {
        link.style.display = 'inline-block';
    });
    
    console.log('UI updated for authenticated user');
}

// Update UI for guests
function updateUIForGuestUser() {
    // Hide todo management links
    const todoLinks = document.querySelectorAll('.nav-link[asp-controller="Todo"]');
    todoLinks.forEach(link => {
        link.style.display = 'none';
    });
    
    console.log('UI updated for guest user');
}

// Login function - redirect to login page
function login() {
    window.location.href = '/User/Login';
}

// Register function - redirect to register page
function register() {
    window.location.href = '/User/Register';
}

// Logout function with SweetAlert2 confirmation
async function logout() {
    // SweetAlert2 confirmation popup
    const result = await Swal.fire({
        title: 'Çıkış yapmak istediğinizden emin misiniz?',
        text: 'Oturumunuz sonlandırılacak.',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Evet, Çıkış Yap',
        cancelButtonText: 'İptal'
    });

    if (result.isConfirmed) {
        // Find and submit the logout form
        const logoutForm = document.querySelector('form[asp-action="Logout"]');
        if (logoutForm) {
            logoutForm.submit();
        } else {
            // Fallback: redirect to logout action
            window.location.href = '/User/Logout';
        }
    }
}

// Get current user ID from the page (if available)
function getCurrentUserId() {
    // Try to get from data attribute or other source
    const userIdElement = document.querySelector('[data-user-id]');
    if (userIdElement) {
        return userIdElement.dataset.userId;
    }
    return null;
}

// Get current username from the page
function getCurrentUsername() {
    const usernameElement = document.querySelector('.dropdown-toggle');
    if (usernameElement) {
        return usernameElement.textContent.trim();
    }
    return null;
}

// Check if user is authenticated
function isAuthenticated() {
    const userMenu = document.querySelector('.dropdown-toggle');
    return userMenu && userMenu.textContent.trim() !== '';
}

// Add authentication headers to fetch requests (for API calls)
function getAuthHeaders() {
    return {
        'Content-Type': 'application/json'
    };
}

// Show toast notification using SweetAlert2
function showToast(message, type = 'info') {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: type,
        title: message
    });
}

// Initialize authentication on page load
document.addEventListener('DOMContentLoaded', function() {
    console.log('Auth.js loaded, checking authentication status...');
    checkAuthStatus();
    
    // Add logout event listener for any logout buttons
    const logoutButtons = document.querySelectorAll('[onclick="logout()"]');
    logoutButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            logout();
        });
    });
});

// Export functions for use in other scripts
window.auth = {
    login,
    register,
    logout,
    checkAuthStatus,
    getCurrentUserId,
    getCurrentUsername,
    isAuthenticated,
    getAuthHeaders,
    showToast
};
