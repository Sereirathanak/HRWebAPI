using System;
using System.Collections.Generic;

namespace ccf_re_seller_api.Models
{
    public class HRAuthentication
    {
        public string ucode { get; set; }
        public string uid { get; set; }
        public string eid { get; set; }
        public string uname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string pwe { get; set; }
        public string ufacebook { get; set; }
        public string mtoken { get; set; }
        public string ustatus { get; set; }
        public int level { get; set; }
        public string brcode { get; set; }
        public string u5 { get; set; }
        public string changePassword { get; set; }
        public string token { get; set; }


        public DateTime datecreate { get; set; }
        public string isapprover { get; set; }
        public DateTime exdate { get; set; }

        public List<string> roles { get; set; }

    }
}
