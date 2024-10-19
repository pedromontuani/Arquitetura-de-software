namespace metricas.dto;

public class HtmlFullReportLine(int lineNumber, bool isError, bool isWarning, List<string> messages, string code)
{
    public int number { get; set; } = lineNumber;
    public bool isError { get; set; } = isError;
    public bool isWarning { get; set; } = isWarning;
    public string code { get; set; } = code;
    public List<string> messages { get; set; } = messages;
}