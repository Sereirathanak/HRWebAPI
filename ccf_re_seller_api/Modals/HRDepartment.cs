using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfdep")]
    public class HRDepartment
    {
        [Key]
        public string depid { get; set; }
        public string depname { get; set; }
        public string remark { get; set; }
    }
}
