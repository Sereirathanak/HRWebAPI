using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfbranch")]
    public partial class BranchClass
    {
            [Key]
            public string brcode { get; set; }
            public string bname { get; set; }

        }
       
}
