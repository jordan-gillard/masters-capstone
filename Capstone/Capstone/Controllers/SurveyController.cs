using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        // GET: Survey
        public ActionResult Index()
        {
            SurveyModel surveyModel = new SurveyModel();
            surveyModel.AppointmentDates = RandomDateTimeList();
            return View(surveyModel);
        }

        private Random DayGen = new Random();
        private Random MinuteGen = new Random();
        public List<String> RandomDateTimeList()
        {
            List<String> listOfDates = new List<string>();
            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;
            for (int i = 0; i < 5; i++)
            {
                DateTime tempDateTime = start.AddDays(DayGen.Next(range));
                tempDateTime = tempDateTime.AddMinutes(420 + MinuteGen.Next(500));
                listOfDates.Add(tempDateTime.ToString("MM/dd/yyyy h:mm tt"));
            }
            return listOfDates;
        }
        public ActionResult SubmitSurvey(SurveyModel model, string returnUrl)
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                SurveyResult survey = new SurveyResult();
                survey.AppointmentDate = model.AppointmentDate;
                survey.PhysicanWait = model.PhysicanWait;
                survey.PhysicianTime = model.PhysicanTime;
                survey.TopicsDiscussed = "Yes".Equals(model.TopicsDiscussed);
                survey.FollowupScheduled = "Yes".Equals(model.FollowupScheduled);
                survey.Recommend = "Yes".Equals(model.Recommend);
                survey.Rating = model.Rating;
                survey.Comments = model.Comments;
                survey.CREATE_DATE = DateTime.Now;
                db.SurveyResults.Add(survey);
                db.SaveChanges();
            }
            return View();
        }
    }
}