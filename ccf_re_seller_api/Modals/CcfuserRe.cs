using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccfuser_re")]
    public partial class CcfuserRe
    {
        

        public decimal uno { get; set; }
        [Key]
        public string uid { get; set; }
        public string pwd { get; set; }
        public string uname { get; set; }
        public string uotpcode { get; set; }
        public DateTime datecreate { get; set; }
        public string ustatus { get; set; }
        public DateTime exdate { get; set; }
        public string utype { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
        public string mtoken { get; set; }
        public string ufacebook { get; set; }
        public string phone { get; set; }

        public virtual ICollection<CcfreferalRe> ccfreferalRe { get; set; }


    }
}
