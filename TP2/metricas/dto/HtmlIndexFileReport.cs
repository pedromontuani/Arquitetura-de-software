namespace metricas.dto;

public class HtmlIndexFileReport(string filePath, string fullReportPath, int reportPercentage, int errorsPercentage, int errorsCount, int warningsPercentage, int warningsCount, int totalCount)
{   
    public string filePath { get; set; } = InitFilePath(filePath);
    public string fullReportPath { get; set; } = fullReportPath;
    public int percentage { get; set; } = reportPercentage;
    public int errorsPercentage { get; set; } = errorsPercentage;
    public int errorsCount { get; set; } = errorsCount;
    public int warningsPercentage { get; set; } = warningsPercentage;
    public int warningsCount { get; set; } = warningsCount;
    public int totalAnalyzesCount { get; set; } = totalCount;
    
    private static string InitFilePath(string path)
    {
        if (path.StartsWith("./"))
        {
            return path.Substring(2);
        }

        if (path.StartsWith("/"))
        {
            return path.Substring(1);
        }
        
        return path;
    }
}