namespace ApiUsageExamples.ConsoleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            StorageService storageService = new StorageService();
            storageService.FindOneAndUpdateWithProjection();

            Console.ReadKey();
        }
    }
}
