using EmployeeManagement.Context;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _employeeContext;

        public EmployeesController(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public IActionResult AllEmployees()
        {
            var model = new EmployeesViewModel();

            model.AllEmployees = new List<Employee>();

            model.AllEmployees = _employeeContext.Employees.ToList();

            return View(model);
        }

        public IActionResult SaveEmployee(Employee employee)
        {
            if(employee == null)
            {
                return View("Error");
            }

            var employeeToUpdate = _employeeContext.Employees.FirstOrDefault(x => x.EmployeeID == employee.EmployeeID);

            if(employeeToUpdate == null)
            {
                _employeeContext.Employees.Add(employee);
            }
            else
            {
                employeeToUpdate.FirstName = employee.FirstName;
                employeeToUpdate.LastName = employee.LastName;
                employeeToUpdate.BirthDate = employee.BirthDate;
            }

            _employeeContext.SaveChanges();

            return RedirectToAction("AllEmployees");
        }

        public IActionResult NewEmployee()
        {
            var model = new Employee();
            return View("ManageEmployee", model);
        }

        public IActionResult UpdateEmployee(int id)
        {
            var employee = _employeeContext.Employees.FirstOrDefault(x => x.EmployeeID ==id);

            if (employee == null)
            {
                return BadRequest("Bad Request. There is no employee with this id");
            }

            return View("ManageEmployee", employee);
        }

        public IActionResult RemoveEmployee(int id)
        {
            var employee = _employeeContext.Employees.FirstOrDefault(x => x.EmployeeID == id);

            if (employee == null)
            {
                return BadRequest("Bad Request. There is no employee with this id");
            }

            _employeeContext.Employees.Remove(employee);

            _employeeContext.SaveChanges();

            return RedirectToAction("AllEmployees");
        }
    }
}
