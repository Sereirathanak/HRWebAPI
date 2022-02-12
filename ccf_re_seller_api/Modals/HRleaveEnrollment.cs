using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfler")]
    public class HRleaveEnrollment
    {
        [Key]
        public string lerid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        public int accruyear { get; set; }
        public int accrunum { get; set; }
        public int releav { get; set; }
        public int usleav { get; set; }
        public int acruleav { get; set; }
        public string remark { get; set; }

        public virtual HROrganizationClass ccforg { get; set; }
        public virtual HREmployee ccfpinfo { get; set; }
    }
}
