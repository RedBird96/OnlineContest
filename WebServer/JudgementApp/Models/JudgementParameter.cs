using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class JudgementParameter
    {

       
        public string UserName { get; set; }
        public string FKCompany { get; set; }
        public string ProblemName { get; set; }
        public List<JudgmentQ> Result { get; set; }

    }
    public class JudgmentQ
    {
        public int ID { get; set; }
        public string QResult { get; set; }
        public string Param { get; set; }
        public string QType { get; set; }
    }
}