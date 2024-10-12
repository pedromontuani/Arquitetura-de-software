using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MemoryLeakAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "../exemplo2/Program.cs"; // Caminho para o arquivo de código
            AnalyzeMemoryLeak(filePath);
        }

        static void AnalyzeMemoryLeak(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Arquivo não encontrado: " + filePath);
                return;
            }

            // Lê o conteúdo do arquivo
            string code = File.ReadAllText(filePath);

            // Cria a árvore de sintaxe a partir do código
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            // Procura por todas as expressões de nova alocação de objetos
            var allocations = root.DescendantNodes().OfType<ObjectCreationExpressionSyntax>();

            int memoryAllocationCount = 0;

            Console.WriteLine("Linhas com alocações de memória:");

            foreach (var allocation in allocations)
            {
                memoryAllocationCount++;
                // Obtém a linha onde a alocação ocorre
                int lineNumber = allocation.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                Console.WriteLine($"Linha {lineNumber}: {allocation}");
            }

            Console.WriteLine($"\nNúmero total de alocações de memória detectadas: {memoryAllocationCount}");
            if (memoryAllocationCount > 0)
            {
                Console.WriteLine("Possível vazamento de memória identificado. Considere liberar os objetos após o uso.");
            }
            else
            {
                Console.WriteLine("Nenhum vazamento de memória identificado.");
            }
        }
    }
}
