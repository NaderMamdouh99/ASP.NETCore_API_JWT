using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Day1.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Manager { get; set; }

        public List<Employee> Employees { get; set; } = new();
    }
}
