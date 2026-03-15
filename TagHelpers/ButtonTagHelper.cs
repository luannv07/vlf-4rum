using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vlf_4rum.TagHelpers;

[HtmlTargetElement("luan-button")]
public class ButtonTagHelper : BaseTagHelper
{
    public string Text { get; set; } = "Button";
    public string? Icon { get; set; }
    public string? IconRight { get; set; }
    public bool IconOnly { get; set; }
    public string Variant { get; set; } = "primary";
    public string Size { get; set; } = "md";
    public bool Block { get; set; }
    public bool Loading { get; set; }
    public bool Disabled { get; set; }
    public string Type { get; set; } = "button";
    public string? Href { get; set; }
    public string? Target { get; set; }
    public string? OnClick { get; set; }

    private bool IsDisabled => Disabled || Loading;
    private bool IsLink => !string.IsNullOrEmpty(Href);

    protected override string RootTag(TagHelperContext context) => IsLink ? "a" : "button";

    protected override string BuildClass(TagHelperContext context) => MergeClasses(
    Block ? "w-full justify-center" : "inline-flex",
    "items-center font-medium leading-none whitespace-nowrap select-none",
    "transition-all duration-150 ease-in-out",
    "focus:outline-none focus-visible:ring-2 focus-visible:ring-[var(--border-focus)]",
    "active:scale-[0.97] rounded-[var(--radius-md)]",
    Ui.Size(Size, IconOnly),       // ← gọn
    Ui.Variant(Variant),           // ← gọn
    IsDisabled ? "opacity-50 cursor-not-allowed pointer-events-none" : "cursor-pointer",
    AddClass
);

    protected override string BuildContent(TagHelperContext context)
    {
        var html = "";
        if (Loading)
            html += $"<span class=\"{Ui.Spinner(Size)} border-current border-t-transparent rounded-full animate-spin\"></span>";
        else if (!string.IsNullOrEmpty(Icon))
            html += $"<span class=\"leading-none\">{Icon}</span>";
        if (!IconOnly)
            html += $"<span>{Text}</span>";
        if (!Loading && !string.IsNullOrEmpty(IconRight))
            html += $"<span class=\"leading-none\">{IconRight}</span>";
        return html;
    }

    protected override void ApplyAttributes(TagHelperOutput output, TagHelperContext context)
    {
        output.Attributes.SetAttribute("aria-label", Text);

        if (IsLink)
        {
            output.Attributes.SetAttribute("href", IsDisabled ? null : Href);
            SetAttrIfNotEmpty(output, "target", Target);
            SetAttrIf(output, Target == "_blank", "rel", "noopener noreferrer");
            SetAttrIf(output, IsDisabled, "aria-disabled", "true");
        }
        else
        {
            output.Attributes.SetAttribute("type", Type);
            SetAttrIf(output, IsDisabled, "disabled", "disabled");
            SetAttrIf(output, Loading, "aria-busy", "true");
        }

        SetAttrIfNotEmpty(output, "onclick", OnClick);
    }

    private string SizeClass() => IconOnly
        ? Size switch
        {
            "xs" => "p-1   text-xs   min-h-[26px] min-w-[26px]",
            "sm" => "p-1.5 text-sm   min-h-[32px] min-w-[32px]",
            "lg" => "p-2.5 text-base min-h-[44px] min-w-[44px]",
            "xl" => "p-3   text-lg   min-h-[52px] min-w-[52px]",
            _ => "p-2   text-sm   min-h-[38px] min-w-[38px]",
        }
        : Size switch
        {
            "xs" => "px-2.5 py-1   text-xs   gap-1   min-h-[26px]",
            "sm" => "px-3   py-1.5 text-sm   gap-1.5 min-h-[32px]",
            "lg" => "px-5   py-2.5 text-base gap-2   min-h-[44px]",
            "xl" => "px-6   py-3   text-lg   gap-2.5 min-h-[52px]",
            _ => "px-4   py-2   text-sm   gap-1.5 min-h-[38px]",
        };

    private string RadiusClass() => "rounded-[var(--radius-md)]";

    private string VariantClass() => Variant switch
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
}