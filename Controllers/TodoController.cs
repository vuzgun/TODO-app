using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Authorize] // Require authentication for all actions
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: Todo/Index - Ana todo sayfası
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Görevlerim";
            
            // Get user ID from claims
            var userId = GetCurrentUserId();
            var todos = await _todoService.GetAllTodosByUserIdAsync(userId);
            return View(todos);
        }

        // GET: Todo/Create - Görev oluşturma formu
        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Görev";
            return View();
        }

        // POST: Todo/Create - Görev oluşturma
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createTodoDto);
            }

            var userId = GetCurrentUserId();
            var todo = await _todoService.CreateTodoAsync(createTodoDto, userId);
            
            TempData["Success"] = "Görev başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Edit/5 - Görev düzenleme formu
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.GetTodoByIdAndUserIdAsync(id, userId);
            if (todo == null)
            {
                TempData["Error"] = "Görev bulunamadı veya bu görevi düzenleme yetkiniz yok.";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateTodoDto
            {
                Title = todo.Title,
                Description = todo.Description,
                Priority = todo.Priority,
                IsCompleted = todo.IsCompleted
            };

            ViewData["Title"] = "Görev Düzenle";
            return View(updateDto);
        }

        // POST: Todo/Edit/5 - Görev güncelleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTodoDto updateTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateTodoDto);
            }

            var userId = GetCurrentUserId();
            var todo = await _todoService.UpdateTodoAsync(id, updateTodoDto);
            if (todo == null)
            {
                TempData["Error"] = "Görev bulunamadı veya bu görevi güncelleme yetkiniz yok.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Görev başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Todo/Delete/5 - Görev silme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _todoService.DeleteTodoAsync(id);
            if (!result)
            {
                TempData["Error"] = "Görev bulunamadı veya bu görevi silme yetkiniz yok.";
            }
            else
            {
                TempData["Success"] = "Görev başarıyla silindi.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Todo/Complete/5 - Görevi tamamla
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.MarkTodoAsCompletedAsync(id);
            if (todo == null)
            {
                TempData["Error"] = "Görev bulunamadı veya bu görevi tamamlama yetkiniz yok.";
            }
            else
            {
                TempData["Success"] = "Görev tamamlandı olarak işaretlendi.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Todo/Details/5 - Görev detayları
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.GetTodoByIdAndUserIdAsync(id, userId);
            if (todo == null)
            {
                TempData["Error"] = "Görev bulunamadı veya bu görevi görüntüleme yetkiniz yok.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = "Görev Detayları";
            return View(todo);
        }

        // GET: Todo/Filter - Görev filtreleme
        public async Task<IActionResult> Filter(string filterType, int? priority = null)
        {
            var userId = GetCurrentUserId();
            IEnumerable<TodoResponseDto> todos;
            
            switch (filterType?.ToLower())
            {
                case "completed":
                    todos = await _todoService.GetCompletedTodosByUserIdAsync(userId);
                    ViewData["Title"] = "Tamamlanan Görevler";
                    ViewData["FilterType"] = "completed";
                    break;
                    
                case "pending":
                    todos = await _todoService.GetPendingTodosByUserIdAsync(userId);
                    ViewData["Title"] = "Bekleyen Görevler";
                    ViewData["FilterType"] = "pending";
                    break;
                    
                case "priority" when priority.HasValue && priority >= 1 && priority <= 3:
                    todos = await _todoService.GetTodosByPriorityAndUserIdAsync(priority.Value, userId);
                    ViewData["Title"] = $"Öncelik {priority} Görevler";
                    ViewData["FilterType"] = "priority";
                    ViewData["Priority"] = priority;
                    break;
                    
                default:
                    todos = await _todoService.GetAllTodosByUserIdAsync(userId);
                    ViewData["Title"] = "Tüm Görevler";
                    ViewData["FilterType"] = "all";
                    break;
            }

            return View("Index", todos);
        }

        // GET: Todo/Search - Görev arama
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = GetCurrentUserId();
            // Get all user's todos and filter by search term
            var allTodos = await _todoService.GetAllTodosByUserIdAsync(userId);
            var filteredTodos = allTodos.Where(t => 
                t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (t.Description != null && t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            ViewData["Title"] = $"Arama Sonuçları: {searchTerm}";
            ViewData["SearchTerm"] = searchTerm;
            ViewData["ResultCount"] = filteredTodos.Count;

            return View("Index", filteredTodos);
        }

        // Helper method to get current user ID from claims
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            
            // Fallback to session if claims not available
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            return sessionUserId ?? 0;
        }
    }
}
