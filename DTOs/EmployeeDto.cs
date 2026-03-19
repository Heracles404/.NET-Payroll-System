namespace _NET_Payroll_System.DTOs;

public class EmployeeCreateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal DailyRate { get; set; }
    public string WorkingDays { get; set; }
}

public class EmployeeDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? DailyRate { get; set; }
    public string? WorkingDays { get; set; }
}