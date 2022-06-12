using EmployeeManagement.Web.Models;
using System.Collections.Generic;

namespace EmployeeManagement.Web.Repositories.Abstractions
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployees();

        Employee Add(Employee employee);

        Employee Update(Employee employeeChanges);

        Employee Delete(int id);
    }
}
