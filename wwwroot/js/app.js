/**
 * app.js — Global utilities, tái sử dụng toàn app
 * Không chứa business logic — chỉ là helpers & base behaviors
 */

/* ══════════════════════════════════════════
   VOTE
   Usage: tự động khởi tạo khi có [data-vote-group]
   HTML:
     <div data-vote-group="post-1">
       <button class="vote-btn vote-up"   data-vote="up">▲ 10</button>
       <button class="vote-btn vote-down" data-vote="down">▼</button>
     </div>
══════════════════════════════════════════ */
const Vote = {
    init() {
        document.querySelectorAll('[data-vote]').forEach(btn => {
            btn.addEventListener('click', () => this.handle(btn));
        });
    },

    handle(btn) {
        const group    = btn.closest('[data-vote-group]');
        const isActive = btn.classList.contains('active');

        group.querySelectorAll('[data-vote]').forEach(b => b.classList.remove('active'));
        if (!isActive) btn.classList.add('active');

        // TODO: gọi API khi có backend
        // const id  = group.dataset.voteGroup;
        // const dir = btn.dataset.vote; // 'up' | 'down'
        // fetch('/api/vote', { method: 'POST', body: JSON.stringify({ id, dir }), headers: { 'Content-Type': 'application/json' } });
    }
};

/* ══════════════════════════════════════════
   TOAST
   Usage: Toast.show('Lưu thành công!', 'success')
   Types: 'info' | 'success' | 'warning' | 'danger'
══════════════════════════════════════════ */
const Toast = {
    _container: null,

    _getContainer() {
        if (!this._container) {
            this._container = document.createElement('div');
            this._container.style.cssText =
                'position:fixed;bottom:24px;right:24px;z-index:9999;display:flex;flex-direction:column;gap:8px;pointer-events:none;';
            document.body.appendChild(this._container);
        }
        return this._container;
    },

    show(message, type = 'info', duration = 3000) {
        const map = {
            info:    { bg: '#0d1f3a', border: '#1c3a6e', color: '#79c0ff', icon: 'ℹ️' },
            success: { bg: '#0d2918', border: '#1a4d2a', color: '#3fb950', icon: '✅' },
            warning: { bg: '#2a1f00', border: '#4a3800', color: '#e3b341', icon: '⚠️' },
            danger:  { bg: '#2a0d0d', border: '#4a1010', color: '#f85149', icon: '❌' },
        };
        const c = map[type] ?? map.info;

        const el = document.createElement('div');
        el.style.cssText = `
            background:${c.bg};border:1px solid ${c.border};color:${c.color};
            border-radius:8px;padding:12px 16px;font-size:14px;font-weight:600;
            font-family:var(--font-body,Manrope,sans-serif);
            display:flex;align-items:center;gap:8px;
            box-shadow:0 4px 12px rgba(0,0,0,.5);
            min-width:240px;max-width:360px;pointer-events:auto;
            transform:translateX(20px);opacity:0;
            transition:transform .2s ease, opacity .2s ease;
        `;
        el.innerHTML = `<span>${c.icon}</span><span>${message}</span>`;

        this._getContainer().appendChild(el);

        // Animate in
        requestAnimationFrame(() => {
            requestAnimationFrame(() => {
                el.style.transform = 'translateX(0)';
                el.style.opacity   = '1';
            });
        });

        // Animate out
        setTimeout(() => {
            el.style.transform = 'translateX(20px)';
            el.style.opacity   = '0';
            setTimeout(() => el.remove(), 200);
        }, duration);
    }
};

/* ══════════════════════════════════════════
   CONFIRM DIALOG
   Usage: Confirm.ask('Xoá bài này?', () => doDelete())
══════════════════════════════════════════ */
const Confirm = {
    ask(message, onConfirm, onCancel = null) {
        const overlay = document.createElement('div');
        overlay.style.cssText =
            'position:fixed;inset:0;background:rgba(0,0,0,.7);z-index:300;display:flex;align-items:center;justify-content:center;padding:24px;';

        overlay.innerHTML = `
            <div style="background:#161b22;border:1px solid #21262d;border-radius:12px;padding:24px;max-width:400px;width:100%;font-family:Manrope,sans-serif;box-shadow:0 8px 32px rgba(0,0,0,.6);">
                <p style="font-size:15px;color:#e6edf3;line-height:1.6;margin-bottom:20px;">${message}</p>
                <div style="display:flex;justify-content:flex-end;gap:10px;">
                    <button id="_cfmCancel" style="background:transparent;border:1px solid #21262d;border-radius:8px;padding:7px 16px;color:#8b949e;font-size:13px;font-weight:600;cursor:pointer;font-family:Manrope,sans-serif;">Huỷ</button>
                    <button id="_cfmOk"     style="background:#da3633;border:none;border-radius:8px;padding:7px 16px;color:#fff;font-size:13px;font-weight:700;cursor:pointer;font-family:Manrope,sans-serif;">Xác nhận</button>
                </div>
            </div>`;

        document.body.appendChild(overlay);

        const close = () => overlay.remove();

        overlay.querySelector('#_cfmOk').addEventListener('click', () => { onConfirm(); close(); });
        overlay.querySelector('#_cfmCancel').addEventListener('click', () => { onCancel?.(); close(); });
        overlay.addEventListener('click', e => { if (e.target === overlay) { onCancel?.(); close(); } });
        document.addEventListener('keydown', function esc(e) {
            if (e.key === 'Escape') { onCancel?.(); close(); document.removeEventListener('keydown', esc); }
        });
    }
};

/* ══════════════════════════════════════════
   FORMAT HELPERS
   Usage:
     Format.number(1500)          → '1.5k'
     Format.timeAgo('2024-01-01') → 'x ngày trước'
══════════════════════════════════════════ */
const Format = {
    number(n) {
        if (n >= 1_000_000) return (n / 1_000_000).toFixed(1) + 'M';
        if (n >= 1_000)     return (n / 1_000).toFixed(1) + 'k';
        return String(n);
    },

    timeAgo(dateStr) {
        const diff = Date.now() - new Date(dateStr).getTime();
        const m    = Math.floor(diff / 60_000);
        if (m < 1)   return 'vừa xong';
        if (m < 60)  return `${m} phút trước`;
        const h = Math.floor(m / 60);
        if (h < 24)  return `${h} giờ trước`;
        const d = Math.floor(h / 24);
        if (d < 30)  return `${d} ngày trước`;
        return new Date(dateStr).toLocaleDateString('vi-VN');
    }
};

/* ══════════════════════════════════════════
   INIT
══════════════════════════════════════════ */
document.addEventListener('DOMContentLoaded', () => {
    Vote.init();
});

/* ══════════════════════════════════════════
   MODAL
   Usage:
     Modal.open('my-modal')
     Modal.close('my-modal')
     Modal.close()   ← đóng tất cả
══════════════════════════════════════════ */
const Modal = {
    open(id) {
        const el = document.getElementById(id);
        if (!el) return console.warn(`Modal #${id} not found`);
        el.classList.add('open');
        document.body.style.overflow = 'hidden';
    },

    close(id = null) {
        if (id) {
            document.getElementById(id)?.classList.remove('open');
        } else {
            document.querySelectorAll('.modal-overlay.open').forEach(el => el.classList.remove('open'));
        }
        // Chỉ restore scroll nếu không còn modal nào mở
        if (!document.querySelector('.modal-overlay.open')) {
            document.body.style.overflow = '';
        }
    }
};

/* ══════════════════════════════════════════
   DROPDOWN
   Usage: tự động, chỉ cần HTML đúng cấu trúc:
     <div class="dropdown">
       <button data-dropdown="my-menu">Toggle</button>
       <div class="dropdown-menu" id="my-menu">...</div>
     </div>
══════════════════════════════════════════ */
const Dropdown = {
    init() {
        document.addEventListener('click', (e) => {
            const trigger = e.target.closest('[data-dropdown]');

            // Đóng tất cả dropdown khác
            document.querySelectorAll('.dropdown-menu.open').forEach(menu => {
                if (!trigger || menu.id !== trigger.dataset.dropdown) {
                    menu.classList.remove('open');
                }
            });

            // Toggle dropdown được click
            if (trigger) {
                const menuId = trigger.dataset.dropdown;
                document.getElementById(menuId)?.classList.toggle('open');
            }
        });
    }
};

/* ══════════════════════════════════════════
   TABS
   Usage: tự động khi có data-tab-group
     <div data-tab-group>
       <button class="tab-btn active" data-tab="tab1">Tab 1</button>
       <button class="tab-btn"        data-tab="tab2">Tab 2</button>
     </div>
     <div id="tab1" data-tab-panel>Nội dung 1</div>
     <div id="tab2" data-tab-panel style="display:none">Nội dung 2</div>
══════════════════════════════════════════ */
const Tabs = {
    init() {
        document.querySelectorAll('[data-tab-group]').forEach(group => {
            group.querySelectorAll('[data-tab]').forEach(btn => {
                btn.addEventListener('click', () => {
                    // Deactivate all
                    group.querySelectorAll('[data-tab]').forEach(b => b.classList.remove('active'));
                    document.querySelectorAll('[data-tab-panel]').forEach(p => p.style.display = 'none');

                    // Activate clicked
                    btn.classList.add('active');
                    const panel = document.getElementById(btn.dataset.tab);
                    if (panel) panel.style.display = '';
                });
            });
        });
    }
};

/* ══════════════════════════════════════════
   ESCAPE KEY — đóng modal/dropdown
══════════════════════════════════════════ */
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
        Modal.close();
        document.querySelectorAll('.dropdown-menu.open').forEach(m => m.classList.remove('open'));
    }
});

/* ══════════════════════════════════════════
   INIT
══════════════════════════════════════════ */
document.addEventListener('DOMContentLoaded', () => {
    console.log("app.js loaded.");
    Vote.init();
    Dropdown.init();
    Tabs.init();
});
