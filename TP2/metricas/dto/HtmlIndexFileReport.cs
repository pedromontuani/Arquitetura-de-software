using metricas.models;

namespace metricas.dto;

public class HtmlIndexFileReport(string filePath, string fullReportPath, int reportPercentage, int errorsPercentage, int errorsCount, int warningsPercentage, int warningsCount, int totalCount, Rating classification)
{   
    public string filePath { get; set; } = InitFilePath(filePath);
    public string fullReportPath { get; set; } = fullReportPath;
    public int percentage { get; set; } = reportPercentage;
    public int errorsPercentage { get; set; } = errorsPercentage;
    public int errorsCount { get; set; } = errorsCount;
    public int warningsPercentage { get; set; } = warningsPercentage;
    public int warningsCount { get; set; } = warningsCount;
    public int totalAnalyzesCount { get; set; } = totalCount;
    public Rating classification { get; set; } = classification;
    public string ratingCssClass { get; set; } = GetRatingCssClass(classification);
    public string formattedClassification { get; set; } = GetFormattedClassification(classification);
    
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

    public static string GetFormattedClassification(Rating rating)
    {
        switch (rating)
        {
            case Rating.Bad:
                return "Ruim";
            case Rating.Medium:
                return "Regular";
            case Rating.Good:
                return "Bom";
            default:
                return "Ótimo";
        }
    }

    private static string GetRatingCssClass(Rating rating)
    {
        switch (rating)
        {
            case Rating.Bad:
                return "low";
            case Rating.Medium:
                return "medium";
            default:
                return "high";
        }
    }
}