using System;
using System.ComponentModel.DataAnnotations;

namespace ccf_re_seller_api.CO_Models
{
    public class Users
    {


        //[ForeignKey("Loan")]
        //public string ucode { get; set; }
        [Key]
        public string uid { get; set; }

        public string upassword { get; set; }

        public int ulevel { get; set; }

        public string btlcode { get; set; }

        public string bcode { get; set; }

        public DateTime datecreate { get; set; }

        public string isapprover { get; set; }

        public string ustatus { get; set; }

        public DateTime exdate { get; set; }

        public string mtoken { get; set; }

        public string uname { get; set; }

        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
    }
    
}

