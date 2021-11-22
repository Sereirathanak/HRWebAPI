using System;
using System.ComponentModel.DataAnnotations;
namespace ccf_re_seller_api.Models
{
    public class HRValidateUserRoles
    {
        [Required(ErrorMessage = "This field is required.")]
        public string ucode { get; set; }

        public string[] rcode { get; set; }
        public string rdes { get; set; }
        public DateTime adate { get; set; }
        public string aby { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
    }
}
