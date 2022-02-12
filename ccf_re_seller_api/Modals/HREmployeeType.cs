using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfempt")]
    public class HREmployeeType
    {
        [Key]
        public string empid { get; set; }
        public string typ { get; set; }
        public string remark { get; set; }
    }
}
