using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Modals;

namespace ccf_re_seller_api.Models
{
    [Table("ccfbranch")]
    public partial class HRBranchClass
    {
        [Key]
        public string braid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }
        public string createBy { get; set; }
        public string braname { get; set; }
        public string typ { get; set; }
        public string braadd { get; set; }
        public string cp { get; set; }
        public string dis { get; set; }
        public string com { get; set; }
        public string vil { get; set; }
        public string roa { get; set; }
        public string no { get; set; }
        public string con { get; set; }
        public string ema { get; set; }
        public string man { get; set; }
        public string remark { get; set; }


        public virtual HROrganizationClass ccforg { get; set; }


    }

}
