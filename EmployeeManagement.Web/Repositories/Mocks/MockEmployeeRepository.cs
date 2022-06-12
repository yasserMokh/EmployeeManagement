using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Models.Enums;
using EmployeeManagement.Web.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Web.Repositories.Mocks
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){Id=1, Name="Employee1", Email="Employee1@corp.tst", Department=Dept.HR},
                new Employee(){Id=2, Name="Employee2", Email="Employee2@corp.tst", Department=Dept.IT},
                new Employee(){Id=3, Name="Employee3", Email="Employee3@corp.tst", Department=Dept.IT},
                new Employee(){Id=4, Name="Employee4", Email="Employee4@corp.tst", Department=Dept.Payroll}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e=>e.Id==id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var emp = GetEmployee(employeeChanges.Id);
            if (emp != null)
            {
                emp.Name = employeeChanges.Name;
                emp.Email = employeeChanges.Email;
                emp.Department = employeeChanges.Department;
            }
            return emp;
        }

        public Employee Delete(int id)
        {
            var emp = GetEmployee(id);
            if (emp != null)
            {
                _employeeList.Remove(emp);
            }
            return emp;
        }
    }
}
