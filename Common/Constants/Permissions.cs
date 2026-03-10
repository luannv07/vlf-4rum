namespace VlfForum.Common.Constants;

public static class Permissions
{
    public static class Post
    {
        public const string Create = "post.create";
        public const string Edit = "post.edit";
        public const string Delete = "post.delete";
    }

    public static class Comment
    {
        public const string Create = "comment.create";
        public const string Delete = "comment.delete";
    }

    public static class User
    {
        public const string Ban = "user.ban";
    }
}