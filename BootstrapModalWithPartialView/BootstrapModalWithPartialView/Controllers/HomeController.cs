using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapModalWithPartialView.Models;

namespace BootstrapModalWithPartialView.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ModalAction1()
        {
            return PartialView("_ModalContent");
        }

        public ActionResult ModalFormAction()
        {
            return PartialView("_ModalFormContent");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostModal()
        {
            var closeModal = new CloseModal
            {
                ShouldClose = true,
                FetchData = true,
                Destination = Url.Action("List"),
                Target = "list",
                OnSuccess = "success"
            };

            return PartialView("_CloseModal", closeModal);
        }
    }
}