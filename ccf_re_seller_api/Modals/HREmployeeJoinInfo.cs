using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ccf_re_seller_api.Models
{
    [Table("ejon")]
    public class HREmployeeJoinInfo
    {
        [Key]
        public string jonid { get; set; }

        [ForeignKey("pinfo")]
        public string eid { get; set; }

        public DateTime jdate { get; set; }
        public string site { get; set; }
        public string dep { get; set; }
        public string pos { get; set; }
        public string sup { get; set; }
        public int pmsal { get; set; }
        public int msal { get; set; }
        public string stype { get; set; }
        public string con { get; set; }
        public string etype { get; set; }
        public int npm { get; set; }
        public DateTime edate { get; set; }
        public DateTime ecdate { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        //public string u1 { get; set; }
        //public string u2 { get; set; }
        //public string u3 { get; set; }
        //public string u4 { get; set; }
        //public string u5 { get; set; }

        //[Key]
        //public string jonid { get; set; }
        //[ForeignKey("pinfo")]
        //public string eid { get; set; }
        //public DateTime jdate { get; set; }
        //public string site { get; set; }

        //public string dep { get; set; }
        //public string pos { get; set; }
        //public DateTime edate { get; set; }

        //public DateTime ecdate { get; set; }


    }
}