using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfuserprofile")]
    public class HRImageProfile
    {

        [Key]
        public string pid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string userid { get; set; }
        public byte[] file { get; set; }

        public virtual HREmployee ccfpinfo { get; set; }

    }

}
