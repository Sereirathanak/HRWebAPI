using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfmapzone")]
    public class HRMapZoneClass
    {
        [Key]
        public string zoneid { get; set; }
        [ForeignKey("ccfbranch")]
        public string braid { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string remark { get; set; }
        public DateTime datecreate { get; set; }
        public string cby { get; set; }


        public virtual HRBranchClass ccfbranch { get; set; }
    }
}
