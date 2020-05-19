using Capstone.Extensions;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        // GET: Reporting
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FeedbackResults(String HospitalID)
        {
            DataTable dt = new DataTable();
            if (HospitalID != null)
            { 
                dt = GetFeedbackDataTable(HospitalID);
                using (MedicalEntities db = new MedicalEntities())
                {
                    ViewBag.HospitalName = db.hospitals.Where(x => x.id.ToString() == HospitalID).FirstOrDefault().name;
                }
            }
                return View(dt);
        }
        private  string GetLocalFile(String Folder, String FileName)
        {
            string path = Path.Combine(Server.MapPath("~/" + Folder + "/" + FileName));

            string FileString = System.IO.File.ReadAllText(path);

            return FileString;
        }
        private DataTable GetFeedbackDataTable(String HospitalID)
        {
            String SQL = GetLocalFile("SQL", "FeedBackPivoted.sql");
            SQL = String.Format(SQL, HospitalID);
            using (SqlConnection myConnection = getConnection())
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(SQL, myConnection))
                {
                    myCommand.Connection = myConnection;
                    using (SqlDataReader reader = myCommand.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }

            }
        }
        public static SqlConnection getConnection()
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = db.Database.Connection.ConnectionString;
                return con;
            }
        }
        class Feedback
        {
            public DateTime CreatedOn { get; set; }
            public String Question { get; set; }
            public String Response { get; set; }
        }
        public JsonResult GetFeedbackList(String HospitalID)
        {
            DataTable FeedbackDataTable = GetFeedbackDataTable(HospitalID);
            var list = FeedbackDataTable.ToList<Feedback>();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        class PieChartRow
        {
            public decimal y { get; set; }
            public string label { get; set; }
        }
        public JsonResult GetFeedbackScorePercentages()
        {
            List<PieChartRow> list = new List<PieChartRow>();
            using (MedicalEntities db = new MedicalEntities())
            {
                var results = from responses in db.responses
                              join questions in db.questions on responses.question_id equals questions.id
                              join options in db.options on responses.option_id equals options.id
                              where questions.text.ToUpper().Contains("RATE YOUR VISIT") == true
                              select new { response = options.text };
                foreach(var r in results.Distinct())
                {
                    decimal percentage = Math.Round((Convert.ToDecimal(results.Count(x => x.response == r.response))/ Convert.ToDecimal(results.Count()))*100,2);
                    list.Add(new PieChartRow { y = percentage, label = r.response });
                }

            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        class HospitalRatingChartDetail
        {
            public int option { get; set; }
            public int count { get; set; }
        }
        class HospitalRatingChart
        {
            public string HospitalName { get; set; }
            public List<HospitalRatingChartDetail> detail { get; set; }
        }
        public JsonResult GetHospitalFeedbackScores()
        {
            List<HospitalRatingChart> hospitalList = new List<HospitalRatingChart>();
            using (MedicalEntities db = new MedicalEntities())
            {
                var results = from responses in db.responses
                              join questions in db.questions on responses.question_id equals questions.id
                              join options in db.options on responses.option_id equals options.id
                              join hospital in db.hospitals on responses.hospital_id equals hospital.id
                              where questions.text.ToUpper().Contains("RATE YOUR VISIT") == true
                              select new { hospital = hospital.name, response = options.text };
                foreach (var hospital in results.OrderBy(x=>x.hospital).Select(x => x.hospital).Distinct())
                {
                    List<HospitalRatingChartDetail> hospitalListDetail = new List<HospitalRatingChartDetail>();
                    
                    foreach (var r in results.Where(x=>x.hospital == hospital).Distinct())
                    {
                        int count = results.Count(x => x.response == r.response && x.hospital == hospital);
                        int response = Convert.ToInt16(r.response);
                        hospitalListDetail.Add(new HospitalRatingChartDetail { option = response, count = count });
                    }
                    hospitalList.Add(new HospitalRatingChart { HospitalName = hospital, detail = hospitalListDetail });
                }

            }

            return Json(hospitalList, JsonRequestBehavior.AllowGet);
        }
    }
}