// See https://aka.ms/new-console-template for more information
using ApiFindOneAndUpdateAsync;

Console.WriteLine("Hello, World!");

Worker w = new Worker();
await w.StartProcessing();

Console.WriteLine("WORK FINISHED");
Console.ReadKey();
