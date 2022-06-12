using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.ViewModels.Abstractions;
using System.Collections.Generic;

namespace EmployeeManagement.Web.ViewModels
{
    public class EmployeeListViewModel:PageBaseViewModel
    {        
        public IEnumerable<Employee> EmployeeList { get; set; }
    }
}
