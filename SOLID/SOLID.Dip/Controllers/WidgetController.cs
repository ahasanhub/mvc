using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOLID.Dip.Models;

namespace SOLID.Dip.Controllers
{
    public class WidgetController : Controller
    {
       
        // GET: Widget
        public ActionResult Index()
        {
            //Call the service layer for this
            var service =new CoordinatingServiec(new DbLogger());
            service.CordinateTransaction(new List<IWidget>
            {
                new Widget(new DbLogger()) {Length = 3,Width = 4},
                new Widget(new DbLogger()) { Length = 5,Width = 6}
            });
           
            
            
             var model = new {Message= "Success !! Dependency Inversion Principle."};
            return View(model);
        }
    }
}