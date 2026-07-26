const API_URL = 'https://localhost:7118/api'; // ← Đổi port theo TaskApi của bạn

// Lấy token + username từ localStorage (đã lưu lúc đăng nhập)
const token    = localStorage.getItem('token');
const username = localStorage.getItem('username');

// Nếu chưa đăng nhập → về trang login
// Tạm comment để xem giao diện không cần đăng nhập
// Khi kết nối API thật thì bỏ comment dòng dưới ra
// if (!token) window.location.href = 'login.html';

let allTasks    = [];  // Lưu toàn bộ task từ API
let editingId   = null; // Id task đang sửa (null = đang tạo mới)
let activeFilter = 'all'; // Tab đang chọn

// ===== KHỞI TẠO TRANG =====
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('username').textContent = username || 'User';
    document.getElementById('avatarLetter').textContent = (username || 'U')[0].toUpperCase();
    loadTasks();
});

// ===== GỌI API LẤY DANH SÁCH TASK =====
async function loadTasks() {
    try {
        const res = await fetch(`${API_URL}/tasks`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (res.status === 401) {
            localStorage.clear();
            window.location.href = 'login.html';
            return;
        }

        allTasks = await res.json();
        updateStats();
        renderTasks(activeFilter);

    } catch (err) {
        console.error('Không kết nối được API:', err);
        // Dùng data mẫu để xem giao diện khi chưa có API
        allTasks = [
            { id: 1, title: 'Học EF Core', description: 'Ôn lại Day 8-9, Migration, Include', isCompleted: false, createdAt: new Date().toISOString() },
            { id: 2, title: 'Xây dựng TaskApi', description: 'Hoàn thành 14 ngày bootcamp', isCompleted: true, createdAt: new Date().toISOString() },
            { id: 3, title: 'Thiết kế UI TaskFlow', description: 'Tách HTML, CSS, JS riêng biệt', isCompleted: false, createdAt: new Date().toISOString() },
        ];
        updateStats();
        renderTasks(activeFilter);
    }
}

// ===== CẬP NHẬT THỐNG KÊ =====
function updateStats() {
    const total     = allTasks.length;
    const pending   = allTasks.filter(t => !t.isCompleted).length;
    const completed = allTasks.filter(t => t.isCompleted).length;

    document.getElementById('statTotal').textContent     = total;
    document.getElementById('statPending').textContent   = pending;
    document.getElementById('statCompleted').textContent = completed;
    document.getElementById('subGreeting').textContent   =
        pending > 0 ? `Bạn có ${pending} task đang làm` : '🎉 Bạn đã hoàn thành tất cả task!';
}

// ===== RENDER DANH SÁCH TASK =====
function renderTasks(filter = 'all') {
    activeFilter = filter;

    // Cập nhật tab active
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.toggle('active', btn.dataset.filter === filter);
    });

    // Lọc tasks
    const filtered = allTasks.filter(t => {
        if (filter === 'pending')   return !t.isCompleted;
        if (filter === 'completed') return t.isCompleted;
        return true;
    });

    const taskList = document.getElementById('taskList');

    if (filtered.length === 0) {
        taskList.innerHTML = `
            <div class="empty-state">
                <div class="empty-icon">📋</div>
                <h3>Chưa có task nào</h3>
                <p>Bấm "+ Thêm task" để bắt đầu!</p>
            </div>`;
        return;
    }

    taskList.innerHTML = filtered.map(task => `
        <div class="task-card ${task.isCompleted ? 'completed' : 'pending'}" id="task-${task.id}">
            <div class="task-checkbox ${task.isCompleted ? 'checked' : ''}"
                onclick="toggleComplete(${task.id}, ${task.isCompleted})">
            </div>
            <div class="task-info">
                <div class="task-title">${escapeHtml(task.title)}</div>
                ${task.description ? `<div class="task-description">${escapeHtml(task.description)}</div>` : ''}
                <div class="task-meta">
                    <span class="badge ${task.isCompleted ? 'badge-done' : 'badge-pending'}">
                        ${task.isCompleted
                            ? '<i class="fa-solid fa-circle-check"></i> Hoàn thành'
                            : '<i class="fa-solid fa-clock"></i> Đang làm'}
                    </span>
                    <span class="task-date"><i class="fa-regular fa-calendar"></i> ${formatDate(task.createdAt)}</span>
                </div>
            </div>
            <div class="task-actions">
                <button class="btn-icon btn-edit" onclick="openEditModal(${task.id})">
                    <i class="fa-solid fa-pen"></i>
                </button>
                <button class="btn-icon btn-delete" onclick="deleteTask(${task.id})">
                    <i class="fa-solid fa-trash"></i>
                </button>
            </div>
        </div>
    `).join('');
}

// ===== TOGGLE HOÀN THÀNH =====
async function toggleComplete(id, currentStatus) {
    const task = allTasks.find(t => t.id === id);
    if (!task) return;

    try {
        await fetch(`${API_URL}/tasks/${id}`, {
            method: 'PUT',
            headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
            body: JSON.stringify({
                title: task.title,
                description: task.description,
                isCompleted: !currentStatus
            })
        });
        await loadTasks(); // Reload lại danh sách
    } catch (err) {
        // Nếu không có API, toggle trực tiếp trên UI
        task.isCompleted = !currentStatus;
        updateStats();
        renderTasks(activeFilter);
    }
}

// ===== MỞ MODAL TẠO MỚI =====
function openCreateModal() {
    editingId = null;
    document.getElementById('modalTitle').textContent = '✨ Thêm task mới';
    document.getElementById('taskTitle').value       = '';
    document.getElementById('taskDesc').value        = '';
    document.getElementById('modalOverlay').classList.add('show');
    document.getElementById('taskTitle').focus();
}

// ===== MỞ MODAL SỬA =====
function openEditModal(id) {
    const task = allTasks.find(t => t.id === id);
    if (!task) return;

    editingId = id;
    document.getElementById('modalTitle').textContent = '✏️ Sửa task';
    document.getElementById('taskTitle').value        = task.title;
    document.getElementById('taskDesc').value         = task.description || '';
    document.getElementById('modalOverlay').classList.add('show');
    document.getElementById('taskTitle').focus();
}

// ===== ĐÓNG MODAL =====
function closeModal() {
    document.getElementById('modalOverlay').classList.remove('show');
}

// Click ra ngoài modal để đóng
document.getElementById('modalOverlay').addEventListener('click', (e) => {
    if (e.target === document.getElementById('modalOverlay')) closeModal();
});

// ===== LƯU TASK (TẠO MỚI / SỬA) =====
async function saveTask() {
    const title = document.getElementById('taskTitle').value.trim();
    const desc  = document.getElementById('taskDesc').value.trim();

    if (!title) {
        document.getElementById('taskTitle').focus();
        return;
    }

    const btn = document.getElementById('btnSave');
    btn.textContent = 'Đang lưu...';
    btn.disabled    = true;

    try {
        if (editingId) {
            // Sửa task
            const task = allTasks.find(t => t.id === editingId);
            await fetch(`${API_URL}/tasks/${editingId}`, {
                method: 'PUT',
                headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
                body: JSON.stringify({ title, description: desc, isCompleted: task.isCompleted })
            });
        } else {
            // Tạo mới
            await fetch(`${API_URL}/tasks`, {
                method: 'POST',
                headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
                body: JSON.stringify({ title, description: desc })
            });
        }
        closeModal();
        await loadTasks();
    } catch (err) {
        // Nếu không có API, cập nhật UI trực tiếp
        if (editingId) {
            const task = allTasks.find(t => t.id === editingId);
            task.title = title; task.description = desc;
        } else {
            allTasks.unshift({ id: Date.now(), title, description: desc, isCompleted: false, createdAt: new Date().toISOString() });
        }
        closeModal();
        updateStats();
        renderTasks(activeFilter);
    }

    btn.textContent = 'Lưu task';
    btn.disabled    = false;
}

// ===== XÓA TASK =====
async function deleteTask(id) {
    if (!confirm('Bạn có chắc muốn xóa task này?')) return;

    try {
        await fetch(`${API_URL}/tasks/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });
        await loadTasks();
    } catch (err) {
        allTasks = allTasks.filter(t => t.id !== id);
        updateStats();
        renderTasks(activeFilter);
    }
}

// ===== ĐĂNG XUẤT =====
function logout() {
    localStorage.clear();
    window.location.href = 'login.html';
}

// ===== HELPER FUNCTIONS =====
function formatDate(dateStr) {
    return new Date(dateStr).toLocaleDateString('vi-VN', {
        day: '2-digit', month: '2-digit', year: 'numeric'
    });
}

function escapeHtml(str) {
    return str.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
}

// Phím tắt: Enter trong modal để lưu, Escape để đóng
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') closeModal();
    if (e.key === 'Enter' && e.ctrlKey) saveTask();
});
