using System.ComponentModel.DataAnnotations.Schema;

namespace Day1.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Age { get; set; }
        [ForeignKey("Department")]
        public int? Deptid { get; set; }
        public virtual Department Department { get; set; }
    }
}
