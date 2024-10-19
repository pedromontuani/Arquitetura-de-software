using HtmlAgilityPack;
using metricas.models;
using metricas.utils;

namespace metricas;

public class A11yAnalyzer
{
    private HtmlDocument document;
    private string path;
    
    private List<A11YError> errors { get; } = new();
    
    private List<string> correctImages = new();
    private List<string> incorrectImages = new();
    private List<string> correctHeaders = new();
    private List<string> missingSemanticTags = new();
    
    private int totalAnalyzed = 0;
    
    public A11yAnalyzer(string htmlPath)
    {
        this.path = htmlPath;
        var content = FilesManager.GetFileContent(htmlPath);
        this.document = new HtmlDocument();
        this.document.LoadHtml(HtmlString.FormatHtml(content));
    }
    
    public void Analyze()
    {
        CheckImageAltAttributes();
        CheckSemanticStructure();
    }
    
    public FileReport GetReport()
    {
        return new FileReport(path, errors, totalAnalyzed);
    }
    
    private void CheckImageAltAttributes()
    {
        var images = document.DocumentNode.SelectNodes("//img");
        if (images != null)
        {
            foreach (var img in images)
            {
                var altAttribute = img.GetAttributeValue("alt", null);
                int line = img.Line;
                totalAnalyzed++;

                if (string.IsNullOrEmpty(altAttribute))
                {
                    errors.Add(new($"Imagem sem atributo 'alt'", line, A11yErrorType.ImageAlt, A11yErrorSeverity.Error));
                }
                else if (altAttribute == "")
                {
                    errors.Add(new($"Imagem com 'alt' vazio", line, A11yErrorType.ImageAlt, A11yErrorSeverity.Error));
                    incorrectImages.Add($"Imagem com 'alt' vazio. Penalização aplicada. (Linha {line})");
                }
                else
                {
                    correctImages.Add($"Imagem com 'alt' adequado: {altAttribute}. Pontuação adicionada. (Linha {line})");
                }
            }
        }
    }

    private void CheckSemanticStructure()
    {
        string[] semanticTags = { "header", "nav", "main", "footer", "article", "section" };
        foreach (var tag in semanticTags)
        {
            totalAnalyzed++;
            var nodes = document.DocumentNode.SelectNodes($"//{tag}");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    correctHeaders.Add($"Tag semântica <{tag}> encontrada. Pontuação adicionada. (Linha {node.Line})");
                }
            }
            else
            {
                errors.Add(new($"Tag semântica <{tag}> ausente", 0, A11yErrorType.SemanticStructure, A11yErrorSeverity.Error));
                missingSemanticTags.Add($"Tag semântica <{tag}> ausente. Penalização aplicada.");
            }
        }

        CheckHeaderHierarchy();
    }

    private void CheckHeaderHierarchy()
    {
        var headers = document.DocumentNode.SelectNodes("//h1|//h2|//h3|//h4|//h5|//h6");
        if (headers != null)
        {
            foreach (var header in headers)
            {
                totalAnalyzed++;
                correctHeaders.Add($"Cabeçalho {header.Name} está correto. Pontuação adicionada. (Linha {header.Line})");
            }
        }
    }
}