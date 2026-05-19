using System;
using System.Linq;

namespace StudentManagement;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        StudentGroup group = new StudentGroup
        {
            GroupName = "КН-21",
            Specialty = "Комп'ютерні науки",
            Course = 2
        };

        string dbPath = "students.json";
        group.LoadFromFile(dbPath);

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Система управління групою {group.GroupName} ===");
            Console.WriteLine("1. Додати студента");
            Console.WriteLine("2. Видалити студента");
            Console.WriteLine("3. Вивести всіх студентів (з пагінацією)");
            Console.WriteLine("4. Пошук студента (за ПІБ або заліковою)");
            Console.WriteLine("5. Редагувати дані студента (додати оцінку)");
            Console.WriteLine("6. Вивести відмінників / боржників (< 60 балів)");
            Console.WriteLine("7. Вивести статистику групи");
            Console.WriteLine("8. Зберегти дані у файл");
            Console.WriteLine("9. Вийти");
            Console.Write("\nОберіть дію: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть ПІБ (мін. 5 символів): ");
                        string name = Console.ReadLine()!;
                        Console.Write("Номер залікової (8 цифр): ");
                        string record = Console.ReadLine()!;
                        Console.Write("Email: ");
                        string email = Console.ReadLine()!;

                        var newStudent = new Student
                        {
                            FullName = name,
                            RecordBookNumber = record,
                            PersonalEmail = email,
                            DateOfBirth = new DateTime(2005, 05, 15) // Дефолтна дата для прикладу
                        };
                        group.AddStudent(newStudent);
                        Console.WriteLine("Студента успішно додано!");
                        break;

                    case "2":
                        Console.Write("Введіть номер залікової для видалення: ");
                        string recToRemove = Console.ReadLine()!;
                        if (group.RemoveStudent(recToRemove))
                            Console.WriteLine("Студента видалено.");
                        else
                            Console.WriteLine("Студента з такою заліковою не знайдено.");
                        break;

                    case "3":
                        Console.WriteLine("\n--- Список студентів ---");
                        if (group.GroupSize == 0)
                        {
                            Console.WriteLine("Група порожня.");
                            break;
                        }
                        // Проста пагінація
                        int pageSize = 10;
                        var page = group.Students.Take(pageSize);
                        foreach (var s in page)
                        {
                            Console.WriteLine($"- {s.FullName} | Залікова: {s.RecordBookNumber} | Сер. бал: {s.AverageGrade}");
                        }
                        break;

                    case "4":
                        Console.Write("Введіть залікову або частину ПІБ для пошуку: ");
                        string searchStr = Console.ReadLine()!;
                        var byRecord = group.FindStudent(searchStr);
                        if (byRecord != null)
                        {
                            byRecord.ShowDetailedInfo();
                        }
                        else
                        {
                            var byName = group.FindStudentByName(searchStr);
                            if (byName.Any())
                            {
                                foreach (var s in byName) s.ShowDetailedInfo();
                            }
                            else Console.WriteLine("Нікого не знайдено.");
                        }
                        break;

                    case "5":
                        Console.Write("Введіть номер залікової для редагування: ");
                        string recToEdit = Console.ReadLine()!;
                        var studentToEdit = group.FindStudent(recToEdit);
                        if (studentToEdit != null)
                        {
                            Console.Write("Введіть назву предмету: ");
                            string subject = Console.ReadLine()!;
                            Console.Write("Введіть оцінку (0-100): ");
                            if (double.TryParse(Console.ReadLine(), out double grade))
                            {
                                studentToEdit.AddGrade(subject, grade);
                                Console.WriteLine("Оцінку додано, середній бал перераховано!");
                            }
                        }
                        else Console.WriteLine("Студента не знайдено.");
                        break;

                    case "6":
                        Console.WriteLine("\n--- ВІДМІННИКИ (>=90) ---");
                        foreach (var s in group.GetExcellentStudents()) Console.WriteLine($"- {s.FullName}");

                        Console.WriteLine("\n--- МАЮТЬ БОРГИ (<60) ---");
                        foreach (var s in group.Students.Where(st => st.IsFailing())) Console.WriteLine($"- {s.FullName}");
                        break;

                    case "7":
                        Console.WriteLine($"\nСтатистика групи {group.GroupName}:");
                        Console.WriteLine($"Кількість студентів: {group.GroupSize}");
                        Console.WriteLine($"Середній бал групи: {group.AverageGroupGrade}");
                        double excPercent = group.GroupSize == 0 ? 0 : (double)group.GetExcellentStudents().Count() / group.GroupSize * 100;
                        Console.WriteLine($"Відсоток відмінників: {excPercent:F1}%");
                        break;

                    case "8":
                        group.SaveToFile(dbPath);
                        Console.WriteLine("Дані успішно збережено у файл students.json!");
                        break;

                    case "9":
                        group.SaveToFile(dbPath);
                        return;

                    default:
                        Console.WriteLine("Неправильний вибір.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка валідації: {ex.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }
    }
}