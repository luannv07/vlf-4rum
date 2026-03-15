// TagHelpers/Ui.cs — utility dùng chung
namespace vlf_4rum.TagHelpers;

public static class Ui
{
    public static string Size(string size, bool iconOnly = false) => iconOnly
        ? size switch
        {
            "xs" => "p-1   text-xs   min-h-[26px] min-w-[26px]",
            "sm" => "p-1.5 text-sm   min-h-[32px] min-w-[32px]",
            "lg" => "p-2.5 text-base min-h-[44px] min-w-[44px]",
            "xl" => "p-3   text-lg   min-h-[52px] min-w-[52px]",
            _ => "p-2   text-sm   min-h-[38px] min-w-[38px]",
        }
        : size switch
        {
            "xs" => "px-2.5 py-1   text-xs   gap-1   min-h-[26px]",
            "sm" => "px-3   py-1.5 text-sm   gap-1.5 min-h-[32px]",
            "lg" => "px-5   py-2.5 text-base gap-2   min-h-[44px]",
            "xl" => "px-6   py-3   text-lg   gap-2.5 min-h-[52px]",
            _ => "px-4   py-2   text-sm   gap-1.5 min-h-[38px]",
        };

    public static string Spinner(string size) => size switch
    {
        "xs" => "w-3 h-3 border",
        "sm" => "w-3.5 h-3.5 border",
        "lg" => "w-5 h-5 border-2",
        "xl" => "w-5 h-5 border-2",
        _ => "w-4 h-4 border-2",
    };

    public static string Variant(string variant) => variant switch
    {
        "secondary" => "bg-[var(--bg-elevated)] text-[var(--text-primary)] border border-[var(--border)] hover:bg-[var(--bg-overlay)] hover:border-[var(--border-focus)] hover:text-[var(--accent-teal)]",
        "ghost" => "bg-transparent text-[var(--text-secondary)] border border-transparent hover:bg-[var(--bg-elevated)] hover:text-[var(--text-primary)]",
        "outline" => "bg-transparent text-[var(--accent-blue)] border border-[var(--accent-blue)] hover:bg-[var(--accent-blue)] hover:text-white",
        "danger" => "bg-[var(--accent-red-dk)] text-white border border-[var(--accent-red-dk)] hover:bg-[var(--accent-red)] hover:border-[var(--accent-red)]",
        "success" => "bg-[var(--accent-green-dk)] text-white border border-[var(--accent-green-dk)] hover:bg-[var(--accent-green)] hover:border-[var(--accent-green)]",
        "warning" => "bg-[var(--accent-yellow)] text-[#0d1117] border border-[var(--accent-yellow)] hover:opacity-90",
        "link" => "bg-transparent text-[var(--accent-blue)] border border-transparent hover:underline hover:text-[var(--accent-teal)] p-0 h-auto min-h-0",
        _ => "bg-[var(--accent-blue-dk)] text-white border border-[var(--accent-blue-dk)] hover:bg-[var(--accent-blue)] hover:border-[var(--accent-blue)]",
    };

    // Input → bg-input
    public static string InputBase =>
        "w-full bg-[var(--bg-input)] text-[var(--text-primary)] " +
        "border border-[var(--border-muted)] rounded-[var(--radius-md)] " +
        "placeholder:text-[var(--text-muted)] " +
        "transition-all duration-150 ease-in-out " +
        "focus:outline-none focus:border-[var(--border-focus)] focus:ring-1 focus:ring-[var(--border-focus)] " +
        "disabled:opacity-50 disabled:cursor-not-allowed " +
        "read-only:opacity-70 read-only:cursor-default";

    public static string InputSize(string size) => size switch
    {
        "xs" => "px-2.5 py-1   text-xs   min-h-[26px]",
        "sm" => "px-3   py-1.5 text-sm   min-h-[32px]",
        "lg" => "px-4   py-2.5 text-base min-h-[44px]",
        "xl" => "px-5   py-3   text-lg   min-h-[52px]",
        _ => "px-3.5 py-2   text-sm   min-h-[38px]",
    };

    public static string InputError =>
        "border-[var(--accent-red)] focus:border-[var(--accent-red)] focus:ring-[var(--accent-red)]";

    public static string InputSuccess =>
"border-[var(--accent-green)] focus:border-[var(--accent-green)] focus:ring-[var(--accent-green)]";

    public static string CardVariant(string variant) => variant switch
    {
        "outlined" => "bg-transparent border border-[var(--border-subtle)]",
        "flat" => "bg-[var(--bg-page)] border border-transparent",
        "ghost" => "bg-transparent border border-transparent",
        _ => "bg-[var(--bg-card)] border border-[var(--border-muted)] shadow-md",
    };

    public static string CardRadius(string radius) => radius switch
    {
        "none" => "rounded-none",
        "sm" => "rounded-[var(--radius-sm)]",
        "lg" => "rounded-[var(--radius-lg)]",
        _ => "rounded-[var(--radius-md)]",
    };

    public static string CardPadding(string padding) => padding switch
    {
        "none" => "p-0",
        "sm" => "p-3",
        "lg" => "p-6",
        _ => "p-5",
    };
}