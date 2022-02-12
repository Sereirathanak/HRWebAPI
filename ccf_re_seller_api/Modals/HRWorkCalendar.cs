using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfwkc")]
    public class HRWorkCalendar
    {
        [Key]
        public string wkcid { get; set; }
        [ForeignKey("ccforg")]
        public string orgid { get; set; }
        [ForeignKey("ccfbranch")]
        public string braid { get; set; }
        public string stim { get; set; }
        public string etim { get; set; }
        public int wod { get; set; }
        public string remark { get; set; }

        public virtual HROrganizationClass ccforg { get; set; }
        public virtual HRBranchClass ccfbranch { get; set; }


    }
}
