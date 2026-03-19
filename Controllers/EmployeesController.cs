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
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _context.Employees.ToListAsync();
        return Ok(employees);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Ok(employee);
    }
}