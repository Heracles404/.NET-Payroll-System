using Microsoft.AspNetCore.Mvc;
using NET_Payroll_System.Data;
using NET_Payroll_System.Models;
using Microsoft.EntityFrameworkCore;

namespace NET_Payroll_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
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
}