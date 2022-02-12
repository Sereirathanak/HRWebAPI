using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
   [Table("ccforg")]
    public class HROrganizationClass
    {
        [Key]
        public string orgid { get; set; }
        public string orgname { get; set; }
        public string regno { get; set; }
        public string vat { get; set; }
        public string ind { get; set; }
        public string add { get; set; }
        public string cp { get; set; }
        public string dis { get; set; }
        public string com { get; set; }
        public string vil { get; set; }
        public string roa { get; set; }
        public string no { get; set; }
        public string con { get; set; }
        public string ema { get; set; }
        public string web { get; set; }
        public string remark { get; set; }

    }
}
