using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Praktychna1
{
    public enum StudentStatus { Active, AcademicLeave, Expelled, Graduated }
    // Додано ICloneable для виконання додаткових вимог 
    public class Student : ICloneable
    {
        private string _fullName;
        private string _recordBookNumber;
        // Додано одновимірний масив оцінок за лабораторні (10 елементів) 
        public byte[] LabGrades { get; private set; } = new byte[10];
        public string FullName
        {
            get => _fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
                    throw new ArgumentException("ПІБ має містити мінімум 5 символів");
                _fullName = value;
            }
        }
        public DateTime DateOfBirth { get; init; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public required string RecordBookNumber
        {
            get => _recordBookNumber;
            set
            {
                if (value?.Length != 8 || !long.TryParse(value, out _))
                    throw new ArgumentException("Номер заліковки має містити рівно 8 цифр");
                _recordBookNumber = value;
            }
        }


        public GradeJournal Journal { get; } = new GradeJournal();
        public double AverageGrade => Journal.CalculateAverage();
        public StudentStatus Status { get; set; }
        public DateTime EnrollmentDate { get; init; } = DateTime.Now;
        public string PersonalEmail { get; set; }
        public string Notes { get; set; }
        // Метод додавання оцінки за лабораторну
        public void AddLabGrade(int labNumber, byte grade)
        {
            // Перевірка індексу для обробки винятків
            if (labNumber < 0 || labNumber >= LabGrades.Length)
                throw new IndexOutOfRangeException("Номер лабораторної має бути від 0 до 9");
            LabGrades[labNumber] = grade;
        }
        // Метод отримання середнього балу за лабораторні
        public double GetAverageLabGrade()
        {
            if (LabGrades.Length == 0) return 0;
            int sum = 0;
            for (int i = 0; i < LabGrades.Length; i++)
            {
                sum += LabGrades[i];
            }
            return (double)sum / LabGrades.Length;
        }
        public bool IsExcellent() => AverageGrade >= 90;
        public bool IsFailing() => AverageGrade < 60;
        public int GetYearsToGraduation() => 4 - (DateTime.Now.Year - EnrollmentDate.Year);
        // Використання StringBuilder замість конкатенації рядків (+) 
        public void ShowDetailedInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[Студент] {0} | Квиток: {1} | Вік: {2} | Сер. бал: {3:F2}\n",
                FullName, RecordBookNumber, Age, AverageGrade);
            sb.Append("   Оцінки (Journal): ").AppendLine(Journal.GetGradesSummary());
            // Вивід масиву лабораторних
            sb.Append("   Лабораторні роботи: ").AppendLine(string.Join(", ",

LabGrades));
            sb.AppendFormat("   Сер. бал лаб: {0:F2}\n", GetAverageLabGrade());
            Console.WriteLine(sb.ToString());
        }
        // Реалізація клонування 
        public object Clone()
        {
            var clone = (Student)this.MemberwiseClone();
            clone.LabGrades = (byte[])this.LabGrades.Clone(); // Глибоке копіювання масиву
            return clone;
        }
    }
}
