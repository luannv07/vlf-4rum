// Constants/AppPermissions.cs
namespace Vlf4rum.Constants
{
    public static class AppPermissions
    {
        // ── Bài viết (Thread) ──────────────────────────────────
        public static class Thread
        {
            public const string View = "thread.view";
            public const string Create = "thread.create";
            public const string Edit = "thread.edit";         // sửa bài của chính mình
            public const string EditAny = "thread.edit.any";    // mod sửa bài của người khác
            public const string Delete = "thread.delete";       // xóa bài của chính mình
            public const string DeleteAny = "thread.delete.any";  // mod xóa bài của người khác
            public const string Pin = "thread.pin";          // ghim bài
            public const string Lock = "thread.lock";         // khóa không cho reply
            public const string Move = "thread.move";         // chuyển sang category khác
        }

        // ── Bình luận / Reply ──────────────────────────────────
        public static class Reply
        {
            public const string View = "reply.view";
            public const string Create = "reply.create";
            public const string Edit = "reply.edit";
            public const string EditAny = "reply.edit.any";
            public const string Delete = "reply.delete";
            public const string DeleteAny = "reply.delete.any";
        }

        // ── Category / Sub-forum ───────────────────────────────
        public static class Category
        {
            public const string View = "category.view";
            public const string Create = "category.create";
            public const string Edit = "category.edit";
            public const string Delete = "category.delete";
        }

        // ── Tài khoản người dùng ───────────────────────────────
        public static class User
        {
            public const string ViewProfile = "user.profile.view";
            public const string EditProfile = "user.profile.edit";    // sửa profile của mình
            public const string EditAny = "user.profile.edit.any"; // admin sửa profile người khác
            public const string Ban = "user.ban";
            public const string Unban = "user.unban";
            public const string AssignRole = "user.role.assign";
            public const string ViewList = "user.list.view";        // xem danh sách toàn bộ user
        }

        // ── Báo cáo vi phạm ───────────────────────────────────
        public static class Report
        {
            public const string Create = "report.create";   // user gửi báo cáo
            public const string View = "report.view";     // mod xem báo cáo
            public const string Resolve = "report.resolve";  // mod xử lý báo cáo
        }

        // ── Reaction / Like ────────────────────────────────────
        public static class Reaction
        {
            public const string Create = "reaction.create";
            public const string Delete = "reaction.delete";  // xóa reaction của mình
        }

        // ── Tag ───────────────────────────────────────────────
        public static class Tag
        {
            public const string View = "tag.view";
            public const string Create = "tag.create";
            public const string Delete = "tag.delete";
            public const string Assign = "tag.assign";  // gán tag vào thread
        }

        // ── Tìm kiếm ──────────────────────────────────────────
        public static class Search
        {
            public const string Basic = "search.basic";    // tìm theo từ khóa
            public const string Advanced = "search.advanced"; // lọc nâng cao theo user, date...
        }

        // ── Thông báo ─────────────────────────────────────────
        public static class Notification
        {
            public const string Receive = "notification.receive";
            public const string Manage = "notification.manage"; // admin gửi thông báo hệ thống
        }

        // ── Quản trị hệ thống ──────────────────────────────────
        public static class Admin
        {
            public const string ViewDashboard = "admin.dashboard.view";
            public const string ViewLogs = "admin.logs.view";
            public const string ManageSettings = "admin.settings.manage";
        }
    }
}