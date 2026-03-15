using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vlf_4rum.TagHelpers;

public abstract class BaseTagHelper : TagHelper
{
    public string? Id { get; set; }
    public string? Title { get; set; }

    [HtmlAttributeName("cx")]
    public string? AddClass { get; set; }

    protected abstract string RootTag(TagHelperContext context);
    protected abstract string BuildClass(TagHelperContext context);
    protected abstract string BuildContent(TagHelperContext context);
    protected virtual void ApplyAttributes(TagHelperOutput output, TagHelperContext context) { }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = RootTag(context);
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", BuildClass(context));
        if (!string.IsNullOrEmpty(Id)) output.Attributes.SetAttribute("id", Id);
        if (!string.IsNullOrEmpty(Title)) output.Attributes.SetAttribute("title", Title);
        ApplyAttributes(output, context);
        output.Content.SetHtmlContent(BuildContent(context));
    }

    protected static string ClassIf(bool condition, string cls) => condition ? cls : "";
    protected static string MergeClasses(params string?[] parts) =>
        string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p))).Trim();
    protected static void SetAttrIf(TagHelperOutput output, bool condition, string name, string value)
    {
        if (condition) output.Attributes.SetAttribute(name, value);
    }
    protected static void SetAttrIfNotEmpty(TagHelperOutput output, string name, string? value)
    {
        if (!string.IsNullOrEmpty(value)) output.Attributes.SetAttribute(name, value);
    }
}