// Thêm vào file JS global của bạn
document.addEventListener('click', function(e) {
    const btn = e.target.closest('[data-confirm]');
    if (!btn) return;

    const message = btn.getAttribute('data-confirm');
    if (!confirm(message)) {
        e.preventDefault();
        e.stopPropagation();
    }
});