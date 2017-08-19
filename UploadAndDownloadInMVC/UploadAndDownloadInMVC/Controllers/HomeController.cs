using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadAndDownloadInMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].FileName != "")
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/uploads/";
                    string filename = Path.GetFileName(Request.Files[upload].FileName);
                    Request.Files[upload].SaveAs(Path.Combine(path, filename));
                }
            }
            return View("Upload");
        }

        public ActionResult Download()
        {
            var dir=new DirectoryInfo(Server.MapPath("~/App_data/uploads/"));
            FileInfo[] files = dir.GetFiles("*.*");
            IList<string> items=new List<string>();
            foreach (var file in files)
            {
                items.Add(file.Name);
            }
            return View(items);
        }

        public FileResult ForceDownload(string fileName)
        {
            var fileVirtualPath = "~/App_Data/uploads/" + fileName;
            return File(fileVirtualPath, "application/force-download",Path.GetFileName(fileVirtualPath));
        }
    }
}