namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            new SendSmsWorker().RunProcess(args);
        }
    }
}
