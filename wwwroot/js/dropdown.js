export function initDropdown() {
    const trigger  = document.getElementById('avatar-trigger');
    const dropdown = document.getElementById('avatar-dropdown');
    if (!trigger || !dropdown) return;

    trigger.addEventListener('click', e => {
        e.stopPropagation();
        dropdown.classList.toggle('hidden');
    });

    document.addEventListener('click', () => {
        dropdown.classList.add('hidden');
    });
}

export function initStickyNav() {
    const nav = document.querySelector('nav');
    if (!nav) return;

    // Tạo sentinel — element giữ chỗ cho nav khi fixed
    const sentinel = document.createElement('div');
    sentinel.style.height = nav.offsetHeight + 'px';
    sentinel.style.display = 'none';
    nav.parentNode.insertBefore(sentinel, nav);

    nav.style.transition = 'transform .3s ease, box-shadow .3s ease, background .3s ease';

    let lastScroll = 0;
    let isPinned = false;

    window.addEventListener('scroll', () => {
        const current = window.scrollY;
        const triggerPoint = sentinel.offsetTop;

        if (current < triggerPoint) {
            // Chưa scroll qua nav — về static
            if (isPinned) {
                isPinned = false;
                nav.style.position = 'relative';
                nav.style.top = '';
                nav.style.left = '';
                nav.style.right = '';
                nav.style.transform = 'translateY(0)';
                nav.style.boxShadow = 'none';
                nav.style.background = 'var(--bg-surface)';
                nav.style.backdropFilter = 'none';
                nav.style.zIndex = '';
                sentinel.style.display = 'none';
            }
        } else {
            // Đã scroll qua nav — pin
            if (!isPinned) {
                isPinned = true;
                sentinel.style.display = 'block';
                nav.style.position = 'fixed';
                nav.style.top = '0';
                nav.style.left = '0';
                nav.style.right = '0';
                nav.style.zIndex = '40';
                nav.style.background = 'rgba(22,27,34,0.88)';
                nav.style.backdropFilter = 'blur(12px)';
                nav.style.webkitBackdropFilter = 'blur(12px)';
                nav.style.boxShadow = '0 1px 0 var(--border-muted), 0 4px 16px rgba(0,0,0,.35)';
            }

            // Cuộn xuống → ẩn, cuộn lên → hiện
            if (current > lastScroll)
                nav.style.transform = 'translateY(-100%)';
            else
                nav.style.transform = 'translateY(0)';
        }

        lastScroll = current;
    });
}