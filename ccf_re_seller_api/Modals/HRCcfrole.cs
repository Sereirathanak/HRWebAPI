using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Models
{
    [Table("ccfmrole")]
    public partial class HRCcfrole
    {

        [Key]
        public string rcode { get; set; }
        public string rname { get; set; }
        public string rdes { get; set; }
        public DateTime cdate { get; set; }
        public string cby { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }

    }
}
