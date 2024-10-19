using AngleSharp.Text;
using HtmlAgilityPack;
using metricas.models;
using metricas.utils;

namespace metricas;

public class A11YAnalyzer
{
    private readonly HtmlDocument _document;
    private readonly string _path;
    
    private List<A11YError> errors { get; } = new();
    
    private int _totalAnalyzed = 0;
    
    public A11YAnalyzer(string htmlPath)
    {
        this._path = htmlPath;
        var content = FilesManager.GetFileContent(htmlPath);
        this._document = new HtmlDocument();
        this._document.LoadHtml(HtmlString.FormatHtml(content));
    }
    
    public void Analyze()
    {
        CheckImageAltAttributes();
        CheckSemanticStructure();
        IdentifyInputPurpose();
    }
    
    public FileReport GetReport()
    {
        return new FileReport(_path, errors, _totalAnalyzed);
    }
    
    private void CheckImageAltAttributes()
    {
        var images = _document.DocumentNode.SelectNodes("//img");
        if (images != null)
        {
            foreach (var img in images)
            {
                var altAttribute = img.GetAttributeValue("alt", null);
                int line = img.Line;
                
                _totalAnalyzed++;

                if (string.IsNullOrEmpty(altAttribute))
                {
                    errors.Add(new($"Imagem sem atributo 'alt'", line, A11YErrorType.ImageAlt, A11YErrorSeverity.Error));
                }
                else if (altAttribute == "")
                {
                    errors.Add(new($"Imagem com 'alt' vazio", line, A11YErrorType.ImageAlt, A11YErrorSeverity.Error));
                }
            }
        }
    }

    private void CheckSemanticStructure()
    {
        string[] semanticTags = { "header", "nav", "main", "footer", "article", "section" };
        foreach (var tag in semanticTags)
        {
            _totalAnalyzed++;
            var nodes = _document.DocumentNode.SelectNodes($"//{tag}");
            if (nodes == null)
            {
                errors.Add(new($"Tag semântica <{tag}> ausente", 1, A11YErrorType.SemanticStructure, A11YErrorSeverity.Error));
            }
        }
    }

    private void IdentifyInputPurpose()
    {
        var inputs = _document.DocumentNode.SelectNodes("//input");

        if (inputs != null)
        {
            foreach (var input in inputs)
            {
                _totalAnalyzed++;
                
                var type = input.GetAttributeValue("type", null);
                var autocomplete = input.GetAttributeValue("autocomplete", null);

                if (string.IsNullOrEmpty(type))
                {
                    errors.Add(new($"Input com 'type' vazio", input.Line, A11YErrorType.InputProperties, A11YErrorSeverity.Error));
                }

                if (string.IsNullOrEmpty(autocomplete))
                {
                    errors.Add(new($"Input com 'autocomplete' vazio", input.Line, A11YErrorType.InputProperties, A11YErrorSeverity.Warning));

                }
            }
        }
    }
    

}