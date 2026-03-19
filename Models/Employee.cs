namespace NET_Payroll_System.Models;

public class Employee
{
    public int Id { get; set; }

    public string EmployeeNumber { get; set; } = "";

    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string MiddleName { get; set; } = "";

    public DateTime DateOfBirth { get; set; }

    public decimal DailyRate { get; set; }

    public string WorkingDays { get; set; } = ""; // MWF or TTHS
}