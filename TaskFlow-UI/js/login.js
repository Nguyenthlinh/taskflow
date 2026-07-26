const API_URL = 'https://localhost:7118/api'; // ← Đổi port theo TaskApi của bạn

document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const btn      = document.getElementById('loginBtn');
    const errorMsg = document.getElementById('errorMsg');
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    // Reset error
    errorMsg.style.display = 'none';
    btn.textContent = 'Đang đăng nhập...';
    btn.disabled    = true;

    try {
        const res = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (res.ok) {
            const data = await res.json();

            // Lưu token vào localStorage để dùng ở trang khác
            localStorage.setItem('token', data.token);
            localStorage.setItem('username', data.username);

            // Chuyển sang trang danh sách task
            window.location.href = 'tasks.html';
        } else {
            showError('❌ Sai username hoặc mật khẩu!');
        }

    } catch (err) {
        showError('⚠️ Không kết nối được server. Hãy chắc chắn TaskApi đang chạy!');
    }

    btn.textContent = 'Đăng nhập';
    btn.disabled    = false;
});

function showError(message) {
    const errorMsg = document.getElementById('errorMsg');
    errorMsg.textContent   = message;
    errorMsg.style.display = 'block';
}
