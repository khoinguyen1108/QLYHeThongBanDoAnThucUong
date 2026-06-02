// ====================== TOGGLE PASSWORD VISIBILITY ======================
document.addEventListener('DOMContentLoaded', function () {

    // Kiểm tra xem có trường mật khẩu trên trang hiện tại không
    const passwordInput = document.getElementById('MatKhau');
    const toggleButton = document.getElementById('togglePassword');
    const eyeIcon = document.getElementById('eyeIcon');

    if (passwordInput && toggleButton && eyeIcon) {
        toggleButton.addEventListener('click', function () {
            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                eyeIcon.classList.remove('bi-eye-fill');
                eyeIcon.classList.add('bi-eye-slash-fill');
            } else {
                passwordInput.type = 'password';
                eyeIcon.classList.remove('bi-eye-slash-fill');
                eyeIcon.classList.add('bi-eye-fill');
            }
        });
    }
});