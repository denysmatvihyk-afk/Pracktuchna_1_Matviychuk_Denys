using System;
using System.Text.RegularExpressions;

namespace StudentManagement;

public class Student
{
    private string _fullName = string.Empty;
    private string _personalEmail = string.Empty;
    private string _recordBookNumber = string.Empty;

    public required string RecordBookNumber
    {
        get => _recordBookNumber;
        set
        {
            if (value.Length != 8 || !long.TryParse(value, out _))
                throw new ArgumentException("Номер залікової книжки має складатись рівно з 8 цифр.");
            _recordBookNumber = value;
        }
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
                throw new ArgumentException("ПІБ не може бути порожнім і має містити мінімум 5 символів.");
            _fullName = value;
        }
    }

    public DateTime DateOfBirth { get; init; }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    public double AverageGrade { get; private set; }

    public StudentStatus Status { get; set; } = StudentStatus.Active;
    public DateTime EnrollmentDate { get; init; } = DateTime.Now;
    public string Notes { get; set; } = string.Empty;

    public string PersonalEmail
    {
        get => _personalEmail;
        set
        {
            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Некоректний формат email.");
            _personalEmail = value;
        }
    }

    public GradeJournal Journal { get; } = new();

    public void UpdateAverageGrade()
    {
        AverageGrade = Journal.CalculateAverage();
    }

    public void AddGrade(string subject, double grade)
    {
        if (grade < 0 || grade > 100) throw new ArgumentException("Оцінка має бути від 0 до 100.");
        Journal.Grades[subject] = grade;
        UpdateAverageGrade();
    }

    public bool IsExcellent() => AverageGrade >= 90;

    public bool IsFailing() => AverageGrade < 60;

    public int GetYearsToGraduation(int totalProgramYears = 4)
    {
        int yearsStudied = DateTime.Now.Year - EnrollmentDate.Year;
        int yearsLeft = totalProgramYears - yearsStudied;
        return yearsLeft > 0 ? yearsLeft : 0;
    }

    public void ShowDetailedInfo()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine($"Студент: {FullName} (Залікова: {RecordBookNumber})");
        Console.WriteLine($"Вік: {Age}");
        Console.WriteLine($"Email: {PersonalEmail}");
        Console.WriteLine($"Статус: {Status}");
        Console.WriteLine($"Середній бал: {AverageGrade}");
        Console.WriteLine("=====================================");
    }
}