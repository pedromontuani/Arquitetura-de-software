using System.Collections.Generic;
using metricas.dto;
using metricas.utils;

namespace metricas.models;


public class FileReport(string path, List<A11YError> errors, int total)
{
    public string path { get; set; } = path;
    public List<A11YError> errors { get; set; } = errors;
    public int totalAnalyzed { get; set; } = total;
    
    public HtmlIndexFileReport ToHtmlIndexFileReport()
    {
        int errorsCount = errors.Count(e => e.severity == A11yErrorSeverity.Error);
        int warningsCount = errors.Count(e => e.severity == A11yErrorSeverity.Warning);
        int errorsPercentage = (int) Math.Round((double) errorsCount / totalAnalyzed * 100);
        int warningsPercentage = (int) Math.Round((double) warningsCount / totalAnalyzed * 100);
        int reportPercentage = (int)((1 - (double)(errorsCount + warningsCount) / totalAnalyzed) * 100);
        
        return new HtmlIndexFileReport(path, FilesManager.HashFileName(path), reportPercentage, errorsPercentage, errorsCount, warningsPercentage, warningsCount, totalAnalyzed);
    }
    
    private Dictionary<int, HtmlFullReportLine> GetErrorsDictionary(string[] lines)
    {
       Dictionary<int, HtmlFullReportLine> dict = new();
       
        foreach (var error in errors)
        {
            if (!dict.ContainsKey(error.line))
            {
                if (error.line == 38)
                {
                    Console.Write("");
                }
                var report = new HtmlFullReportLine(error.line,
                        error.severity == A11yErrorSeverity.Error,
                        error.severity == A11yErrorSeverity.Warning,
                        new List<string>{
                            error.title
                        }, lines[error.line]);
                
                dict.Add(error.line,report);
            }
            
            dict[error.line].messages.Add(error.title);
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
}