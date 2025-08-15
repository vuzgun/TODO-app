// Main application logic for Todo MVC Application with Cookie Authentication

// Todo management functions
class TodoManager {
    constructor() {
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.loadTodos();
    }

    setupEventListeners() {
        // Add event listeners for todo actions
        document.addEventListener('click', (e) => {
            const completeBtn = e.target.closest('.btn-complete');
            if (completeBtn) {
                this.completeTodo(completeBtn.dataset.id);
                return;
            }

            const deleteBtn = e.target.closest('.btn-delete');
            if (deleteBtn) {
                this.deleteTodo(deleteBtn.dataset.id);
                return;
            }

            const editBtn = e.target.closest('.btn-edit');
            if (editBtn) {
                this.editTodo(editBtn.dataset.id);
            }
        });

        // Add event listeners for filters
        const filterButtons = document.querySelectorAll('.filter-btn');
        filterButtons.forEach(btn => {
            btn.addEventListener('click', (e) => {
                this.filterTodos(e.target.dataset.filter);
            });
        });
    }

    async loadTodos() {
        // Check if user is authenticated via cookie
        if (!this.isUserAuthenticated()) {
            console.log('User not authenticated, redirecting to login');
            window.location.href = '/User/Login';
            return;
        }

        try {
            const response = await fetch('/api/todo', {
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const todos = await response.json();
                this.renderTodos(todos);
            } else if (response.status === 401) {
                // Unauthorized - redirect to login
                window.location.href = '/User/Login';
            } else {
                console.error('Failed to load todos');
            }
        } catch (error) {
            console.error('Error loading todos:', error);
        }
    }

    isUserAuthenticated() {
        // Check if user menu exists and has content
        const userMenu = document.querySelector('.dropdown-toggle');
        return userMenu && userMenu.textContent.trim() !== '';
    }

    renderTodos(todos) {
        const todoContainer = document.getElementById('todo-container');
        if (!todoContainer) return;

        if (todos.length === 0) {
            todoContainer.innerHTML = `
                <div class="text-center py-5">
                    <i class="fas fa-clipboard-list fa-4x text-muted mb-3"></i>
                    <h4 class="text-muted">Henüz görev bulunmuyor</h4>
                    <p class="text-muted">İlk görevinizi oluşturmak için "Yeni Görev" butonuna tıklayın.</p>
                    <a href="/Todo/Create" class="btn btn-primary">
                        <i class="fas fa-plus me-1"></i>İlk Görevi Oluştur
                    </a>
                </div>
            `;
            return;
        }

        const todoCards = todos.map(todo => this.createTodoCard(todo)).join('');
        todoContainer.innerHTML = `
            <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-3">
                ${todoCards}
            </div>
        `;
    }

    createTodoCard(todo) {
        const priorityClass = `priority-${todo.priority}`;
        const completedClass = todo.isCompleted ? 'completed' : '';
        const completedText = todo.isCompleted ? 'Tamamlandı' : 'Bekliyor';
        const completedIcon = todo.isCompleted ? 'check' : 'clock';

        return `
            <div class="col-md-6 col-lg-4 mb-3">
                <div class="card h-100 todo-card ${completedClass} ${todo.isCompleted ? 'border-success' : ''}">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <span class="badge bg-${this.getPriorityColor(todo.priority)}">
                            Öncelik ${todo.priority}
                        </span>
                        <span class="badge bg-${todo.isCompleted ? 'success' : 'secondary'}">
                            <i class="fas fa-${completedIcon} me-1"></i>${completedText}
                        </span>
                    </div>
                    
                    <div class="card-body">
                        <h5 class="card-title ${todo.isCompleted ? 'text-decoration-line-through text-muted' : ''}">
                            ${todo.title}
                        </h5>
                        
                        ${todo.description ? `
                            <p class="card-text ${todo.isCompleted ? 'text-muted' : ''}">
                                ${todo.description.length > 100 ? todo.description.substring(0, 100) + '...' : todo.description}
                            </p>
                        ` : ''}
                        
                        <div class="small text-muted mb-3">
                            <div><i class="fas fa-calendar me-1"></i>Oluşturulma: ${this.formatDate(todo.createdAt)}</div>
                            ${todo.completedAt ? `
                                <div><i class="fas fa-check-circle me-1"></i>Tamamlanma: ${this.formatDate(todo.completedAt)}</div>
                            ` : ''}
                        </div>
                    </div>
                    
                    <div class="card-footer">
                        <div class="btn-group w-100" role="group">
                            <a href="/Todo/Details/${todo.id}" class="btn btn-outline-info btn-sm">
                                <i class="fas fa-eye"></i>
                            </a>
                            
                            ${!todo.isCompleted ? `
                                <a href="/Todo/Edit/${todo.id}" class="btn btn-outline-warning btn-sm">
                                    <i class="fas fa-edit"></i>
                                </a>
                                
                                <button type="button" class="btn btn-outline-success btn-sm btn-complete" data-id="${todo.id}">
                                    <i class="fas fa-check"></i>
                                </button>
                            ` : ''}
                            
                            <button type="button" class="btn btn-outline-danger btn-sm btn-delete" data-id="${todo.id}">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    getPriorityColor(priority) {
        switch (priority) {
            case 1: return 'success';
            case 2: return 'warning';
            case 3: return 'danger';
            default: return 'secondary';
        }
    }

    formatDate(dateString) {
        const date = new Date(dateString);
        return date.toLocaleDateString('tr-TR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    async completeTodo(todoId) {
        // SweetAlert2 confirmation popup
        const result = await Swal.fire({
            title: 'Emin misiniz?',
            text: 'Bu görevi tamamlandı olarak işaretlemek istediğinizden emin misiniz?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#28a745',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Evet, Tamamla',
            cancelButtonText: 'İptal'
        });

        if (!result.isConfirmed) {
            return;
        }

        try {
            const response = await fetch(`/api/todo/${todoId}/complete`, {
                method: 'PATCH',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                this.loadTodos(); // Reload todos
                this.showToast('Görev tamamlandı olarak işaretlendi.', 'success');
            } else if (response.status === 401) {
                // Unauthorized - redirect to login
                window.location.href = '/User/Login';
            } else {
                this.showToast('Görev güncellenirken bir hata oluştu.', 'error');
            }
        } catch (error) {
            console.error('Error completing todo:', error);
            this.showToast('Bir hata oluştu.', 'error');
        }
    }

    async deleteTodo(todoId) {
        // SweetAlert2 confirmation popup
        const result = await Swal.fire({
            title: 'Emin misiniz?',
            text: 'Bu görevi silmek istediğinizden emin misiniz? Bu işlem geri alınamaz!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Evet, Sil',
            cancelButtonText: 'İptal'
        });

        if (!result.isConfirmed) {
            return;
        }

        try {
            const response = await fetch(`/api/todo/${todoId}`, {
                method: 'DELETE',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                this.loadTodos(); // Reload todos
                this.showToast('Görev başarıyla silindi.', 'success');
            } else if (response.status === 401) {
                // Unauthorized - redirect to login
                window.location.href = '/User/Login';
            } else {
                this.showToast('Görev silinirken bir hata oluştu.', 'error');
            }
        } catch (error) {
            console.error('Error deleting todo:', error);
            this.showToast('Bir hata oluştu.', 'error');
        }
    }

    editTodo(todoId) {
        window.location.href = `/Todo/Edit/${todoId}`;
    }

    filterTodos(filter) {
        // Remove active class from all filter buttons
        document.querySelectorAll('.filter-btn').forEach(btn => {
            btn.classList.remove('active');
        });

        // Add active class to clicked button
        event.target.classList.add('active');

        // Apply filter logic here
        // This would typically involve calling the API with filter parameters
        console.log(`Filtering todos by: ${filter}`);
    }

    // SweetAlert2 toast notification
    showToast(message, type = 'info') {
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
}

// Make TodoManager globally available
window.TodoManager = TodoManager;

// Initialize the application
document.addEventListener('DOMContentLoaded', function() {
    // Initialize todo manager if we're on a todo page
    if (window.location.pathname.includes('/Todo')) {
        window.todoManager = new TodoManager();
    }

    // Initialize authentication
    if (typeof auth !== 'undefined') {
        auth.checkAuthStatus();
    }
});

// Utility functions
window.utils = {
    formatDate: function(dateString) {
        const date = new Date(dateString);
        return date.toLocaleDateString('tr-TR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    },

    // SweetAlert2 toast notification
    showToast: function(message, type = 'info') {
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
};
