using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Models
{
    [Table("ccfbranch")]
    public partial class HRBranchClass
    {
        [Key]
        public string brcode { get; set; }
        public string bname { get; set; }

    }

}
