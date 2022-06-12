using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Repositories.Abstractions;
using EmployeeManagement.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace EmployeeManagement.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var employeeListViewModel = new EmployeeListViewModel();
            employeeListViewModel.PageTitle = "Employee List";
            employeeListViewModel.EmployeeList = _employeeRepository.GetAllEmployees();
            return View(employeeListViewModel);
        }
        public IActionResult Details(int id)
        {
            var employeeDetailsViewModel = new EmployeeDetailsViewModel();
            employeeDetailsViewModel.PageTitle = "Employee Details";
            employeeDetailsViewModel.Employee = _employeeRepository.GetEmployee(id);
            if (employeeDetailsViewModel.Employee == null)
            {
                return NotFound();
            }
            return View(employeeDetailsViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                var newEmp = new Employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmp);

                return RedirectToAction("Details", new { id = newEmp.Id });
            }
            else
            {
                return View();
            }
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName;
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);
                using (var fileStream= new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                    
            }

            return uniqueFileName;
        }

        public IActionResult Edit(int id)
        {
            var emp = _employeeRepository.GetEmployee(id);
            if (emp == null)
            {
                return NotFound();
            }

            var employeeUpdateViewModel = new EmployeeUpdateViewModel()
            {
                Id = emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                Department = emp.Department,
                ExistingPhotoPath = emp.PhotoPath
            };

            return View(employeeUpdateViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emp = _employeeRepository.GetEmployee(model.Id);
                emp.Name = model.Name;
                emp.Email = model.Email;
                emp.Department = model.Department;
                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        var oldImgPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(oldImgPath);
                    }
                    emp.PhotoPath = ProcessUploadedFile(model);
                }

                _employeeRepository.Update(emp);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        

    }
}


