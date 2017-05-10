using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidationSample.Models;

namespace FluentValidationSample.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult AddEditCustomer()
        {
            CustomerViewModel model=new CustomerViewModel();
            return View("AddEditCustomer", model);
        }
        [HttpPost]
        public ActionResult AddEditCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Operations goes here
                return RedirectToAction("Success");
            }
            return View("AddEditCustomer", model);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}