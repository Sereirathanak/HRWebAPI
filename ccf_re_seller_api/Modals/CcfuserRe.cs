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
        public int level { get; set; }
        public string staffid { get; set; }
        public string staffposition { get; set; }
        public string brcode { get; set; }
        public string job { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        //
        public string dob { get; set; }
        public string idtype { get; set; }
        public string idnumber { get; set; }
        public string banktype { get; set; }
        public string banknumber { get; set; }
        public string verifystatus { get; set; }
        public string gender { get; set; }

        

        public virtual ICollection<CcfreferalRe> ccfreferalRe { get; set; }

    }
}
