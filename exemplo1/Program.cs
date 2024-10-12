using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<MyObject> objects = new List<MyObject>();

        for (int i = 0; i < 10000; i++)
        {
            MyObject obj = new MyObject(i);
            objects.Add(obj);
            // Simula o uso de memória sem liberar os objetos
        }
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}
class MyObject
{
    public int Id { get; }
    public string Data { get; }
    public MyObject(int id)
    {
        Id = id;
        Data = new string('A', 10000); // Alocação de memória
    }
}
