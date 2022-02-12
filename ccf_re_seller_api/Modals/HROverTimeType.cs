using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfovty")]
    public class HROverTimeType
    {
        [Key]
        public string ovtyid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }
        public string vtyp { get; set; }
        public string remark { get; set; }

        public virtual HROrganizationClass ccforg { get; set; }
    }
}
