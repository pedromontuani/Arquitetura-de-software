using metricas.utils;

namespace metricas;

static class Program
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
        
        List<A11YAnalyzer> analysis = CreateAnalyzes(path);

        Parallel.ForEach(analysis, a =>
        {
            a.Analyze();
        });
        
        var analyzesResult = analysis.Select(a => a.GetReport()).ToList();
        
        var report = new Report(analyzesResult);
        
        report.GenerateReport();

    }

    static List<A11YAnalyzer> CreateAnalyzes(string path)
    {
        List<A11YAnalyzer> analysis = new();

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