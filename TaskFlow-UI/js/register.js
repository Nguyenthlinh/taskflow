const API_URL = 'https://localhost:7118/api'; // ← Đổi port theo TaskApi của bạn

document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const username   = document.getElementById('username').value.trim();
    const password   = document.getElementById('password').value;
    const confirm    = document.getElementById('confirmPassword').value;
    const btn        = document.getElementById('registerBtn');
    const errorMsg   = document.getElementById('errorMsg');
    const successMsg = document.getElementById('successMsg');

    // Reset messages
    errorMsg.style.display   = 'none';
    successMsg.style.display = 'none';
    clearErrors();

    // ===== VALIDATE PHÍA CLIENT =====
    let hasError = false;

    if (username.length < 3) {
        showFieldError('username', 'Username tối thiểu 3 ký tự');
        hasError = true;
    }

    if (password.length < 6) {
        showFieldError('password', 'Mật khẩu tối thiểu 6 ký tự');
        hasError = true;
    }

    if (password !== confirm) {
        showFieldError('confirmPassword', 'Mật khẩu xác nhận không khớp!');
        hasError = true;
    }

    if (hasError) return;

    // ===== GỌI API ĐĂNG KÝ =====
    btn.textContent = 'Đang đăng ký...';
    btn.disabled    = true;

    try {
        const res = await fetch(`${API_URL}/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (res.ok) {
            // Thành công → hiện thông báo → chuyển sang login sau 2 giây
            successMsg.textContent   = '🎉 Đăng ký thành công! Đang chuyển sang trang đăng nhập...';
            successMsg.style.display = 'block';
            setTimeout(() => window.location.href = 'login.html', 2000);
        } else if (res.status === 409) {
            showError('❌ Username này đã tồn tại, hãy chọn tên khác!');
        } else {
            showError('❌ Đăng ký thất bại. Vui lòng thử lại!');
        }

    } catch (err) {
        showError('⚠️ Không kết nối được server. Hãy chắc chắn TaskApi đang chạy!');
    }

    btn.textContent = 'Tạo tài khoản';
    btn.disabled    = false;
});

// ===== THANH ĐO ĐỘ MẠNH MẬT KHẨU =====
document.getElementById('password').addEventListener('input', (e) => {
    const pwd      = e.target.value;
    const segments = document.querySelectorAll('.strength-segment');
    const label    = document.getElementById('strengthLabel');

    // Tính điểm độ mạnh
    let strength = 0;
    if (pwd.length >= 6)            strength++; // Đủ dài cơ bản
    if (pwd.length >= 10)           strength++; // Dài hơn
    if (/[A-Z]/.test(pwd))         strength++; // Có chữ hoa
    if (/[0-9]/.test(pwd))         strength++; // Có số
    if (/[^A-Za-z0-9]/.test(pwd))  strength++; // Có ký tự đặc biệt

    const colors = ['#EF4444', '#F59E0B', '#F59E0B', '#10B981', '#10B981'];
    const labels = ['', 'Yếu', 'Trung bình', 'Khá', 'Mạnh', 'Rất mạnh'];

    // Tô màu từng segment
    segments.forEach((seg, i) => {
        seg.style.background = i < strength ? colors[strength - 1] : '#E2E8F0';
    });

    label.textContent = pwd.length > 0 ? labels[strength] : '';
    label.style.color = strength > 0 ? colors[strength - 1] : '';
});

// ===== HELPER FUNCTIONS =====
function showError(msg) {
    const el = document.getElementById('errorMsg');
    el.textContent   = msg;
    el.style.display = 'block';
}

function showFieldError(fieldId, msg) {
    const field = document.getElementById(fieldId);
    field.classList.add('error');

    let hint = field.parentElement.nextElementSibling;
    if (!hint || !hint.classList.contains('field-error')) {
        hint = document.createElement('p');
        hint.className = 'field-error';
        hint.style.cssText = 'color:#EF4444; font-size:12px; margin-top:5px;';
        field.parentElement.after(hint);
    }
    hint.textContent = msg;
}

function clearErrors() {
    document.querySelectorAll('.error').forEach(el => el.classList.remove('error'));
    document.querySelectorAll('.field-error').forEach(el => el.remove());
}
