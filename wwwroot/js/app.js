import { initToasts }    from './toast.js';
import { initDropdown, initStickyNav } from './dropdown.js';

document.addEventListener('DOMContentLoaded', () => {
    initToasts();
    initDropdown();
    initStickyNav();
});

document.addEventListener('DOMContentLoaded', () => {
    initToasts();
    // Sau này thêm module khác vào đây
    // import { initDropdown } from './dropdown.js';
    // initDropdown();
});
