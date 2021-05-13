using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ccf_re_seller_api.Modals
{
    [Table("ccfcurrency")]
    public class Currency
    {
        [Key]
        public string curcode { get; set; }
        public string curname { get; set; }
        public string curdes { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
    }

    public class ReturnCurrency
    {
        [Key]
        public string curcode { get; set; }
        public string curname { get; set; }
    }
}
