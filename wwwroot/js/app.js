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
console.log("loaded");

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
window.addEventListener('DOMContentLoaded', () => {
    Vote.init();
});
