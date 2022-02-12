using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfuserprofile")]
    public class HRImageProfile
    {
        [Key]
        public string pid { get; set; }
        public string userid { get; set; }
        public byte[] file { get; set; }
    }

}
