internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к папке:");
        string path = Console.ReadLine();
        double lengthBeforeClean = Counter.Count(path);
        Cleaner.CleanFolder(path);
        double lengthAfterClean = Counter.Count(path);
        Console.WriteLine($"Исходный размер папки {lengthBeforeClean} мегабайт");
        Console.WriteLine($"Освобождено {lengthBeforeClean-lengthAfterClean} мегабайт");
        Console.WriteLine($"Текущий размер папки {lengthAfterClean} мегабайт");


    }


    static class Cleaner
    {
        private static TimeSpan timeSpan;

        static Cleaner()
        {
            timeSpan = TimeSpan.FromMinutes(30);
        }

        static public void CleanFolder(string path)
        {
            DirectoryInfo di = CheckDir(path);
            if (di != null)
            {
                FileInfo[] fileInfoArr = di.GetFiles();
                foreach (FileInfo fi in fileInfoArr)
                {
                    if (CheckFileDate(fi))
                    {
                        try
                        {
                            fi.Delete();
                            Console.WriteLine($"Файл удален: {fi.FullName}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Ошибка при удалении файла: {fi.FullName}\t{e.Message}");
                        }
                    }

                }
            }
        }

        static private bool CheckFileDate(FileInfo file)
        {
            if (DateTime.Now - file.LastWriteTime > timeSpan)
            {
                return true;
            }
            return false;
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

    static class Counter
    {
        static private long sumLength;


        static public double Count(string path)
        {
            sumLength = default(long);
            DirectoryInfo dir = CheckDir(path);
            if (dir != null)
            {
                CountFileLengthRecursion(dir);
                return ConvertToMegabytes(sumLength);
            }
            return default(double);
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