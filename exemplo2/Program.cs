using System;
using System.Collections.Generic;

class ComplexObject
{
    public int Id { get; }
    public double[] Data { get; }

    public ComplexObject(int id)
    {
        Id = id;
        Data = new double[10000]; // Aumentando o tamanho do array
    }
}

// Código executável
List<ComplexObject> objects = new List<ComplexObject>();

for (int i = 0; i < 100000; i++) // Aumentando o número de alocações
{
    ComplexObject obj = new ComplexObject(i);
    objects.Add(obj);
}

Console.WriteLine("Processamento de objetos complexos concluído.");


