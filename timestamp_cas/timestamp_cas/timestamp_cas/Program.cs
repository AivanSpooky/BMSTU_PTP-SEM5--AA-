namespace timestamp_cas
{
    internal class Program
    {
        static void Main()
        {
            DateTimeOffset currentTime = DateTimeOffset.Now;
            string formattedTime = currentTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            Console.WriteLine($"\"{formattedTime}\"");
        }
    }
}
