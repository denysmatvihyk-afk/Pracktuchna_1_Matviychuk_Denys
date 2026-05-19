using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace StudentManagement;

public class StudentGroup
{
    public string GroupName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int Course { get; set; }

    // Приватне поле (інкапсуляція списку)
    private List<Student> _students = new();

    public int GroupSize => _students.Count;

    public double AverageGroupGrade => _students.Any() ? Math.Round(_students.Average(s => s.AverageGrade), 2) : 0;

    // Безпечний доступ до списку тільки для читання
    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public void AddStudent(Student student)
    {
        if (_students.Any(s => s.RecordBookNumber == student.RecordBookNumber))
            throw new InvalidOperationException("Студент з такою заліковою книжкою вже існує.");

        _students.Add(student);
    }

    public bool RemoveStudent(string recordBookNumber)
    {
        var student = _students.FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);
        if (student != null)
        {
            _students.Remove(student);
            return true;
        }
        return false;
    }

    // Перевантажені методи пошуку (як вимагає завдання)
    public Student? FindStudent(string recordBookNumber) =>
        _students.FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);

    public IEnumerable<Student> FindStudentByName(string namePart) =>
        _students.Where(s => s.FullName.Contains(namePart, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Student> GetExcellentStudents() => _students.Where(s => s.IsExcellent());

    public IEnumerable<Student> GetStudentsByStatus(StudentStatus status) =>
        _students.Where(s => s.Status == status);

    // Збереження та завантаження JSON
    public void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_students, options);
        File.WriteAllText(filePath, json);
    }

    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return;

        try
        {
            string json = File.ReadAllText(filePath);
            var loaded = JsonSerializer.Deserialize<List<Student>>(json);
            if (loaded != null) _students = loaded;
        }
        catch { }
    }
}