using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Models
{
    [Table("ccfulog")]
    public class HRCcfulog
    {
        [Key]
        public string id { get; set; }
        public string ucode { get; set; }
        public DateTime ldate { get; set; }
        public DateTime odate { get; set; }
        public string fdevice { get; set; }
        public string iostatus { get; set; }
        public string token { get; set; }

    }
}
