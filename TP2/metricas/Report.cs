using AngleSharp.Html;
using AngleSharp.Html.Parser;
using JinianNet.JNTemplate;
using metricas.dto;
using metricas.models;
using metricas.utils;

namespace metricas;

public class Report(List<FileReport> reports)
{
    private const string indexTemplatePath = "resources/index.html";
    private const string fullTemplatePath ="resources/full-report.html";
    private const string outDir = "report";
    private const string fullReportDir = outDir + "/full";
    
    private List<FileReport> reports = reports;
    private ITemplate indexTemplate = Engine.LoadTemplate(indexTemplatePath);

    public void GenerateReport()
    {
        FilesManager.DeleteDirectory(fullReportDir);
        CreateIndexFile();
        Parallel.ForEach(reports, CreateFullReportFile);
    }
    
    private List<HtmlIndexHeaderItem> GetHeaderItems()
    {
        List<HtmlIndexHeaderItem> headerItems = new();
        
        headerItems.Add(new("teste", "Teste", "test"));
        headerItems.Add(new("teste", "Teste", "test"));
        
        return headerItems;
    }

    private void CreateIndexFile()
    {
        var headerItems = GetHeaderItems();
        var formattedReports = reports.Select(r => r.ToHtmlIndexFileReport()).ToList();
        
        indexTemplate.Set("headerItems", headerItems);
        indexTemplate.Set("reports", formattedReports);
        
        FilesManager.SaveFile(outDir + "/index.html", indexTemplate.Render());
    }
    
    private void CreateFullReportFile(FileReport report)
    {
        var fullReportTemplate = Engine.LoadTemplate(fullTemplatePath);
        var fileContent = FilesManager.GetFileContent(report.path);
        var formattedCode = HtmlString.FormatHtml(fileContent);
        var fileName = FilesManager.GetFileName(report.path);
        var hashedFilename = FilesManager.HashFileName(report.path);
        var linesInfo = report.GetFullReportLines(formattedCode);
        
        
        fullReportTemplate.Set("fileName", fileName);
        fullReportTemplate.Set("fullFilePath", report.path);
        fullReportTemplate.Set("lines", linesInfo);
        
        var output = fullReportTemplate.Render();
        
        FilesManager.SaveFile(fullReportDir, hashedFilename, output);
    }

   
    
    
}