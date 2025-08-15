// Configuration for Todo MVC Application

const CONFIG = {
    // API base URL - same as the application
    API_BASE_URL: window.location.origin,
    
    // Application settings
    APP_NAME: 'Todo Uygulaması',
    APP_VERSION: '1.0.0',
    
    // Default settings
    DEFAULT_PRIORITY: 2,
    MAX_TITLE_LENGTH: 200,
    MAX_DESCRIPTION_LENGTH: 1000,
    
    // Priority levels
    PRIORITY_LEVELS: {
        1: { name: 'Düşük', color: 'success', icon: 'arrow-down' },
        2: { name: 'Orta', color: 'warning', icon: 'minus' },
        3: { name: 'Yüksek', color: 'danger', icon: 'arrow-up' }
    },
    
    // Status values
    STATUS: {
        PENDING: 'pending',
        COMPLETED: 'completed',
        ACTIVE: 'active',
        INACTIVE: 'inactive'
    },
    
    // Date format
    DATE_FORMAT: 'tr-TR',
    DATE_OPTIONS: {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    },
    
    // Pagination
    ITEMS_PER_PAGE: 10,
    
    // Animation durations
    ANIMATION_DURATION: 300,
    ALERT_TIMEOUT: 5000,
    
    // Local storage keys
    STORAGE_KEYS: {
        THEME: 'theme',
        LANGUAGE: 'language'
    },
    
    // Validation messages
    VALIDATION_MESSAGES: {
        REQUIRED: 'Bu alan zorunludur.',
        MIN_LENGTH: 'En az {0} karakter olmalıdır.',
        MAX_LENGTH: 'En fazla {0} karakter olabilir.',
        EMAIL: 'Geçerli bir e-posta adresi girin.',
        PASSWORD_MISMATCH: 'Şifreler eşleşmiyor.',
        PRIORITY_RANGE: 'Öncelik 1-3 arasında olmalıdır.'
    },
    
    // Success messages
    SUCCESS_MESSAGES: {
        TODO_CREATED: 'Görev başarıyla oluşturuldu.',
        TODO_UPDATED: 'Görev başarıyla güncellendi.',
        TODO_DELETED: 'Görev başarıyla silindi.',
        TODO_COMPLETED: 'Görev tamamlandı olarak işaretlendi.',
        USER_REGISTERED: 'Hesap başarıyla oluşturuldu.',
        USER_LOGGED_IN: 'Başarıyla giriş yapıldı.',
        USER_LOGGED_OUT: 'Başarıyla çıkış yapıldı.'
    },
    
    // Error messages
    ERROR_MESSAGES: {
        GENERAL_ERROR: 'Bir hata oluştu. Lütfen tekrar deneyin.',
        NETWORK_ERROR: 'Ağ hatası oluştu. İnternet bağlantınızı kontrol edin.',
        AUTH_REQUIRED: 'Bu işlem için giriş yapmalısınız.',
        NOT_FOUND: 'İstenen kaynak bulunamadı.',
        VALIDATION_ERROR: 'Lütfen form verilerini kontrol edin.',
        SERVER_ERROR: 'Sunucu hatası oluştu. Lütfen daha sonra tekrar deneyin.'
    },
    
    // API endpoints
    ENDPOINTS: {
        TODO: {
            LIST: '/api/todo',
            COMPLETE: '/api/todo/{id}/complete',
            DELETE: '/api/todo/{id}'
        },
        USER: {
            REGISTER: '/api/user/register',
            LOGIN: '/api/user/login'
        }
    },
    
    // UI settings
    UI: {
        THEME: {
            LIGHT: 'light',
            DARK: 'dark'
        },
        LANGUAGE: {
            TR: 'tr',
            EN: 'en'
        },
        SIDEBAR_COLLAPSED: 'sidebar-collapsed',
        MODAL_BACKDROP: 'modal-backdrop'
    }
};

// Make config available globally
window.CONFIG = CONFIG;

// Utility functions for configuration
window.ConfigUtils = {
    // Get API endpoint with parameters
    getEndpoint: function(endpoint, params = {}) {
        let url = CONFIG.ENDPOINTS[endpoint];
        if (typeof url === 'string') {
            Object.keys(params).forEach(key => {
                url = url.replace(`{${key}}`, params[key]);
            });
            return url;
        }
        return null;
    },
    
    // Get validation message
    getValidationMessage: function(key, ...args) {
        let message = CONFIG.VALIDATION_MESSAGES[key];
        if (message && args.length > 0) {
            args.forEach((arg, index) => {
                message = message.replace(`{${index}}`, arg);
            });
        }
        return message || key;
    },
    
    // Get success message
    getSuccessMessage: function(key) {
        return CONFIG.SUCCESS_MESSAGES[key] || key;
    },
    
    // Get error message
    getErrorMessage: function(key) {
        return CONFIG.ERROR_MESSAGES[key] || key;
    },
    
    // Get priority info
    getPriorityInfo: function(priority) {
        return CONFIG.PRIORITY_LEVELS[priority] || CONFIG.PRIORITY_LEVELS[2];
    },
    
    // Format date
    formatDate: function(date) {
        if (!date) return '';
        const dateObj = new Date(date);
        return dateObj.toLocaleDateString(CONFIG.DATE_FORMAT, CONFIG.DATE_OPTIONS);
    },
    
    // Get storage value
    getStorageValue: function(key) {
        const storageKey = CONFIG.STORAGE_KEYS[key];
        return storageKey ? localStorage.getItem(storageKey) : null;
    },
    
    // Set storage value
    setStorageValue: function(key, value) {
        const storageKey = CONFIG.STORAGE_KEYS[key];
        if (storageKey) {
            localStorage.setItem(storageKey, value);
        }
    },
    
    // Remove storage value
    removeStorageValue: function(key) {
        const storageKey = CONFIG.STORAGE_KEYS[key];
        if (storageKey) {
            localStorage.removeItem(storageKey);
        }
    },

    // SweetAlert2 confirmation popup
    showConfirm: function(title, text, icon = 'question', confirmText = 'Evet', cancelText = 'İptal') {
        return Swal.fire({
            title: title,
            text: text,
            icon: icon,
            showCancelButton: true,
            confirmButtonColor: '#28a745',
            cancelButtonColor: '#6c757d',
            confirmButtonText: confirmText,
            cancelButtonText: cancelText
        });
    },

    // SweetAlert2 toast notification
    showToast: function(message, type = 'info', position = 'top-end') {
        const Toast = Swal.mixin({
            toast: true,
            position: position,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        });

        return Toast.fire({
            icon: type,
            title: message
        });
    },

    // SweetAlert2 success toast
    showSuccess: function(message) {
        return this.showToast(message, 'success');
    },

    // SweetAlert2 error toast
    showError: function(message) {
        return this.showToast(message, 'error');
    },

    // SweetAlert2 warning toast
    showWarning: function(message) {
        return this.showToast(message, 'warning');
    },

    // SweetAlert2 info toast
    showInfo: function(message) {
        return this.showToast(message, 'info');
    }
};

// Initialize configuration
document.addEventListener('DOMContentLoaded', function() {
    // Set default theme if not set
    if (!ConfigUtils.getStorageValue('THEME')) {
        ConfigUtils.setStorageValue('THEME', CONFIG.UI.THEME.LIGHT);
    }
    
    // Set default language if not set
    if (!ConfigUtils.getStorageValue('LANGUAGE')) {
        ConfigUtils.setStorageValue('LANGUAGE', CONFIG.UI.LANGUAGE.TR);
    }
    
    // Apply theme
    const currentTheme = ConfigUtils.getStorageValue('THEME');
    document.body.setAttribute('data-theme', currentTheme);
    
    // Apply language
    const currentLanguage = ConfigUtils.getStorageValue('LANGUAGE');
    document.documentElement.setAttribute('lang', currentLanguage);
});
