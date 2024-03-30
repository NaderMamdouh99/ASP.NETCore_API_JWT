using AutoMapper;
using Day1.DTO;
using Day1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ITIDbContext context;
        private readonly IMapper mapper;

        public DepartmentController(ITIDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Department> departments = await context.Departments.Include(d=>d.Employees).ToListAsync();
            var departmentDTOs = mapper.Map<ICollection<DepartmentDTO>>(departments);
            //List<DepartmentDTO> departmentDTOs = new();
            //foreach (var department in departments)
            //{
            //    DepartmentDTO departmentDTO = new DepartmentDTO()
            //    {
            //        Id = department.Id,
            //        Name = department.Name,
            //        Manager = department.Manager,
            //        Employees = department.Employees.Select(e => e.Name).ToList()
            //    };
            //    departmentDTOs.Add(departmentDTO);

            //}
            return Ok(departmentDTOs);
        }

        [HttpGet("{id:int}",Name ="GetDepartmemtId")]
        public async Task<IActionResult> Get(int id)
        {
           Department department = await context.Departments.Include(e=>e.Employees).FirstOrDefaultAsync(e=>e.Id==id);
            var departmentDTO = mapper.Map<DepartmentDTO>(department);
            //DepartmentDTO departmentDTO = new();
            //departmentDTO.Id = department.Id;
            //departmentDTO.Name= department.Name;
            //departmentDTO.Manager = department.Manager;
            //foreach (var item in department.Employees)
            //{
            //    departmentDTO.Employees.Add(item.Name);
            //}

            if (department != null)
            {
                return Ok(departmentDTO);
            }
            return BadRequest("Can Not FIND THIS Department");
        }

        [HttpPost]
        public async Task<IActionResult> Add(DepartmentUpdateDTO  departmentDTO)
        {
            if (ModelState.IsValid)
            {
                var department = mapper.Map<Department>(departmentDTO);
                await context.Departments.AddAsync(department);
                await context.SaveChangesAsync();
                string url = Url.Link("GetDepartmemtId",new {id = department.Id});
                return Created(url, department);

            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id , DepartmentUpdateDTO department)
        {
            if (ModelState.IsValid)
            {
                Department OldDepartment = await context.Departments.FindAsync(id);
                if (OldDepartment != null)
                {
                    var newDepartment =  mapper.Map<Department>(department); 
                    //OldDepartment.Name = department.Name;
                    //OldDepartment.Manager = department.Manager;
                    await context.SaveChangesAsync();
                    string url = Url.Link("GetDepartmemtId", new { id = OldDepartment.Id });
                    return Created(url, newDepartment);
                }
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Department department = context.Departments.Find(id);
            if (department != null)
            {
                context.Departments.Remove(department);
                context.SaveChanges();
                return StatusCode(201, "Department Removed Successfully");
            }
            return BadRequest("Can Not Find This Department");
        }
    }
}
