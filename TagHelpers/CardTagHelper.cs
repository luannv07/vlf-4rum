// TagHelpers/CardTagHelper.cs
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vlf_4rum.TagHelpers;
/*
 * <luan-card>
 * ═══════════════════════════════════════════════════════
 *  Prop        Type       Nullable   Default
 * ───────────────────────────────────────────────────────
 *  Title       string     yes        null   (từ BaseTagHelper)
 *  Subtitle    string     yes        null
 *  Icon        string     yes        null   → icon header trái
 *  Variant     string     no         "elevated"
 *              elevated | outlined | flat | ghost
 *  Padding     string     no         "md"
 *              none | sm | md | lg
 *  Radius      string     no         "md"
 *              none | sm | md | lg
 *  Block       bool       no         true
 *  Loading     bool       no         false  → spinner overlay
 *  Hoverable   bool       no         false  → hover shadow + border
 *  Href        string     yes        null   → render <a>
 *  cx          string     yes        null   → extra classes
 * ═══════════════════════════════════════════════════════
 */
[HtmlTargetElement("luan-card")]
public class CardTagHelper : BaseTagHelper
{
    // ── Layout ────────────────────────────────────────────
    public string Variant { get; set; } = "elevated"; // elevated | outlined | flat | ghost
    public string Padding { get; set; } = "md";       // none | sm | md | lg
    public string Radius { get; set; } = "md";       // none | sm | md | lg
    public bool Block { get; set; } = true;

    // ── Header ────────────────────────────────────────────
    public string? Subtitle { get; set; }
    public string? Icon { get; set; }

    // ── State ─────────────────────────────────────────────
    public bool Loading { get; set; }
    public bool Hoverable { get; set; }
    public string? Href { get; set; }

    private bool IsLink => !string.IsNullOrEmpty(Href);

    protected override string RootTag(TagHelperContext context) => IsLink ? "a" : "div";

    protected override string BuildClass(TagHelperContext context) => MergeClasses(
        "relative flex flex-col overflow-hidden mx-auto",
        Block ? "w-full" : "inline-flex",
        Ui.CardVariant(Variant),
        Ui.CardRadius(Radius),
        Hoverable || IsLink
            ? "transition-all duration-150 ease-in-out hover:shadow-lg hover:border-[var(--border-focus)] cursor-pointer"
            : "",
        Loading ? "pointer-events-none select-none" : "",
        AddClass
    );

    protected override string BuildContent(TagHelperContext context)
    {
        var html = "";

        // ── Header ────────────────────────────────────────
        var hasHeader = !string.IsNullOrEmpty(Title) || !string.IsNullOrEmpty(Subtitle);
        if (hasHeader)
        {
            var paddingClass = Ui.CardPadding(Padding);
            html += $"<div class=\"flex items-start gap-3 {paddingClass} pb-0\">";

            if (!string.IsNullOrEmpty(Icon))
                html += $"""
                    <span class="mt-0.5 shrink-0 text-[var(--accent-blue)] leading-none"
                          aria-hidden="true">
                        {Icon}
                    </span>
                """;

            html += "<div class=\"flex flex-col gap-0.5 min-w-0\">";

            if (!string.IsNullOrEmpty(Title))
                html += $"""
                    <h3 class="text-base font-semibold text-[var(--text-primary)] leading-snug truncate">
                        {Title}
                    </h3>
                """;

            if (!string.IsNullOrEmpty(Subtitle))
                html += $"""
                    <p class="text-sm text-[var(--text-muted)] leading-snug">
                        {Subtitle}
                    </p>
                """;

            html += "</div></div>";

            // Divider giữa header và body
            html += "<div class=\"border-t border-[var(--border)] mx-0 mt-3\"></div>";
        }

        // ── Body (slot — content từ Razor) ────────────────
        html += $"<div class=\"{Ui.CardPadding(Padding)} flex flex-col gap-4\">__SLOT__</div>";

        // ── Loading overlay ───────────────────────────────
        if (Loading)
            html += """
                <div class="absolute inset-0 bg-[var(--bg-base)]/60 backdrop-blur-[1px]
                            flex items-center justify-center z-10"
                     aria-busy="true" role="status">
                    <span class="w-6 h-6 border-2 border-[var(--accent-blue)]
                                 border-t-transparent rounded-full animate-spin"></span>
                </div>
            """;

        return html;
    }

    protected override void ApplyAttributes(TagHelperOutput output, TagHelperContext context)
    {
        if (IsLink)
        {
            output.Attributes.SetAttribute("href", Href);
            output.Attributes.SetAttribute("role", "article");
        }

        if (Loading)
            output.Attributes.SetAttribute("aria-busy", "true");

        if (!string.IsNullOrEmpty(Title))
            output.Attributes.SetAttribute("aria-label", Title);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = RootTag(context);
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", BuildClass(context));

        if (!string.IsNullOrEmpty(Id)) output.Attributes.SetAttribute("id", Id);
        if (!string.IsNullOrEmpty(Title)) output.Attributes.SetAttribute("title", Title);
        ApplyAttributes(output, context);

        // Inject child content vào slot
        var inner = BuildContent(context);
        var childHtml = output.GetChildContentAsync().Result.GetContent();
        output.Content.SetHtmlContent(inner.Replace("__SLOT__", childHtml));
    }
}