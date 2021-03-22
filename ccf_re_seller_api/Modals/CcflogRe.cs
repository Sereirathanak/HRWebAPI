using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccflog_re")]
    public partial class CcflogRe
    {
        [ForeignKey("Ccfuser")]
        public string uid { get; set; }
        public DateTime ldate { get; set; }
        public DateTime odate { get; set; }
        public string fdevice { get; set; }
        public char iostatus { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }

        [Key]
        public string id { get; set; }

        public virtual CcfuserRe CcfuserRe { get; set; }
    }
}
