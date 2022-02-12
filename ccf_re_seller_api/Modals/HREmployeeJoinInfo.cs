using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Modals;

namespace ccf_re_seller_api.Models
{
    [Table("ccfejon")]
    public class HREmployeeJoinInfo
    {
        private readonly HRContext _context;

        public HREmployeeJoinInfo(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string jonid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        public DateTime jdate { get; set; }
        public string site { get; set; }
        public string dep { get; set; }
        public string pos { get; set; }
        public string sup { get; set; }
        public decimal pmsal { get; set; }
        public decimal msal { get; set; }
        public string stype { get; set; }
        public string con { get; set; }
        public string etype { get; set; }
        public int npm { get; set; }
        public DateTime edate { get; set; }
        public DateTime ecdate { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string incharge { get; set; }


        public virtual HREmployee ccfpinfo { get; set; }

        public string branchName
        {
            get
            {
                var checkbranch = _context?.hrBranchClass.SingleOrDefault(cur => cur.braid == this.site);
                if (checkbranch != null)
                {
                    return checkbranch.braname;
                }
                return "";
            }
        }

        public string departmentName
        {
            get
            {
                var checkDepartment = _context?.department.SingleOrDefault(cur => cur.depid == this.dep);
                if (checkDepartment != null)
                {
                    return checkDepartment.depname;
                }
                return "";
            }
        }
    }
}