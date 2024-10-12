using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace MemoryLeakDynamicAnalyzer
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Iniciando análise dinâmica de alocação de memória...");

            // Verifica o uso de memória antes da execução do código
            long initialMemory = GC.GetTotalMemory(true);
            Console.WriteLine($"Uso de memória inicial: {initialMemory / 1024} KB");

            // Executa o código exemplo
            await ExecuteMemoryLeakExampleAsync("../exemplo1/Program.cs");

            // Verifica o uso de memória após a execução do código
            long finalMemory = GC.GetTotalMemory(true);
            Console.WriteLine($"Uso de memória após a execução: {finalMemory / 1024} KB");

            // Força a coleta de lixo
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Verifica o uso de memória após a coleta de lixo
            long memoryAfterGC = GC.GetTotalMemory(true);
            Console.WriteLine($"Uso de memória após coleta de lixo: {memoryAfterGC / 1024} KB");

            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }

        static async System.Threading.Tasks.Task ExecuteMemoryLeakExampleAsync(string filePath)
        {
            // Converte o caminho relativo em absoluto
            string fullPath = Path.GetFullPath(filePath);

            // Verifica se o arquivo existe
            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"Arquivo não encontrado: {fullPath}");
                Console.WriteLine("Por favor, verifique se o caminho está correto e se o arquivo existe.");
                return;
            }

            // Lê o conteúdo do arquivo
            string code = await File.ReadAllTextAsync(fullPath);

            // Verifica se o código lido não está vazio
            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("O arquivo foi lido, mas está vazio.");
                return;
            }

//            Console.WriteLine($"Código lido:\n{code}");

            // Executa o código lido usando CSharpScript
            try
            {
                await CSharpScript.RunAsync(code, ScriptOptions.Default);
            }
            catch (CompilationErrorException e)
            {
                Console.WriteLine("Erros de compilação:");
                foreach (var error in e.Diagnostics)
                {
                    Console.WriteLine(error.ToString());
                }
            }
        }
    }
}
