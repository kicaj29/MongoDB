namespace ApiUsageExamples.ConsoleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            // Use this project only if the are some issues with logging mongo driver in other projects
            StorageService storageService = new StorageService();
            storageService.FindOneAndUpdateWithProjection();

            Console.ReadKey();
        }
    }
}
