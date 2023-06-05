internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к папке для очистки:");
        string path = Console.ReadLine();
        Cleaner.CleanFolder(path);
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
}