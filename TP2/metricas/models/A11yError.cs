using metricas.utils;

namespace metricas.models;

public enum A11YErrorType
{
    ImageAlt,
    SemanticStructure,
    InputProperties,
}

public enum A11YErrorSeverity
{
    Error,
    Warning
}

public class A11YError(string title, int line, A11YErrorType type, A11YErrorSeverity severity)
{
    public string title { get; set; } = HtmlString.ReplaceHtmlSymbols(title);
    public int line { get; set; } = line;
    public A11YErrorType type { get; set; } = type;
    public A11YErrorSeverity severity { get; set; } = severity;
}