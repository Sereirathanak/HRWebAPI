using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccfreferal_cus")]
    public partial class CcfreferalCu
    {
        [Key]
        public string cid { get; set; }

        [ForeignKey("CcfreferalRe")]
        public string refcode { get; set; }

        [ForeignKey("CcfuserRe")]
        public string uid { get; set; }
        public string cname { get; set; }
        public string phone { get; set; }
        public decimal? lamount { get; set; }
        public string? lpourpose { get; set; }
        public DateTime refdate { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
        public string status { get; set; }


        public virtual CcfreferalRe CcfreferalRe { get; set; }
        public virtual CcfuserRe CcfuserRe { get; set; }
        public virtual ICollection<CcfreferalCusUp> ccfreferalCusUp { get; set; }

    }
}
