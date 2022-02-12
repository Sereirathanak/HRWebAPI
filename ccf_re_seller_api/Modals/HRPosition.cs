using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfpos")]
    public class HRPosition
    {
        [Key]
        public string posid { get; set; }
        public string pos { get; set; }
        public string tit { get; set; }
        public string remark { get; set; }
    }
}
