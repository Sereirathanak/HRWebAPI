using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfmis")]
    public class HRMissionType
    {
        [Key]
        public string misid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }
        public string mistp { get; set; }
        public string remark { get; set; }

        public virtual HROrganizationClass ccforg { get; set; }

    }
}
