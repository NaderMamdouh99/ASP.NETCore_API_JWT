using System.Collections.Generic;

namespace Day1.DTO
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public List<string> Employees { get; set; } = new();
    }
}
