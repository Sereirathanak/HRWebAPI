using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ccf_re_seller_api.Models
{
    public class HREmployeeHistory
    {
        [Key]
        public string ecom { get; set; }
        [ForeignKey("pinfo")]
        public string eid { get; set; }
        public string com { get; set; }
        public string pos { get; set; }
        public string res { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public string lres { get; set; }
        public string remark { get; set; }

    }
}
