using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Models
{
    [Table("ccfurassign")]
    public partial class HRCcfassign
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("ccfuser")]
        public string ucode { get; set; }
        [ForeignKey("ccfmrole")]
        public string rcode { get; set; }
        public string rdes { get; set; }
        public DateTime adate { get; set; }
        public string aby { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
       

        //public virtual CcfroleRe CcfroleRe { get; set; }
        //public virtual CcfUserClass CcfuserRe { get; set; }
    }

    public class ReturnAssign
    {
        public string rcode { get; set; }
    }
}