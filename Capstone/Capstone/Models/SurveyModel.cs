using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class SurveyModel
    {
        public DateTime AppointmentDate { get; set; }
        public List<String> AppointmentDates { get; set; }
        public string PhysicanWait { get; set; }
        public string PhysicanTime { get; set; }
        public string TopicsDiscussed { get; set; }
        public string FollowupScheduled { get; set; }
        public string Recommend { get; set; }
        public string Rating { get; set; }
        public string Comments { get; set; }
    }
}