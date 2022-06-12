using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.ViewModels.Abstractions;

namespace EmployeeManagement.Web.ViewModels
{
    public class EmployeeDetailsViewModel:PageBaseViewModel
    {        
        public Employee Employee { get; set; }
    }
}
