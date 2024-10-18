namespace metricas.models;

public class FileReport(string path, List<A11YError> errors)
{
    public string path { get; set; } = path;
    public List<A11YError> errors { get; set; } = errors;
}