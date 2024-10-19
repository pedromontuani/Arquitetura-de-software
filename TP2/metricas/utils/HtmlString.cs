using AngleSharp.Html;
using AngleSharp.Html.Parser;

namespace metricas.utils;

public static class HtmlString
{
    public static string ReplaceHtmlSymbols(string source)
    {
        return source.Replace("<", "&lt;").Replace(">", "&gt;").Replace("/n", "&nbsp;");
    }
    
    public static string FormatHtml(string source)
    {
        var parser = new HtmlParser();
    
        var document = parser.ParseDocument(source);

        var sw = new StringWriter();
        document.ToHtml(sw, new PrettyMarkupFormatter());

        return sw.ToString();
    }
}