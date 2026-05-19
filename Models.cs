using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement;

public enum StudentStatus
{
    Active,
    AcademicLeave,
    Expelled,
    Graduated
}

public class GradeJournal
{
    public Dictionary<string, double> Grades { get; set; } = new();

    public double CalculateAverage()
    {
        if (Grades.Count == 0) return 0;
        return Math.Round(Grades.Values.Average(), 2);
    }
}