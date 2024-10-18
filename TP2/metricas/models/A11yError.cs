namespace metricas.models;

public enum A11yErrorType
{
    ImageAlt,
    SemanticStructure
}

public enum A11yErrorSeverity
{
    Error,
    Warning
}

public class A11YError(string title, int line, A11yErrorType type, A11yErrorSeverity severity)
{
    public string title { get; set; } = title;
    public int line { get; set; } = line;
    public A11yErrorType type { get; set; } = type;
    public A11yErrorSeverity severity { get; set; } = severity;
}