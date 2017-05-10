using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidationSample.Models;

namespace FluentValidationSample.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddEditStudent()
        {
            StudentViewModel model=new StudentViewModel();
            return PartialView("_AddEditStudent",model);
        }
        [HttpPost]
        public ActionResult AddEditStudent(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return RedirectToAction("index");
        }
        [HttpGet]
        public ActionResult AddEditStudent1()
        {
            StudentViewModel model = new StudentViewModel();
            return PartialView("_AddEditStudent1");
        }
    }
}