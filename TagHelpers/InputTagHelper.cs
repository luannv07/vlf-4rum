// TagHelpers/InputTagHelper.cs
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vlf_4rum.TagHelpers;

/*
 * <luan-input>
 * ═══════════════════════════════════════════════════════
 *  Prop          Type       Nullable   Default
 * ───────────────────────────────────────────────────────
 *  asp-for       ModelExpr  yes        null   → tự lấy name/value/label/required/error
 *
 *  -- Identity
 *  Type          string     no         "text"
 *                text | password | email | number | date | search | tel | url
 *                (tự detect từ model nếu dùng asp-for)
 *  Name          string     yes        null   → fallback khi không có asp-for
 *  Value         string     yes        null   → fallback khi không có asp-for
 *
 *  -- Display
 *  Label         string     yes        null   → fallback sang [Display] của model
 *  Placeholder   string     yes        null
 *  Hint          string     yes        null   → text nhỏ dưới input, ẩn khi có Error
 *  Error         string     yes        null   → fallback sang ModelState nếu có asp-for
 *
 *  -- Icons
 *  Icon          string     yes        null   → icon trái (svg/html string)
 *  IconRight     string     yes        null   → icon phải
 *
 *  -- Addons
 *  Prefix        string     yes        null   → text dính trái, vd: "@", "https://"
 *  Suffix        string     yes        null   → text dính phải, vd: ".com", "VNĐ"
 *
 *  -- Behavior
 *  Size          string     no         "md"
 *                xs | sm | md | lg | xl
 *  Block         bool       no         true
 *  Disabled      bool       no         false
 *  Readonly      bool       no         false
 *  Required      bool       no         false  → fallback sang [Required] của model
 *  MaxLength     int        yes        null   → giới hạn ký tự + hiện counter
 *  AutoComplete  string     yes        null
 *
 *  -- State
 *  Loading       bool       no         false  → spinner + disable input
 *  Success       bool       no         false  → border xanh + icon ✓
 *
 *  cx            string     yes        null   → extra classes
 * ═══════════════════════════════════════════════════════
 */
[HtmlTargetElement("luan-input")]
public class InputTagHelper : BaseTagHelper
{
    private readonly IHtmlGenerator _generator;

    public InputTagHelper(IHtmlGenerator generator)
    {
        _generator = generator;
    }

    // ── asp-for ───────────────────────────────────────────
    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    // ── Identity ──────────────────────────────────────────
    public string Type { get; set; } = "text";
    public string? Name { get; set; }
    public string? Value { get; set; }

    // ── Display ───────────────────────────────────────────
    public string? Label { get; set; }
    public string? Placeholder { get; set; }
    public string? Hint { get; set; }  // hướng dẫn nhập, luôn hiện dưới input
    public string? Error { get; set; }  // lỗi validation, ẩn Hint khi có

    // ── Icons ─────────────────────────────────────────────
    public string? Icon { get; set; }  // icon trái (svg/html string)
    public string? IconRight { get; set; }  // icon phải

    // ── Addons ────────────────────────────────────────────
    public string? Prefix { get; set; }  // text dính trái, vd: "@", "https://"
    public string? Suffix { get; set; }  // text dính phải, vd: ".com", "VNĐ"

    // ── Behavior ──────────────────────────────────────────
    public string Size { get; set; } = "md";
    public bool Block { get; set; } = true;
    public bool Disabled { get; set; }
    public bool Readonly { get; set; }
    public bool Required { get; set; }
    public int? MaxLength { get; set; }  // giới hạn ký tự + hiện counter
    public string? AutoComplete { get; set; }

    // ── State ─────────────────────────────────────────────
    public bool Loading { get; set; }  // spinner — dùng khi async validate
    public bool Success { get; set; }  // border xanh khi field hợp lệ

    // ── Resolved values từ asp-for ────────────────────────
    private string ResolvedName => For?.Name ?? Name ?? "";
    private string? ResolvedValue => For?.Model?.ToString() ?? Value;
    private string? ResolvedLabel => Label
        ?? For?.Metadata.DisplayName
        ?? For?.Metadata.PropertyName;
    private bool ResolvedRequired => Required || (For?.Metadata.IsRequired ?? false);

    private string? ResolvedError
    {
        get
        {
            if (!string.IsNullOrEmpty(Error)) return Error;
            if (For == null) return null;
            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out var entry);
            return entry?.Errors.FirstOrDefault()?.ErrorMessage;
        }
    }

    private string ResolvedType
    {
        get
        {
            if (Type != "text") return Type;
            var name = For?.Name ?? "";
            if (name.Contains("Password", StringComparison.OrdinalIgnoreCase)) return "password";
            if (name.Contains("Email", StringComparison.OrdinalIgnoreCase)) return "email";
            var typeName = For?.Metadata.UnderlyingOrModelType?.Name ?? "";
            return typeName switch
            {
                "DateTime" or "DateOnly" => "date",
                "Int32" or "Int64"
                    or "Decimal" or "Double" => "number",
                _ => "text"
            };
        }
    }

    // ── State flags ───────────────────────────────────────
    private bool HasError => !string.IsNullOrEmpty(ResolvedError);
    private bool HasIconLeft => !string.IsNullOrEmpty(Icon);
    private bool HasIconRight => !string.IsNullOrEmpty(IconRight) && !Loading;
    private bool HasPrefix => !string.IsNullOrEmpty(Prefix);
    private bool HasSuffix => !string.IsNullOrEmpty(Suffix);
    private bool ShowCounter => MaxLength.HasValue;

    protected override string RootTag(TagHelperContext context) => "div";

    protected override string BuildClass(TagHelperContext context) =>
        MergeClasses("flex flex-col gap-1", Block ? "w-full" : "inline-flex", AddClass);

    protected override string BuildContent(TagHelperContext context)
    {
        var html = "";

        // ── Label ─────────────────────────────────────────
        if (!string.IsNullOrEmpty(ResolvedLabel))
        {
            var req = ResolvedRequired
                ? "<span class=\"text-[var(--accent-red)] ml-0.5\" aria-hidden=\"true\">*</span>"
                : "";
            html += $"""
                <label for="{ResolvedName}"
                       class="text-sm font-medium text-[var(--text-secondary)] select-none">
                    {ResolvedLabel}{req}
                </label>
            """;
        }

        // ── Input row (prefix + wrapper + suffix) ─────────
        var hasAddon = HasPrefix || HasSuffix;
        if (hasAddon)
            html += "<div class=\"flex items-stretch\">";

        // Prefix addon
        if (HasPrefix)
            html += $"""
                <span class="inline-flex items-center px-3 text-sm
                             text-[var(--text-muted)] bg-[var(--bg-overlay)]
                             border border-r-0 border-[var(--border)]
                             rounded-l-[var(--radius-md)] select-none whitespace-nowrap">
                    {Prefix}
                </span>
            """;

        // Input wrapper
        html += "<div class=\"relative flex items-center flex-1\">";

        // Icon trái
        if (HasIconLeft)
            html += $"""
                <span class="absolute left-3 text-[var(--text-muted)] leading-none pointer-events-none"
                      aria-hidden="true">
                    {Icon}
                </span>
            """;

        // Build input classes
        var paddingLeft = HasIconLeft ? "pl-9" : HasPrefix ? "" : "";
        var paddingRight = HasIconRight || Loading ? "pr-9" : "";
        var stateClass = HasError ? Ui.InputError
                         : Success ? Ui.InputSuccess
                         : "";
        var radiusClass = HasPrefix && HasSuffix ? "rounded-none"
                         : HasPrefix ? "rounded-r-[var(--radius-md)] rounded-l-none"
                         : HasSuffix ? "rounded-l-[var(--radius-md)] rounded-r-none"
                         : "";
        var inputClass = MergeClasses(
            Ui.InputBase, Ui.InputSize(Size),
            paddingLeft, paddingRight,
            stateClass, radiusClass
        );

        var maxLenAttr = MaxLength.HasValue ? $"maxlength=\"{MaxLength}\"" : "";
        var attrs = new List<string?>
        {
            $"type=\"{ResolvedType}\"",
            $"class=\"{inputClass}\"",
            !string.IsNullOrEmpty(ResolvedName)  ? $"id=\"{ResolvedName}\" name=\"{ResolvedName}\"" : null,
            !string.IsNullOrEmpty(ResolvedValue)  ? $"value=\"{ResolvedValue}\""                    : null,
            !string.IsNullOrEmpty(Placeholder)    ? $"placeholder=\"{Placeholder}\""                : null,
            !string.IsNullOrEmpty(AutoComplete)   ? $"autocomplete=\"{AutoComplete}\""              : null,
            maxLenAttr,
            ResolvedRequired ? "required"  : null,
            Disabled || Loading ? "disabled" : null,
            Readonly  ? "readonly"  : null,
            HasError  ? $"aria-invalid=\"true\" aria-describedby=\"{ResolvedName}-error\""  : null,
            ShowCounter ? $"oninput=\"document.getElementById('{ResolvedName}-counter').textContent=this.value.length\"" : null,
        };

        html += $"<input {string.Join(" ", attrs.Where(a => !string.IsNullOrEmpty(a)))} />";

        // Icon phải / Loading spinner
        if (Loading)
            html += $"""
                <span class="absolute right-3 {Ui.Spinner(Size)} border-[var(--text-muted)] border-t-transparent rounded-full animate-spin"
                      aria-hidden="true"></span>
            """;
        else if (HasIconRight)
            html += $"""
                <span class="absolute right-3 text-[var(--text-muted)] leading-none pointer-events-none"
                      aria-hidden="true">
                    {IconRight}
                </span>
            """;

        // Success icon (nếu không có icon right)
        if (Success && !HasError && !HasIconRight && !Loading)
            html += """
                <span class="absolute right-3 text-[var(--accent-green)] leading-none pointer-events-none"
                      aria-hidden="true">✓</span>
            """;

        html += "</div>"; // đóng input wrapper

        // Suffix addon
        if (HasSuffix)
            html += $"""
                <span class="inline-flex items-center px-3 text-sm
                             text-[var(--text-muted)] bg-[var(--bg-overlay)]
                             border border-l-0 border-[var(--border)]
                             rounded-r-[var(--radius-md)] select-none whitespace-nowrap">
                    {Suffix}
                </span>
            """;

        if (hasAddon)
            html += "</div>"; // đóng input row

        // ── Footer row (error/hint + counter) ────────────
        var hasFooter = HasError || !string.IsNullOrEmpty(Hint) || ShowCounter;
        if (hasFooter)
        {
            html += "<div class=\"flex items-start justify-between gap-2\">";

            if (HasError)
                html += $"""
                    <span id="{ResolvedName}-error"
                          class="text-xs text-[var(--accent-red)]"
                          role="alert">
                        {ResolvedError}
                    </span>
                """;
            else if (!string.IsNullOrEmpty(Hint))
                html += $"""
                    <span class="text-xs text-[var(--text-muted)]">{Hint}</span>
                """;
            else
                html += "<span></span>"; // spacer để counter tetap ke kanan

            if (ShowCounter)
                html += $"""
                    <span id="{ResolvedName}-counter"
                          class="text-xs text-[var(--text-muted)] shrink-0 tabular-nums">
                        {ResolvedValue?.Length ?? 0}/{MaxLength}
                    </span>
                """;

            html += "</div>";
        }

        return html;
    }

    protected override void ApplyAttributes(TagHelperOutput output, TagHelperContext context)
    {
        if (!string.IsNullOrEmpty(ResolvedLabel) || HasError)
            output.Attributes.SetAttribute("role", "group");
    }
}