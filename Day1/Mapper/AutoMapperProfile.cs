using AutoMapper;
using Day1.DTO;
using Day1.Models;
using System.Linq;

namespace Day1.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region Employee Map
            CreateMap<Employee, EmployeeDTO>()
                   .ForMember(e => e.DepartmentID, src => src.MapFrom(e => e.Deptid))
                   .ReverseMap();

            CreateMap<Employee, CreateEmployeeWithOutDepartmentDto>()
                .ReverseMap();
            #endregion

            #region Department Map
            CreateMap<Department, DepartmentDTO>()
                .ForMember(d => d.Employees, src => src.MapFrom(d => d.Employees.Select(e => e.Name)))
                .ReverseMap();

            CreateMap<Department, DepartmentUpdateDTO>()
                .ReverseMap();
            #endregion
        }
    }
}
