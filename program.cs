using DBConnect.Data;
using DBConnect.Models;

namespace AcademyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string conn_str = ConnectDB();

            var studentRepo = new StudentRepository(conn_str);
            var groupRepo = new GroupRepository(conn_str);

            bool is_running = true;

            while (is_running)
            {
                Console.WriteLine("\n========== МЕНЮ ==========");
                Console.WriteLine(" 1  Просмотреть всех студентов");
                Console.WriteLine(" 2  Найти студента по ID");
                Console.WriteLine(" 3  Добавить студента");
                Console.WriteLine(" 4  Просмотреть все группы");
                Console.WriteLine(" 5  Добавить группу");
                Console.WriteLine(" 9  Выход");
                Console.Write(" Выбор: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllStudents(studentRepo);
                        break;
                    case "2":
                        FindStudentById(studentRepo);
                        break;
                    case "3":
                        AddStudentMenu(studentRepo);
                        break;
                    case "4":
                        ShowAllGroups(groupRepo);
                        break;
                    case "5":
                        AddGroupMenu(groupRepo);
                        break;
                    case "9":
                        is_running = false;
                        break;
                    default:
                        Console.WriteLine("Введите номер действия!");
                        break;
                }

                if (is_running)
                {
                    Console.WriteLine("\nНажмите любую кнопку для продолжения!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        // ===== Строка подключения =====
        static string ConnectDB()
        {
            // ⚠️ ПРОВЕРЬ имя БД: у тебя база в SSMS называется "academy"
            return "Data Source=COMP7A2\\SQLEXPRESS;" +
                   "Initial Catalog=academy;" +
                   "Integrated Security=True;" +
                   "TrustServerCertificate=True;";
        }

        // ===== 1. Все студенты =====
        static void ShowAllStudents(StudentRepository repo)
        {
            Console.WriteLine("\n=== Все студенты ===");
            var students = repo.GetAll();
            if (students.Count == 0)
            {
                Console.WriteLine("Студентов нет.");
                return;
            }
            foreach (var s in students)
                Console.WriteLine(s);
        }

        // ===== 2. Найти студента по ID =====
        static void FindStudentById(StudentRepository repo)
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID!");
                return;
            }

            var student = repo.GetById(id);
            Console.WriteLine(student != null
                ? student.ToString()
                : "Студент не найден.");
        }

        // ===== 3. Добавить студента =====
        static void AddStudentMenu(StudentRepository repo)
        {
            var s = new Student();

            Console.Write("Имя: ");
            s.FirstName = Console.ReadLine() ?? "";

            Console.Write("Фамилия: ");
            s.LastName = Console.ReadLine() ?? "";

            Console.Write("Возраст: ");
            s.Age = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("ID группы: ");
            s.GroupId = int.Parse(Console.ReadLine() ?? "0");

            int newId = repo.CreateStudent(s);
            Console.WriteLine($"✅ Студент добавлен. Новый ID = {newId}");
        }

        // ===== 4. Все группы =====
        static void ShowAllGroups(GroupRepository repo)
        {
            Console.WriteLine("\n=== Все группы ===");
            var groups = repo.GetAll();
            if (groups.Count == 0)
            {
                Console.WriteLine("Групп нет.");
                return;
            }
            foreach (var g in groups)
                Console.WriteLine(g);
        }

        // ===== 5. Добавить группу =====
        static void AddGroupMenu(GroupRepository repo)
        {
            var g = new Group();

            Console.Write("Название группы: ");
            g.GroupName = Console.ReadLine() ?? "";

            int newId = repo.CreateGroup(g);
            Console.WriteLine($"✅ Группа добавлена. Новый ID = {newId}");
        }
    }
}