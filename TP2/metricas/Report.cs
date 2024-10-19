using JinianNet.JNTemplate;
using metricas.dto;
using metricas.models;
using metricas.utils;

namespace metricas;

public class Report(List<FileReport> reports)
{
    private const string IndexTemplatePath = "resources/index.html";
    private const string FullTemplatePath ="resources/full-report.html";
    private const string OutDir = "report";
    private const string FullReportDir = "full";
    private const string FormattedFullReportDir = OutDir + "/" + FullReportDir;

    private readonly ITemplate _indexTemplate = Engine.LoadTemplate(IndexTemplatePath);

    public void GenerateReport()
    {
        FilesManager.DeleteDirectory(FormattedFullReportDir);
        CreateIndexFile();
        Parallel.ForEach(reports, CreateFullReportFile);
    }

    private void CreateIndexFile()
    {
        var formattedReports = reports.Select(r => r.ToHtmlIndexFileReport(FullReportDir)).ToList();
        var totalErrorsCount = formattedReports.Sum(f => f.errorsCount);
        var totalWarningsCount = formattedReports.Sum(f => f.warningsCount);
        var totalAnalyzesCount = formattedReports.Sum(f => f.totalAnalyzesCount);
        var overralErrorsPercentage = (int)((1 - (double)(totalErrorsCount + totalWarningsCount) / totalAnalyzesCount) * 100);
        var overralRating = FileReport.GetRatingByPercentage(overralErrorsPercentage);
        var result = HtmlIndexFileReport.GetFormattedClassification(overralRating);
        
        _indexTemplate.Set("reports", formattedReports);
        _indexTemplate.Set("result", result);
        
        FilesManager.SaveFile(OutDir + "/index.html", _indexTemplate.Render());
    }
    
    private void CreateFullReportFile(FileReport report)
    {
        var fullReportTemplate = Engine.LoadTemplate(FullTemplatePath);
        var fileContent = FilesManager.GetFileContent(report.path);
        var formattedCode = HtmlString.FormatHtml(fileContent);
        var fileName = FilesManager.GetFileName(report.path);
        var hashedFilename = report.hashedPath;
        var linesInfo = report.GetFullReportLines(formattedCode);
        
        
        fullReportTemplate.Set("fileName", fileName);
        fullReportTemplate.Set("fullFilePath", report.path);
        fullReportTemplate.Set("lines", linesInfo);
        
        var output = fullReportTemplate.Render();
        
        FilesManager.SaveFile(FormattedFullReportDir, hashedFilename, output);
    }

   
    
    
}