using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day1.Models;
using Day1.DTO;
using AutoMapper;
using System.Security.Policy;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ITIDbContext _context;
        private readonly IMapper mapper;

        public EmployeesController(ITIDbContext context,IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public  IActionResult GetEmployees()
        {
            List<Employee> employees = _context.Employees.Include(d=>d.Department).ToList();

            var employeeDto = mapper.Map<IEnumerable<EmployeeDTO>>(employees);
            //List<EmployeeDTO> employeeDto = new();
            //foreach (var employee in employees)
            //{
            //    EmployeeDTO employeeDTO = new()
            //    {
            //        Id = employee.Id,
            //        Name = employee.Name,
            //        Age = employee.Age,
            //        DepartmentID = employee.Deptid
            //    };
            //    employeeDto.Add(employeeDTO);
            //}
            return Ok(employeeDto);
        }

        // GET: api/Employees/5
        [HttpGet("{id}",Name = "GetEmployeeId")]
        public  IActionResult GetEmployee(int id)
        {
            var employee =  _context.Employees.Include(d => d.Department).FirstOrDefault(e=>e.Id==id);

            var employeeDTO = mapper.Map<EmployeeDTO>(employee);     
            //EmployeeDTO employeeDTO = new();

            //employeeDTO.Id = employee.Id;
            //employeeDTO.Name = employee.Name;
            //employeeDTO.Age = employee.Age;
            //employeeDTO.DepartmentID = employee.Deptid;

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employeeDTO);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            var emplyeeMap =  mapper.Map<Employee>(employee);
            _context.Entry(emplyeeMap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostEmployee(CreateEmployeeWithOutDepartmentDto employeeDTO)
        {
            var employee = mapper.Map<Employee>(employeeDTO);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            string url = Url.Link("GetEmployeeId", new { id = employee.Id });
            return Created(url, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
