using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCR.Root.Models;
using System.IO;

namespace SCR.Root.Controllers
{
    public class CsvUploadController : Controller
    {
        //
        // GET: /CsvUpload/

        public ActionResult CsvUpload()
        {
            CsvUploadModel objCsvUploadModel = new CsvUploadModel();
            return View();
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult CsvUpload( CsvUploadModel objCsvUploadModel)
        {

           
         

            return View();
        }
    }
}
