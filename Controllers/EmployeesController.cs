using _NET_Payroll_System.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET_Payroll_System.Data;
using NET_Payroll_System.Models;

namespace _NET_Payroll_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    // Allowed working days
    private readonly string[] _allowedWorkingDays = { "MWF", "TTHS" };

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateDto dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc),
            DailyRate = dto.DailyRate,
            WorkingDays = dto.WorkingDays
        };

        // Generate Employee Number
        var prefix = employee.LastName.Length >= 3
            ? employee.LastName.Substring(0, 3).ToUpper()
            : employee.LastName.ToUpper().PadRight(3, '*');

        var random = new Random().Next(0, 99999).ToString("D5");
        var dobFormatted = employee.DateOfBirth.ToString("ddMMMyyyy").ToUpper();

        employee.EmployeeNumber = $"{prefix}-{random}-{dobFormatted}";

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Ok(employee);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _context.Employees.ToListAsync();
        return Ok(employees);
    }
    
    [HttpGet("{employeeNumber}")]
    public async Task<IActionResult> GetByEmployeeNumber(string employeeNumber)
    {
        // Find employee by employeeNumber
        var employee = await _context.Employees.FindAsync(employeeNumber);

        // If not found, return 404
        if (employee == null)
            return NotFound();

        // Return the employee data
        return Ok(employee);
    }
    
    [HttpPatch("{employeeNumber}")]
    public async Task<IActionResult> Patch(string employeeNumber, EmployeeDto dto)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);

        if (employee == null)
            return NotFound();

        // Update allowed fields only

        if (!string.IsNullOrEmpty(dto.FirstName))
            employee.FirstName = dto.FirstName;

        if (!string.IsNullOrEmpty(dto.LastName))
            employee.LastName = dto.LastName;

        if (!string.IsNullOrEmpty(dto.MiddleName))
            employee.MiddleName = dto.MiddleName;

        if (dto.DailyRate.HasValue)
            employee.DailyRate = dto.DailyRate.Value;

        if (!string.IsNullOrEmpty(dto.WorkingDays))
        {
            // Validate WorkingDays
            if (!_allowedWorkingDays.Contains(dto.WorkingDays))
            {
                return BadRequest("WorkingDays must be either 'MWF' or 'TTHS'.");
            }

            employee.WorkingDays = dto.WorkingDays;
        }

        // ❗ Do NOT update:
        // employee.EmployeeNumber
        // employee.DateOfBirth
        // employee.Number

        await _context.SaveChangesAsync();

        return Ok(employee);
    }
    
    [HttpDelete("{employeeNumber}")]
    public async Task<IActionResult> Delete(string employeeNumber)
    {
        // Find employee by employeeNumber
        var employee = await _context.Employees.FindAsync(employeeNumber);

        // If not found, return 404
        if (employee == null)
            return NotFound();

        // Remove employee from database
        _context.Employees.Remove(employee);

        // Save changes
        await _context.SaveChangesAsync();

        // Return success response
        return Ok($"Employee with ID Number {employeeNumber} deleted successfully.");
    }
}