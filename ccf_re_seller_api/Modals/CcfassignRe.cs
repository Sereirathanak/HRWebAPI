using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccfassign_re")]
    public partial class CcfassignRe
    {
        public string rcode { get; set; }
        public string uid { get; set; }
        public string rdes { get; set; }
        public DateTime adate { get; set; }
        public string aby { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
        [Key]
        public string id { get; set; }

        public virtual CcfroleRe CcfroleRe { get; set; }
        public virtual CcfuserRe CcfuserRe { get; set; }
    }
}
