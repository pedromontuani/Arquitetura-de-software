using metricas.models;

namespace metricas;

public class Report(List<FileReport> reports)
{
    private List<FileReport> reports = reports;

    public void GenerateReport()
    {
        foreach (var report in reports)
        {
            Console.WriteLine($"Relatório para o arquivo {report.path}");
            Console.WriteLine($"Erros encontrados:");
            foreach (var error in report.errors)
            {
                Console.WriteLine($"- {error.title} (Linha {error.line})");
            }
        }
    }
    
}