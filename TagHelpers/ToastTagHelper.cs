using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vlf_4rum.TagHelpers;

/*
 * <luan-toast>
 * ═══════════════════════════════════════════════════════
 *  Prop          Type     Nullable   Default
 * ───────────────────────────────────────────────────────
 *  Variant       string   no         "info"
 *                info | success | warning | danger
 *  Message       string   yes        null
 *  Title         string   yes        null   → fallback về label variant
 *  Dismissible   bool     no         true
 *  Duration      int      no         3000   (ms)
 *  Icon          string   yes        null   → override icon variant
 *  cx            string   yes        null   → extra classes
 * ═══════════════════════════════════════════════════════
 */
[HtmlTargetElement("luan-toast")]
public class ToastTagHelper : BaseTagHelper
{
    public string      Variant     { get; set; } = "info";
    public string?     Message     { get; set; }
    public new string? Title       { get; set; }
    public bool        Dismissible { get; set; } = true;
    public int         Duration    { get; set; } = 2000;
    public string?     Icon        { get; set; }

    private (string accent, string glow, string border, string tint, string icon, string label) VariantConfig => Variant switch
    {
        "success" => ("#3fb950", "rgba(63,185,80,.35)",  "rgba(63,185,80,.6)",  "rgba(63,185,80,.08)",  "✅", "Thành công"),
        "warning" => ("#e3b341", "rgba(227,179,65,.35)", "rgba(227,179,65,.6)", "rgba(227,179,65,.08)", "⚠️", "Cảnh báo"),
        "danger"  => ("#f85149", "rgba(248,81,73,.35)",  "rgba(248,81,73,.6)",  "rgba(248,81,73,.08)",  "❌", "Lỗi"),
        _         => ("#388bfd", "rgba(56,139,253,.35)", "rgba(56,139,253,.6)", "rgba(56,139,253,.08)", "ℹ️", "Thông báo"),
    };

    protected override string RootTag(TagHelperContext context) => "div";

    protected override string BuildClass(TagHelperContext context) =>
        MergeClasses("w-[320px] max-w-[calc(100vw-2rem)]", AddClass);

    protected override string BuildContent(TagHelperContext context)
    {
        var id = $"toast-{Guid.NewGuid():N}";
        var (accent, glow, border, tint, defaultIcon, defaultLabel) = VariantConfig;
        var resolvedIcon  = !string.IsNullOrEmpty(Icon)  ? Icon  : defaultIcon;
        var resolvedTitle = !string.IsNullOrEmpty(Title) ? Title : defaultLabel;

        var dismissBtn = Dismissible
            ? $"""
                <button id="{id}-close"
                        class="absolute top-3 right-3 w-5 h-5 flex items-center justify-center
                               rounded-full text-xs transition-all opacity-50 hover:opacity-100"
                        style="color:{accent}; background:rgba(255,255,255,.06);"
                        aria-label="Đóng">✕</button>
              """
            : "";

        var bodyHtml = !string.IsNullOrEmpty(Message)
            ? $"""
                <div class="h-px w-full" style="background:linear-gradient(90deg,{accent},transparent);"></div>
                <div class="px-4 py-2.5">
                    <p class="text-sm leading-relaxed opacity-[0.75]">{Message}</p>
                </div>
              """
            : "";

        return $"""
            <div id="{id}"
                 data-duration="{Duration}"
                 class="relative rounded-2xl overflow-hidden"
                 style="background: color-mix(in srgb, {tint} 100%, rgba(22,27,34,0.85));
                   backdrop-filter: blur(20px);
                   -webkit-backdrop-filter: blur(20px);
                   border: 1px solid {border};
                   box-shadow: 0 0 12px {glow}, 0 0 32px rgba(0,0,0,.5), inset 0 1px 0 rgba(255,255,255,.06);
                   animation: toastIn .35s cubic-bezier(.21,1.02,.73,1) both;">

                <div class="flex items-center gap-2.5 px-4 py-3 pr-8">
                    <span class="text-base leading-none shrink-0" aria-hidden="true">{resolvedIcon}</span>
                    <p class="text-sm font-semibold leading-tight" style="color:var(--text-primary);">{resolvedTitle}</p>
                </div>

                {bodyHtml}
                {dismissBtn}

                <div class="absolute inset-x-0 top-0 h-px"
                     style="background:linear-gradient(90deg,transparent,{accent},transparent); opacity:.6;"></div>

            </div>
        """;
    }

    protected override void ApplyAttributes(TagHelperOutput output, TagHelperContext context)
    {
        output.Attributes.SetAttribute("role", "status");
        output.Attributes.SetAttribute("aria-live", "polite");
    }
}
