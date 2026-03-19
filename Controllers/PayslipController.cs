using Microsoft.AspNetCore.Mvc;
using NET_Payroll_System.Data;

namespace NET_Payroll_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayslipController : ControllerBase
{
    private readonly AppDbContext _context;

    public PayslipController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ComputePay(int id, DateTime startDate, DateTime endDate)
    {
        // Finds the employee details based on ID
        var employee = await _context.Employees.FindAsync(id);

        // If employee does not exist, return 404
        if (employee == null)
            return NotFound();

        int workingDaysCount = 0;

        // Loop through each date in the given range
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            // Check working schedule: MWF
            if (employee.WorkingDays == "MWF")
            {
                if (date.DayOfWeek == DayOfWeek.Monday ||
                    date.DayOfWeek == DayOfWeek.Wednesday ||
                    date.DayOfWeek == DayOfWeek.Friday)
                {
                    workingDaysCount++;
                }
            }
            // Check working schedule: TTHS
            else if (employee.WorkingDays == "TTHS")
            {
                if (date.DayOfWeek == DayOfWeek.Tuesday ||
                    date.DayOfWeek == DayOfWeek.Thursday ||
                    date.DayOfWeek == DayOfWeek.Saturday)
                {
                    workingDaysCount++;
                }
            }
        }

        // Compute base pay: working days × daily rate × 2 (twice daily rate)
        decimal basePay = workingDaysCount * employee.DailyRate * 2;

        bool hasBirthday = false;

        // Check if employee birthday falls within the date range
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.Month == employee.DateOfBirth.Month &&
                date.Day == employee.DateOfBirth.Day)
            {
                hasBirthday = true;
                break;
            }
        }

        // Add birthday bonus (100% of daily rate)
        if (hasBirthday)
        {
            basePay += employee.DailyRate;
        }

        // Return computed payroll details
        return Ok(new
        {
            EmployeeNumber = employee.EmployeeNumber,
            Name = $"{employee.LastName}, {employee.FirstName}",
            TotalWorkingDays = workingDaysCount,
            TakeHomePay = basePay
        });
    }
}