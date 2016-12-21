using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCR.Root.Models;
using System.Net;
using System.Text;
using System.IO;
using SCR.Root.App_Code;
using System.Globalization;
using Newtonsoft.Json;
using System.Data;
using System.Threading;
using System.Configuration;

namespace SCR.Root.Controllers
{
    public class SyncRealtorsController : Controller
    {
        //
        // GET: /SyncRealtors/

        bool boolUpdate = false;
        bool boolAddNRDS = false;
        bool boolIsMemberFile;
        bool boolOfficeResult = false;
        bool boolres = false;
        int insertCount = -1;
        int resultPerc = -1;
        double insertPerc;

        string strDirpath = ConfigurationManager.AppSettings["DirPath"].ToString();
        string strRarfilepath = ConfigurationManager.AppSettings["RarFilePath"].ToString();
        string strZipfilepath = ConfigurationManager.AppSettings["ZipFilePath"].ToString();



        public ActionResult Sync()
        {
            SyncRealtorsModel objSyncRealtorsModel = new SyncRealtorsModel();
            return View(objSyncRealtorsModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Sync(SyncRealtorsModel objSyncRealtorsModel, HttpPostedFileBase fupdCSVUpload, HttpPostedFileBase fupdCSVUpload1)
        {

            UserSession userSession = new UserSession();
            if (userSession.Exists)
            {
                try
                {

                    string strfileType = string.Empty;
                    string strfilePath = string.Empty;
                    string strfileLength = string.Empty;
                    string ext = string.Empty;
                   

                    #region File save
                    //check to make sure a file is selected
                    if (fupdCSVUpload != null)
                    {
                        //create the path to save the file to
                        string filename = fupdCSVUpload.FileName;


                        if (filename.Length != 0)
                        {
                            ext = Path.GetExtension(filename);


                            if (ext.ToLower() == ".zip")//ext.ToLower() == ".rar" ||
                            {


                                if (!Directory.Exists(strDirpath))
                                {
                                    Directory.CreateDirectory(strDirpath);
                                }

                                strfilePath = Path.Combine(strDirpath, fupdCSVUpload.FileName);
                               
                                    if (filename.ToUpper().Contains("O00"))
                                    {
                                        boolIsMemberFile = false;
                                        objSyncRealtorsModel.HddnBoolIsMemberFile = 0;

                                    }
                                    else if (filename.ToUpper().Contains("M00"))
                                    {
                                        boolIsMemberFile = true;
                                        objSyncRealtorsModel.HddnBoolIsMemberFile = 1;
                                    }

                                    strfileType = fupdCSVUpload.ContentType;
                                    strfileLength = Convert.ToString(fupdCSVUpload.ContentLength);
                                

                                //save the file to our local path
                                    //if (filename.ToUpper().Contains(".RAR"))
                                    //{
                                    //    fupdCSVUpload.SaveAs(strRarfilepath);
                                    //}
                                  //  else
                                 
                                if (filename.ToUpper().Contains(".ZIP"))

                                    {
                                        fupdCSVUpload.SaveAs(strZipfilepath);
                                    }

                                TempData["error"] = "File uploaded successfully.";
                                TempData["errtitle"] = "Sync Realtors";
                                TempData["errType"] = "success";
                                return View(objSyncRealtorsModel);
                            }
                            else
                            {
                                TempData["error"] = "Allowed only files with .zip extention.";
                                TempData["errtitle"] = "Sync Realtors";
                                TempData["errType"] = "warning";
                                return View(objSyncRealtorsModel);
                            }
                        }
                        else
                        {
                            TempData["error"] = "Please upload file.";
                            TempData["errtitle"] = "Sync Realtors";
                            TempData["errType"] = "warning";
                            return View(objSyncRealtorsModel);
                        }

                    #endregion File save

                        #region Read data from CSV file

                        /*//
                        using (CsvFileReader reader = new CsvFileReader(strfilePath))
                        {
                            CsvRow row = new CsvRow();
                            int i = 0;
                            while (reader.ReadRow(row))
                            {
                                i++;
                                if (i == 7)
                                {
                                    objSyncRealtorsModel.Filename = filename.Trim();
                                    objSyncRealtorsModel.Hddnfilepath = strfilePath.Trim();
                                    objSyncRealtorsModel.HddnTransmittalBatchNumber = Convert.ToInt32(row[2]);
                                    objSyncRealtorsModel.HddnTransactionTotal = Convert.ToInt32(row[4]);
                                }

                            }
                        }*/
                        #endregion Read data from CSV file

                   
                    }

                    else  if (fupdCSVUpload1 != null)
                    {
                        //create the path to save the file to
                        string filename = fupdCSVUpload1.FileName;


                        if (filename.Length != 0)
                        {
                            ext = Path.GetExtension(filename);


                            if (ext.ToLower() == ".xls")//ext.ToLower() == ".rar" ||
                            {


                                if (!Directory.Exists(strDirpath))
                                {
                                    Directory.CreateDirectory(strDirpath);
                                }

                                strfilePath = Path.Combine(strDirpath, fupdCSVUpload1.FileName);
                               
                                    //if (filename.ToUpper().Contains("O00"))
                                    //{
                                    //    boolIsMemberFile = false;
                                    //    objSyncRealtorsModel.HddnBoolIsMemberFile = 0;

                                    //}
                                    //else if (filename.ToUpper().Contains("M00"))
                                    //{
                                    //    boolIsMemberFile = true;
                                    //    objSyncRealtorsModel.HddnBoolIsMemberFile = 1;
                                    //}

                                   // strfileType = fupdCSVUpload.ContentType;
                                   // strfileLength = Convert.ToString(fupdCSVUpload.ContentLength);
                                

                                //save the file to our local path
                                    //if (filename.ToUpper().Contains(".RAR"))
                                    //{
                                    //    fupdCSVUpload.SaveAs(strRarfilepath);
                                    //}
                                  //  else
                                string path_act = strDirpath + filename;
                                if (filename.ToUpper().Contains(".XLS"))

                                    {
                                        fupdCSVUpload1.SaveAs(path_act);
                                    }

                                TempData["error"] = "File uploaded successfully.";
                                TempData["errtitle"] = "Sync Realtors";
                                TempData["errType"] = "success";
                                return View(objSyncRealtorsModel);
                            }
                            else
                            {
                                TempData["error"] = "Allowed only files with .zip extention.";
                                TempData["errtitle"] = "Sync Realtors";
                                TempData["errType"] = "warning";
                                return View(objSyncRealtorsModel);
                            }
                        }
                        else
                        {
                            TempData["error"] = "Please upload file.";
                            TempData["errtitle"] = "Sync Realtors";
                            TempData["errType"] = "warning";
                            return View(objSyncRealtorsModel);
                        }

                   


                   
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return View(objSyncRealtorsModel);
            }
            else
            {
                TempData["expires"] = "true";
                return RedirectToAction("Users", "Login");
            }
        }


        /// <summary>
        /// Get Date String Split
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        private string GetDateStringSplit(string datestr)
        {
            string mm = string.Empty;
            string dd = string.Empty;
            string yyyy = string.Empty;
            string dateresult = string.Empty;
            if (datestr != "")
            {
                if (datestr.Length == 8)
                {
                    mm = datestr.Substring(0, 2);
                    dd = datestr.Substring(2, 2);
                    yyyy = datestr.Substring(4, 4);
                    dateresult = dd + "/" + mm + "/" + yyyy;
                }
                else if (datestr.Length == 7)
                {
                    mm = datestr.Substring(0, 2);
                    dd = datestr.Substring(2, 1);
                    yyyy = datestr.Substring(3, 4);
                    dateresult = "0" + dd + "/" + mm + "/" + yyyy;
                }
            }
            return dateresult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestr"></param>
        /// <returns></returns>
        private string GetTimeStringSplit(string timestr)
        {
            string hh = string.Empty;
            string mm = string.Empty;
            string ss = string.Empty;
            string timeresult = string.Empty;
            if (timestr != "")
            {
                hh = timestr.Substring(0, 2);
                mm = timestr.Substring(2, 2);
                ss = timestr.Substring(3, 2);
                timeresult = hh + ":" + mm + ":" + ss;
            }
            return timeresult;
        }


      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      /// 

        [AcceptVerbs(HttpVerbs.Get)]
        public void DataImportCSV()
        {

            //Thread thread = new Thread(DataImport);
            //thread.Start();

           

        }

        // [AcceptVerbs(HttpVerbs.Get)]
       //public void DataImport()
        //{
        //    SyncRealtorsModel objSyncRealtorsModel = new SyncRealtorsModel();
        //    SyncRealtorsDAL objSyncRealtorsDAL = new SyncRealtorsDAL();
        //    RootObject objRootObject = new RootObject();
        //    try
        //    {
        //        string strfilePath = "";
        //        if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["filePath"])))
        //        {
        //            if (Convert.ToString(Request.QueryString["filePath"]) == "")
        //            {
        //                strfilePath = null;
        //            }
        //            else
        //            {
        //                strfilePath = Convert.ToString(Convert.ToString(Request.QueryString["filePath"]));
        //            }
        //        }

        //        if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["boolIsMemberFile"])))
        //        {
        //            if (Convert.ToString(Request.QueryString["boolIsMemberFile"]) == "1")
        //            {
        //                boolIsMemberFile = true;
        //            }
        //            else
        //            {
        //                boolIsMemberFile = false;
        //            }
        //        }

                
        //        DateTime nowdate = DateTime.Now;
        //        objSyncRealtorsModel.InsertedOn = nowdate;
              
        //        using (CsvFileReader reader = new CsvFileReader(strfilePath))
        //        {
        //            objSyncRealtorsDAL.delete_data(strfilePath, boolIsMemberFile);
        //            CsvRow row = new CsvRow();
        //            int i = 0;
        //            bool brkflag = false;
                    
        //                while (reader.ReadRow(row))
        //                {
        //                    //while (!brkflag)
        //                    //{
        //                        i++;

        //                        if (i > 6)
        //                        {
        //                            //if (i == 16)
        //                            //{
        //                            //    brkflag = true;
        //                            //}  //test while breakflag check

        //                            if (boolIsMemberFile)
        //                            {
        //                                #region Member File Data
        //                                int recordType = Convert.ToInt32(row[0]);
        //                                objSyncRealtorsModel.TransmittalBatchNumber = Convert.ToInt32(row[2]);
        //                                objSyncRealtorsModel.TransactionNumber = Convert.ToInt32(row[3]);
        //                                objSyncRealtorsModel.TransactionDate = Convert.ToInt32(row[5]);
        //                                objSyncRealtorsModel.TransactionTime = Convert.ToInt32(row[2]);
        //                                objSyncRealtorsModel.MemberId = Convert.ToInt32(row[8]);
        //                                string strlastName = Convert.ToString(row[10]);
        //                                string strfirstName = Convert.ToString(row[11]);

        //                                objSyncRealtorsModel.LastName = strlastName;
        //                                objSyncRealtorsModel.FirstName = strfirstName;

        //                                objSyncRealtorsModel.MailCity = Convert.ToString(row[26]);
        //                                objSyncRealtorsModel.MailState = Convert.ToString(row[27]);
        //                                objSyncRealtorsModel.MailZIPCode = Convert.ToString(row[28]);
        //                                objSyncRealtorsModel.MailZIPCode6 = Convert.ToString(row[29]);
        //                                objSyncRealtorsModel.OfficeId = Convert.ToInt32(row[37]);
        //                                objSyncRealtorsModel.MemberType = Convert.ToString(row[38]);

        //                                objSyncRealtorsModel.Status = Convert.ToString(row[41]);
        //                                objSyncRealtorsModel.RELicenseNumber = Convert.ToString(row[48]);
        //                                objSyncRealtorsModel.PrimaryAssociationID = Convert.ToInt32(row[56]);
        //                                objSyncRealtorsModel.PrimaryStateAssociationID = Convert.ToString(row[57]);

        //                                objSyncRealtorsModel.PrimaryFieldofBusiness = Convert.ToString(row[96]);
        //                                objSyncRealtorsModel.MailAddress = Convert.ToString(row[103]);

        //                                objSyncRealtorsModel.NRDSStatus = 0; // 0 => NRDS Synch not started, 1 => Record Found, 2 => No Record, 3 => Time Out. First time insertion its 0 .
        //                                boolres = objSyncRealtorsDAL.ImportMemberCSVFilestoTemp(objSyncRealtorsModel); //save member csv file data to temp 
        //                                if (boolres)
        //                                {

        //                                    if (objSyncRealtorsModel.MemberId != 0 && objSyncRealtorsModel.LastName != null)
        //                                    {
        //                                        try
        //                                        {
        //                                            var req = (HttpWebRequest)WebRequest.Create("https://api.realtor.org:7788/Member-1.0/search/idLastName/" + objSyncRealtorsModel.MemberId + "/" + objSyncRealtorsModel.LastName);
        //                                            req.Credentials = new NetworkCredential("mqssclina", "8Af0xByOUPhQ");
        //                                            var response = req.GetResponse();
        //                                            string JSON = "";
        //                                            using (var streamReader = new System.IO.StreamReader(response.GetResponseStream()))
        //                                            {
        //                                                JSON = streamReader.ReadToEnd();
        //                                            }

        //                                            objRootObject = JsonConvert.DeserializeObject<RootObject>(JSON);

        //                                            if (objSyncRealtorsModel.MemberId == objRootObject.member.MemberId && objSyncRealtorsModel.LastName == objRootObject.member.MemberLastName)
        //                                            {
        //                                                objSyncRealtorsModel.NRDSStatus = 1; // 0 => NRDS Synch not started, 1 => Record Found, 2 => No Record, 3 => Time Out. First time insertion its 0 .
        //                                                boolUpdate = objSyncRealtorsDAL.updateNRDSStatus(objSyncRealtorsModel);
        //                                            }
        //                                            else
        //                                            {
        //                                                objSyncRealtorsModel.NRDSStatus = 2; // 0 => NRDS Synch not started, 1 => Record Found, 2 => No Record, 3 => Time Out. First time insertion its 0 .
        //                                                boolUpdate = objSyncRealtorsDAL.updateNRDSStatus(objSyncRealtorsModel);
        //                                            }

        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            objSyncRealtorsModel.NRDSStatus = 3; // 0 => NRDS Synch not started, 1 => Record Found, 2 => No Record, 3 => Time Out. First time insertion its 0 .
        //                                            boolUpdate = objSyncRealtorsDAL.updateNRDSStatus(objSyncRealtorsModel);
        //                                        }
        //                                    }
        //                                    if (boolUpdate)
        //                                    {

        //                                        try
        //                                        {
        //                                            if (objSyncRealtorsModel.TransmittalBatchNumber != 0)
        //                                            {
        //                                                DateTime now_date = DateTime.Now;
        //                                                objRootObject.member.InsertedOn = now_date;
        //                                                objRootObject.member.TransmittalBatchNumber = objSyncRealtorsModel.TransmittalBatchNumber;
        //                                                boolAddNRDS = objSyncRealtorsDAL.addNRDSJsonData(objRootObject);
        //                                            }
        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            //TempData["error"] = ex.Message;
        //                                            //TempData["errtitle"] = "Sync Realtors";
        //                                            //TempData["errType"] = "error";
        //                                        }
        //                                    }


        //                                }

        //                                #endregion Member File Data
        //                            }
        //                            else
        //                            {
        //                                #region Office File Data
        //                                try
        //                                {
        //                                    objSyncRealtorsModel.RecordType = Convert.ToInt32(row[0]);
        //                                    objSyncRealtorsModel.SenderId = Convert.ToInt32(row[1]);
        //                                    objSyncRealtorsModel.TransmittalBatchNumber = Convert.ToInt32(row[2]);
        //                                    objSyncRealtorsModel.TransactionNumber = Convert.ToInt32(row[3]);
        //                                    objSyncRealtorsModel.TransactionTotal = Convert.ToInt32(row[4]);
        //                                    objSyncRealtorsModel.OfficeTransactionDate = Convert.ToInt32(row[5]);
        //                                    objSyncRealtorsModel.TransactionTime = Convert.ToInt32(row[6]);
        //                                    objSyncRealtorsModel.RecordChangeType = Convert.ToString(row[7]);
        //                                    objSyncRealtorsModel.AssociationId = Convert.ToInt32(row[8]);
        //                                    objSyncRealtorsModel.OfficeId = Convert.ToInt32(row[9]);
        //                                    objSyncRealtorsModel.OfficeBusinessName = Convert.ToString(row[12]);
        //                                    objSyncRealtorsModel.SortSequence = Convert.ToString(row[14]);
        //                                    objSyncRealtorsModel.StreetAddress = Convert.ToString(row[15]);
        //                                    objSyncRealtorsModel.StreetCity = Convert.ToString(row[17]);
        //                                    objSyncRealtorsModel.StreetState = Convert.ToString(row[18]);
        //                                    objSyncRealtorsModel.StreetZIP = Convert.ToString(row[19]);
        //                                    objSyncRealtorsModel.OfficeAreaCode = Convert.ToInt32(row[27]);
        //                                    objSyncRealtorsModel.OfficePhoneNumber = Convert.ToInt32(row[28]);
        //                                    objSyncRealtorsModel.OfficeContactDR = Convert.ToInt32(row[40]);
        //                                    objSyncRealtorsModel.Status = Convert.ToString(row[42]);
        //                                    objSyncRealtorsModel.NMSalespersonCount = Convert.ToInt32(row[46]);
        //                                    objSyncRealtorsModel.PointOfEntry = Convert.ToInt32(row[50]);
        //                                    objSyncRealtorsModel.PrimaryStateCode = Convert.ToInt32(row[54]);
        //                                    objSyncRealtorsModel.OfficeContactManager = Convert.ToInt32(row[56]);

        //                                    boolOfficeResult = objSyncRealtorsDAL.ImportOfficeCSVFiles(objSyncRealtorsModel);//save office csv file data
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    TempData["error"] = ex.Message;
        //                                    TempData["errtitle"] = "Sync Realtors";
        //                                    TempData["errType"] = "error";
        //                                }
        //                                #endregion Office File Data
        //                            }

        //                           // Thread.Sleep(50);
        //                        }
                              
        //                   // }//test while breakflag check
        //                }
                   
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        TempData["error"] = ex.Message;
        //        TempData["errtitle"] = "Sync Realtors";
        //        TempData["errType"] = "error";
        //    }

        //    //return new JsonResult
        //    //  {

        //    //      Data = Url.Action("Sync", objSyncRealtorsModel),
        //    //      JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    //  };
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult UpdateProgress()
        {

            int trantotal = 0;
            int tranbatchno = 0;
            SyncRealtorsDAL syncRealtorsDAL = new SyncRealtorsDAL();
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["TranTotal"])))
            {
                if (Convert.ToString(Request.QueryString["TranTotal"]) == "")
                {
                    trantotal = 0;
                }
                else
                {
                    trantotal = Convert.ToInt32(Convert.ToString(Request.QueryString["TranTotal"]));
                }
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["TranBatchNumber"])))
            {
                if (Convert.ToString(Request.QueryString["TranBatchNumber"]) == "")
                {
                    tranbatchno = 0;
                }
                else
                {
                    tranbatchno = Convert.ToInt32(Convert.ToString(Request.QueryString["TranBatchNumber"]));
                }
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["boolIsMemberFile"])))
            {
                if (Convert.ToString(Request.QueryString["boolIsMemberFile"]) == "1")
                {
                    boolIsMemberFile = true;
                }
                else
                {
                    boolIsMemberFile = false;
                }
            }

            if (boolIsMemberFile)
            {
                insertCount = syncRealtorsDAL.getDataImportProgress(tranbatchno, 1);
            }
            else
            {
                insertCount = syncRealtorsDAL.getDataImportProgress(tranbatchno, 2);
            }
             
          //  resultPerc = (int)Math.Round(((double)insertCount % trantotal)*100);
         double   resultPerc1 = (double)(insertCount *100 ) / trantotal;
            //resultPerc = insertCount;
            return new JsonResult
            {
                Data = Math.Round(resultPerc1,2),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }


}
