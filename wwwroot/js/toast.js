const _toastContainer = () => document.getElementById('toast-container');

export function dismissToast(id) {
    const el = document.getElementById(id);
    if (!el) return;
    el.style.animation = 'toastOut .25s ease forwards';
    setTimeout(() => {
        el.remove();
        _restack();
    }, 240);
}

function _restack() {
    const container = _toastContainer();
    if (!container) return;
    [...container.children].forEach(el => {
        el.style.transition = 'transform .25s ease, opacity .25s ease';
    });
}

export function initToasts() {
    document.querySelectorAll('[data-duration]').forEach(el => {
        const duration = parseInt(el.getAttribute('data-duration'));
        if (!isNaN(duration) && duration > 0)
            setTimeout(() => dismissToast(el.id), duration);

        const btn = document.getElementById(`${el.id}-close`);
        if (btn) btn.addEventListener('click', () => dismissToast(el.id));
    });
}

window.__toastDismiss = dismissToast;
