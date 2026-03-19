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
    public async Task<IActionResult> Create(Employee employee)
    {
        var missingFields = new List<string>();

        // Collect missing/invalid fields

        if (string.IsNullOrEmpty(employee.FirstName))
            missingFields.Add("FirstName");

        if (string.IsNullOrEmpty(employee.LastName))
            missingFields.Add("LastName");

        if (employee.DateOfBirth == default)
            missingFields.Add("DateOfBirth");

        if (employee.DailyRate <= 0)
            missingFields.Add("DailyRate");

        if (string.IsNullOrEmpty(employee.WorkingDays))
            missingFields.Add("WorkingDays");

        // If any required fields are missing, return all at once
        if (missingFields.Count > 0)
        {
            return BadRequest($"Missing or invalid parameters: {string.Join(", ", missingFields)}");
        }

        // Validate WorkingDays
        if (!_allowedWorkingDays.Contains(employee.WorkingDays))
        {
            return BadRequest("WorkingDays must be either 'MWF' or 'TTHS'.");
        }

        // Ensure UTC date
        employee.DateOfBirth = DateTime.SpecifyKind(employee.DateOfBirth, DateTimeKind.Utc);

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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Find employee by primary key (ID)
        var employee = await _context.Employees.FindAsync(id);

        // If not found, return 404
        if (employee == null)
            return NotFound();

        // Return the employee data
        return Ok(employee);
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, EmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);

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
        // employee.ID

        await _context.SaveChangesAsync();

        return Ok(employee);
    }
}