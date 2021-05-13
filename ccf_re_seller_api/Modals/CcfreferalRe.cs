using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccfreferal_re")]

    public partial class CcfreferalRe
    {
        
        [Key]
        public string refcode { get; set; }

        [ForeignKey("CcfuserRe")]
        public string uid { get; set; }
        public string refname { get; set; }
        public string refphone { get; set; }
        public DateTime regdate { get; set; }
        public string address { get; set; }
        public string dob { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
        public string nid { get; set; }
        public string job { get; set; }
        public decimal? bal { get; set; }
        public string status { get; set; }
        public string email { get; set; }
        public string verify { get; set; }
        public string typeaccountbank { get; set; }
        public string typeaccountnumber { get; set; }
        public string idtype { get; set; }
        public string idnumber { get; set; }
        public string verifystatus { get; set; }
        public string gender { get; set; }

        public virtual CcfuserRe CcfuserRe { get; set; }
        
    }

    public class TotalRequest
    {
        public int totalCustomer { get; set; }
        public int totalPaddingCustomer { get; set; }
        public int totalLoanCustomer { get; set; }

        public virtual CcfreferalRe CcfreferalRe { get; set; }
    }
}
