using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Modals;

namespace ccf_re_seller_api.Models
{
    [Table("ccfecom")]
    public class HREmployeeHistory
    {
        [Key]
        public string ecom { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public string com { get; set; }
        public string pos { get; set; }
        public string res { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public string lres { get; set; }
        public string remark { get; set; }

        public virtual HREmployee ccfpinfo { get; set; }

    }
}
