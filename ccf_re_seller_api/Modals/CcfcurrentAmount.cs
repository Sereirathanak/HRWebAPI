using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccf_current_amount")]
    public partial class CcfcurrentAmount
    {
        [Key]
        public string caid { get; set; }

        [ForeignKey("CcfuserRe")]
        public string uid { get; set; }
        public string status { get; set; }
        public int currentamount { get; set; }
        public DateTime createdate { get; set; }

        
        public virtual CcfuserRe CcfuserRe { get; set; }
    }

    public class UserCurrentAmount
    {
        public string uid { get; set; }
        public string status { get; set; }
        public string currentamount { get; set; }
        public string uname { get; set; }
        public string phone { get; set; }
        public string userStatus { get; set; }

    }
}
