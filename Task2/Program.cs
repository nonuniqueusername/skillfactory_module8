internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к папке для подсчета размера:");
        string path = Console.ReadLine();
        Counter.Count(path);
    }

    static class Counter
    {
        static private long sumLength;

        static public void Count(string path)
        {
            sumLength = default(long);
            DirectoryInfo dir = CheckDir(path);
            if (dir != null)
            {
                CountFileLengthRecursion(dir);
                Console.WriteLine($"{ConvertToMegabytes(sumLength)} мегабайт"); 
            }
        }

        static private double ConvertToMegabytes(long bytes)
        {
            return bytes / 1024f / 1024f;
        }

        static private void CountFileLengthRecursion(DirectoryInfo path)
        {
            FileInfo[] files = path.GetFiles();
            foreach (FileInfo file in files)
            {
                sumLength += file.Length;
            }
            DirectoryInfo[] nestedDirectories = path.GetDirectories();
            foreach (DirectoryInfo nestedDirectory in nestedDirectories)
            {
                CountFileLengthRecursion(nestedDirectory);
            }
        }

        static private DirectoryInfo CheckDir(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
            {
                return di;
            }
            else
            {
                Console.WriteLine($"Папка {path} не существует");
                return null;
            }
        }
    }
}
