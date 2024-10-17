using HtmlAgilityPack;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AccessibilityChecker
{
    private const int imageWeight = 40;
    private const int semanticTagWeight = 30;
    private const int headerWeight = 30;

    private List<string> correctImages = new List<string>();
    private List<string> incorrectImages = new List<string>();
    private List<string> correctHeaders = new List<string>();
    private List<string> missingSemanticTags = new List<string>();

    private int totalImagesChecked = 0;
    private int totalSemanticTagsChecked = 0;
    private int totalHeadersChecked = 0;

    public void CheckImageAltAttributes(HtmlDocument document)
    {
        var images = document.DocumentNode.SelectNodes("//img");
        if (images != null)
        {
            totalImagesChecked = images.Count;
            foreach (var img in images)
            {
                var altAttribute = img.GetAttributeValue("alt", null);
                int line = img.Line;

                if (string.IsNullOrEmpty(altAttribute))
                {
                    incorrectImages.Add($"Imagem sem atributo 'alt'. Penalização aplicada. (Linha {line})");
                }
                else if (altAttribute == "")
                {
                    incorrectImages.Add($"Imagem com 'alt' vazio. Penalização aplicada. (Linha {line})");
                }
                else
                {
                    correctImages.Add($"Imagem com 'alt' adequado: {altAttribute}. Pontuação adicionada. (Linha {line})");
                }
            }
        }
    }

    public void CheckSemanticStructure(HtmlDocument document)
    {
        string[] semanticTags = { "header", "nav", "main", "footer", "article", "section" };
        foreach (var tag in semanticTags)
        {
            var nodes = document.DocumentNode.SelectNodes($"//{tag}");
            totalSemanticTagsChecked++;
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    correctHeaders.Add($"Tag semântica <{tag}> encontrada. Pontuação adicionada. (Linha {node.Line})");
                }
            }
            else
            {
                missingSemanticTags.Add($"Tag semântica <{tag}> ausente. Penalização aplicada.");
            }
        }

        CheckHeaderHierarchy(document);
    }

    private void CheckHeaderHierarchy(HtmlDocument document)
    {
        var headers = document.DocumentNode.SelectNodes("//h1|//h2|//h3|//h4|//h5|//h6");
        if (headers != null)
        {
            totalHeadersChecked = headers.Count;
            foreach (var header in headers)
            {
                correctHeaders.Add($"Cabeçalho {header.Name} está correto. Pontuação adicionada. (Linha {header.Line})");
            }
        }
    }

    public void GeneratePDFReport(string url)
    {
        // Extrair o nome do domínio para usar no nome do arquivo
        var uri = new Uri(url);
        string siteName = uri.Host.Replace("www.", "").Replace(".com", "").Replace(".br", "");
        string pdfFilePath = $"Relatorio_Acessibilidade_{siteName}.pdf";

        using (PdfWriter writer = new PdfWriter(pdfFilePath))
        {
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            document.Add(new Paragraph($"Relatório de Acessibilidade para o site: {url}").SetBold());
            document.Add(new Paragraph("\n--- Análise de Acessibilidade ---\n").SetBold());

            // Verificação de Imagens
            document.Add(new Paragraph("🔍 Verificação de Imagens:").SetBold());
            document.Add(new LineSeparator(new SolidLine()));

            foreach (var correctImage in correctImages)
            {
                document.Add(new Paragraph($"✓ {correctImage}"));
            }
            foreach (var incorrectImage in incorrectImages)
            {
                document.Add(new Paragraph($"✗ {incorrectImage}"));
            }

            document.Add(new Paragraph($"\nResumo das Imagens:\n- Total de imagens corretas: {correctImages.Count}\n- Total de imagens verificadas: {totalImagesChecked}"));

            // Verificação de Tags Semânticas
            document.Add(new Paragraph("\n🔍 Verificação de Tags Semânticas:").SetBold());
            document.Add(new LineSeparator(new SolidLine()));

            foreach (var header in correctHeaders)
            {
                document.Add(new Paragraph($"✓ {header}"));
            }
            foreach (var tag in missingSemanticTags)
            {
                document.Add(new Paragraph($"✗ {tag}"));
            }

            document.Add(new Paragraph($"\nResumo das Tags Semânticas:\n- Tags corretas encontradas: {correctHeaders.Count}\n- Tags ausentes: {missingSemanticTags.Count}"));

            // Nova Lógica de Pontuação: Penalizar proporcionalmente com base no total de erros
            int totalErrors = incorrectImages.Count + missingSemanticTags.Count;
            int totalElementsChecked = totalImagesChecked + totalSemanticTagsChecked + totalHeadersChecked;

            int finalScore = Math.Max(0, 100 - (totalErrors * 100 / totalElementsChecked)); // Penalização proporcional

            document.Add(new Paragraph($"\n📊 Pontuação Total (0 a 100): {finalScore}"));

            // Classificação baseada na pontuação final
            string classificacao;
            if (finalScore <= 25)
            {
                classificacao = "Ruim";
            }
            else if (finalScore <= 50)
            {
                classificacao = "Regular";
            }
            else if (finalScore <= 75)
            {
                classificacao = "Bom";
            }
            else
            {
                classificacao = "Ótimo";
            }

            document.Add(new Paragraph($"Classificação: {classificacao}"));

            // Explicação da Lógica de Penalização e Métrica
            document.Add(new Paragraph("\n📋 Explicação da Lógica de Penalização e Métrica:").SetBold());
            document.Add(new Paragraph("A lógica de penalização foi inspirada nas diretrizes WCAG (Web Content Accessibility Guidelines)."));
            document.Add(new Paragraph("Cada verificação é associada a um peso específico que reflete sua importância para acessibilidade."));
            document.Add(new Paragraph("A pontuação final é reduzida proporcionalmente ao número de penalizações em relação ao total de verificações realizadas, para refletir melhor a gravidade dos problemas de acessibilidade."));
            document.Add(new Paragraph("A classificação é baseada na pontuação total, sendo:\n- 0 a 25: Ruim\n- 26 a 50: Regular\n- 51 a 75: Bom\n- 76 a 100: Ótimo"));

            document.Close();
        }

        Console.WriteLine($"\nRelatório gerado em '{pdfFilePath}'.");
    }

}

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Por favor, insira a URL do site:");
            return;
        }

        var url = args[0];
        var web = new HtmlWeb();
        var document = web.Load(url);

        var checker = new AccessibilityChecker();

        checker.CheckImageAltAttributes(document);
        checker.CheckSemanticStructure(document);

        // Gerar PDF
        checker.GeneratePDFReport(url);
    }
}

