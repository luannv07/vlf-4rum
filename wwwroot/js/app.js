import { initToasts } from './toast.js';

document.addEventListener('DOMContentLoaded', () => {
    initToasts();
    initDropdown();
});

document.addEventListener('DOMContentLoaded', () => {
    initToasts();
    // Sau này thêm module khác vào đây
    // import { initDropdown } from './dropdown.js';
    // initDropdown();
});

function initDropdown() {
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