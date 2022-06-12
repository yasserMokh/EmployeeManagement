using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Repositories.Abstractions;
using System.Collections.Generic;

namespace EmployeeManagement.Web.Repositories
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public SqlEmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Employee Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var emp = _context.Employees.Attach(employeeChanges);
            emp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employeeChanges;
        }
    }
}
