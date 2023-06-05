using System.Runtime.Serialization.Formatters.Binary;
#pragma warning disable SYSLIB0011
namespace FinalTask
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к Students.dat:");
            string path = Console.ReadLine();
            StudentSorter sorter = new StudentSorter();
            sorter.Sort(path);
        }

        class StudentSorter
        {
            public void Sort(string path)
            {
                if (CheckFile(path))
                {
                    Student[] students = GetStudentsFromFile(path);
                    List<IGrouping<string, Student>> grouppedStudents = students.GroupBy(p => p.Group).ToList();
                    string directoryPath = CreateDirectory();
                    CreateFilesForStudents(grouppedStudents, directoryPath);
                    Console.Write("Job's done!");
                }
            }

            private void CreateFilesForStudents(List<IGrouping<string, Student>> students, string path)
            {
                foreach (var group in students)
                {
                    string groupName = group.Key;
                    string filePath = String.Concat(path, "\\", groupName, ".txt");
                    foreach (Student student in group)
                    {
                        WriteStudentToFile(filePath, student);
                    }
                }
            }

            private void WriteStudentToFile(string path, Student student)
            {
                FileInfo file = new FileInfo(path);
                using (StreamWriter writer = file.AppendText()) 
                {
                    writer.WriteLine($"{student.Name}, {student.DateOfBirth.ToString()}");
                }
            }

            private string CreateDirectory()
            {
                string path = String.Concat(GetDesktopPath(), "\\Students");
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                }
                return path;
            }

            private Student[] GetStudentsFromFile(string path)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    Student[] students = (Student[])bf.Deserialize(fs);
                    return students;
                }
            }

            private string GetDesktopPath()
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            private bool CheckFile(string path)
            {
                if (File.Exists(path))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Указанный файл не существует");
                    return false;
                }
            }
        }
        
    }
    [Serializable]
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}