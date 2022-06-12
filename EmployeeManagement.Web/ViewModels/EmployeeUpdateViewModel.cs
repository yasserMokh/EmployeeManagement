namespace EmployeeManagement.Web.ViewModels
{
    public class EmployeeUpdateViewModel:EmployeeCreateViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
