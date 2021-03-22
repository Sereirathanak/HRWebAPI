using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccfcust_apr")]
    public partial class CcfcustApr
    {
        [Key]
        public string aprcode { get; set; }

        [ForeignKey("CcfreferalCu")]
        public string cid { get; set; }

        [ForeignKey("CcfuserRe")]
        public string uid { get; set; }
        public string remark { get; set; }
        public string status { get; set; }
        public DateTime date { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }

        public virtual CcfreferalCu CcfreferalCu { get; set; }
        public virtual CcfuserRe CcfuserRe { get; set; }
    }
}
