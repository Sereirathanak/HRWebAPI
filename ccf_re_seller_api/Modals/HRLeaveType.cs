using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccflea")]
    public class HRLeaveType
    {
        [Key]
        public string leaid { get; set; }
        [ForeignKey("ccforg")]
        public string orgid { get; set; }
        public string ltyp { get; set; }
        public string num { get; set; }
        public string remark { get; set; }
        public virtual HROrganizationClass ccforg { get; set; }

    }
}
