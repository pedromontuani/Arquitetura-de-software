using HtmlAgilityPack;
using metricas;
using metricas.utils;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Insira o caminho do arquivo HTML ou da pasta:");
            return;
        }

        var path = args[0];
        
        if (!FilesManager.IsValidPath(path))
        {
            Console.WriteLine("Caminho inválido.");
            return;
        }
        
        List<A11yAnalyzer> analysis = CreateAnalyzers(path);

        Parallel.ForEach(analysis, a =>
        {
            a.Analyze();
        });
        
        var reports = analysis.Select(a => a.GetReport()).ToList();
        
        Report report = new(reports);
        
        report.GenerateReport();

    }

    static List<A11yAnalyzer> CreateAnalyzers(string path)
    {
        List<A11yAnalyzer> analysis = new();

        if (FilesManager.IsFolder(path))
        {
            var files = FilesManager.GetAllHtmlFilesFromDirectory(path);

            files.ForEach(f =>
            {
                analysis.Add(new(f));
            });
        }
        else
        {
            analysis.Add(new(path));
        }

        return analysis;
    }
}

