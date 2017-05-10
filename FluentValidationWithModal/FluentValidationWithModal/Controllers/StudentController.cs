using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidationWithModal.Models;

namespace FluentValidationWithModal.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ModalFormAction()
        {
            return PartialView("_ModalFormContent");
        }
        [HttpGet]
        public PartialViewResult AddEditStudent()
        {
            StudentViewModel model = new StudentViewModel();
            return PartialView("_AddEditStudent", model);
        }
        [HttpPost]
        public ActionResult AddEditStudent(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return RedirectToAction("index");
        }
    }
}