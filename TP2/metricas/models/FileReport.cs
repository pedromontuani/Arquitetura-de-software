using System.Collections.Generic;
using metricas.dto;
using metricas.utils;

namespace metricas.models;


public class FileReport(string path, List<A11YError> errors, int total)
{
    public string path { get; set; } = path;
    public string hashedPath { get; set; } = FilesManager.HashFileName(path);
    public List<A11YError> errors { get; set; } = errors;
    public int totalAnalyzed { get; set; } = total;
    
    public HtmlIndexFileReport ToHtmlIndexFileReport(string outDir)
    {
        int errorsCount = errors.Count(e => e.severity == A11YErrorSeverity.Error);
        int warningsCount = errors.Count(e => e.severity == A11YErrorSeverity.Warning);
        int errorsPercentage = (int) Math.Round((double) errorsCount / totalAnalyzed * 100);
        int warningsPercentage = (int) Math.Round((double) warningsCount / totalAnalyzed * 100);
        int reportPercentage = (int)((1 - (double)(errorsCount + ((double) warningsCount / 2)) / totalAnalyzed) * 100);

        Rating rating = GetRatingByPercentage(reportPercentage);
        
        return new HtmlIndexFileReport(path,
            outDir + "/" + hashedPath,
            reportPercentage,
            errorsPercentage,
            errorsCount,
            warningsPercentage,
            warningsCount,
            totalAnalyzed,
            rating);
    }
    
    private Dictionary<int, HtmlFullReportLine> GetErrorsDictionary(string[] lines)
    {
       Dictionary<int, HtmlFullReportLine> dict = new();
       
        foreach (var error in errors)
        {
            if (!dict.ContainsKey(error.line))
            {
                var report = new HtmlFullReportLine(error.line,
                        error.severity == A11YErrorSeverity.Error,
                        error.severity == A11YErrorSeverity.Warning,
                        new List<string>{
                            error.title
                        }, lines[error.line - 1]);
                
                dict.Add(error.line,report);
            }
            else
            {
                dict[error.line].messages.Add(error.title);
            }
        }
        
        return dict;
    }
    
    public List<HtmlFullReportLine> GetFullReportLines(string formattedHtml)
    {
        var lines = formattedHtml.Split("\n").Select(HtmlString.ReplaceHtmlSymbols).ToArray();
        
        var dict = GetErrorsDictionary(lines);

        for(int i=1; i<=lines.Length; i++)
        {
            if (!dict.ContainsKey(i))
            {   
                dict.Add(i, new(i, false, false, new (), lines[i-1]));
            }
        }

        return dict.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToList();
    }
    
    public static Rating GetRatingByPercentage(int percentage)
    {
        if (percentage < 26)
        {
            return Rating.Bad;
        } else if (percentage < 51)
        {
            return Rating.Medium;
        } else if (percentage < 76)
        {
            return Rating.Good;
        } else
        {
            return Rating.Great;
        }
    }
}