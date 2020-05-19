using Capstone.Models;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    [Authorize]
    public class MaintenanceController : Controller
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(MaintenanceController));
        // GET: Maintenance
        public ActionResult Hospitals()
        {
            //using (MedicalEntities db = new MedicalEntities())
            //{
            //    LOV_Type_Ref list = (from l in db.LOV_Type_Ref
            //                         where l.ID == (LOV_Type_Ref_type?)LOV_Type_key
            //                         select l).FirstOrDefault();

            //    return View(list ?? new LOV_Type_Ref());
            //}
            return View();
        }
        public JsonResult GetHospitalListJSON()
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var list = (from l in db.hospitals
                            select l).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public static IEnumerable<hospital> GetHospitalList()
        {
            IEnumerable<hospital> ReturnList;

            using (MedicalEntities db = new MedicalEntities())
            {
                ReturnList = db.hospitals.Where(x=>x.active == true).OrderBy(x=>x.name).ToList();
            }

            return ReturnList;
        }

        class HospitalInput
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool active { get; set; }
            public System.DateTime created_on { get; set; }
        }
        public JsonResult UpdateHospital_List(string lines)
        {
            try
            {
                var ListOfHospitals = JsonConvert.DeserializeObject<List<HospitalInput>>(lines);
                using (MedicalEntities db = new MedicalEntities())
                {
                    foreach (HospitalInput HospitalValues in ListOfHospitals)
                    {

                        if (!(db.hospitals.Any(t => t.id == HospitalValues.id)))
                        {
                            //add new hospital
                            hospital NewHospital = new hospital();

                            NewHospital.name = HospitalValues.name;
                            NewHospital.active = HospitalValues.active;
                            NewHospital.created_on = DateTime.Now;
                            db.hospitals.Add(NewHospital);

                        }
                        else
                        {
                            hospital existingHospital = db.hospitals.Where(t => t.id == HospitalValues.id).FirstOrDefault();
                            bool SomethingChanged = false;

                            if (existingHospital.name != HospitalValues.name || existingHospital.active != HospitalValues.active)
                                SomethingChanged = true;

                            existingHospital.name = HospitalValues.name;
                            existingHospital.active = HospitalValues.active;

                            if (SomethingChanged)
                                db.Entry(existingHospital).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                }
                return Json(new { });
            }
            catch (Exception e)
            {
                log.Error(e);
                log.Error(lines);
                throw e;
            }
        }

        public ActionResult HospitalQuestions(int HospitalID)
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                hospital thisHospital = db.hospitals.Where(x => x.id == HospitalID).FirstOrDefault();
                if (thisHospital == null || thisHospital.id == 0)
                    return RedirectToAction("hospital");
                return View(thisHospital);
            }
        }
        public JsonResult GetHospitalQuestionsList(int HospitalID)
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var list = db.questions.Where(x => x.hospital_id == HospitalID).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        class HospitalQuestionsInput
        {
            public int id { get; set; }
            public string text { get; set; }
            public bool active { get; set; }
            public bool multiple_choice { get; set; }
            public System.DateTime created_on { get; set; }
        }
        public JsonResult UpdateHospitalQuestions_List(int HospitalID, string lines)
        {
            try
            {
                var ListOfHospitalQuestions = JsonConvert.DeserializeObject<List<HospitalQuestionsInput>>(lines);
                using (MedicalEntities db = new MedicalEntities())
                {
                    foreach (HospitalQuestionsInput HospitalQuestionValues in ListOfHospitalQuestions)
                    {

                        if (!(db.questions.Any(t => t.id == HospitalQuestionValues.id)))
                        {
                            //add new question
                            question NewQuestion = new question();

                            NewQuestion.text = HospitalQuestionValues.text;
                            NewQuestion.active = HospitalQuestionValues.active;
                            NewQuestion.created_on = DateTime.Now;
                            NewQuestion.hospital_id = HospitalID;
                            NewQuestion.user_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                            NewQuestion.free_text_field = false;
                            NewQuestion.multiple_choice = true;

                            db.questions.Add(NewQuestion);

                        }
                        else
                        {
                            question existingHospitalQuestion = db.questions.Where(t => t.id == HospitalQuestionValues.id).FirstOrDefault();
                            existingHospitalQuestion.free_text_field = false;
                            existingHospitalQuestion.multiple_choice = true;
                            

                            existingHospitalQuestion.text = HospitalQuestionValues.text;
                            existingHospitalQuestion.active = HospitalQuestionValues.active;

                            db.Entry(existingHospitalQuestion).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                }
                return Json(new { });
            }
            catch (Exception e)
            {
                log.Error(e);
                log.Error(lines);
                throw e;
            }
        }
        public ActionResult HospitalQuestionOptions(int QuestionID)
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                question thisQuestion = db.questions.Where(x => x.id == QuestionID).FirstOrDefault();
                if (thisQuestion == null || thisQuestion.id == 0)
                    return RedirectToAction("hospital");
                return View(thisQuestion);
            }
        }
        public JsonResult GetHospitalQuestionOptionsList(int QuestionID)
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var list = db.options.Where(x => x.question_id == QuestionID).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        class HospitalQuestionOptionsInput
        {
            public int id { get; set; }
            public string text { get; set; }
            public bool active { get; set; }
            public System.DateTime created_on { get; set; }
        }
        public JsonResult UpdateHospitalQuestionOptions_List(int QuestionID, string lines)
        {
            try
            {
                var ListOfHospitalQuestionOptions = JsonConvert.DeserializeObject<List<HospitalQuestionOptionsInput>>(lines);
                using (MedicalEntities db = new MedicalEntities())
                {
                    foreach (HospitalQuestionOptionsInput HospitalQuestionOptionValues in ListOfHospitalQuestionOptions)
                    {

                        if (!(db.options.Any(t => t.id == HospitalQuestionOptionValues.id)))
                        {
                            //add new hospital
                            option NewHospitalquestionOption = new option();

                            NewHospitalquestionOption.text = HospitalQuestionOptionValues.text;
                            NewHospitalquestionOption.active = HospitalQuestionOptionValues.active;
                            NewHospitalquestionOption.created_on = DateTime.Now;
                            NewHospitalquestionOption.question_id = QuestionID;
                            db.options.Add(NewHospitalquestionOption);

                        }
                        else
                        {
                            option existingHospitalQuestionOption = db.options.Where(t => t.id == HospitalQuestionOptionValues.id).FirstOrDefault();
                            bool SomethingChanged = false;

                            if (existingHospitalQuestionOption.text != HospitalQuestionOptionValues.text || existingHospitalQuestionOption.active != HospitalQuestionOptionValues.active)
                                SomethingChanged = true;

                            existingHospitalQuestionOption.text = HospitalQuestionOptionValues.text;
                            existingHospitalQuestionOption.active = HospitalQuestionOptionValues.active;

                            if (SomethingChanged)
                                db.Entry(existingHospitalQuestionOption).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                }
                return Json(new { });
            }
            catch (Exception e)
            {
                log.Error(e);
                log.Error(lines);
                throw e;
            }
        }
        public static string GetHospitalName(int HospitalID)
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                hospital thisHospital = db.hospitals.Where(x => x.id == HospitalID).FirstOrDefault();
                if (thisHospital == null || thisHospital.id == 0)
                    return "";
                return thisHospital.name;
            }
        }
    }
}