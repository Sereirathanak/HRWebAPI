using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("exchangecurrency")]
    public class ExchangeRate
    {
        [Key]
        public string xid { get; set; }
        public string xname { get; set; }
        public string exchangeamount { get; set; }
        public DateTime datecreate { get; set; }
        public string curcode { get; set; }
        
    }
}
