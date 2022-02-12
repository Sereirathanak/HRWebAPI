using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace ccf_re_seller_api.Modals
{
    [Table("ccfmessages")]
    public class HRCcfmessage
    {
        [Key]
        public string id { get; set; }
        public string ucode { get; set; }
        public string bcode { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int mstatus { get; set; }
        public DateTime createdate { get; set; }
        public string lcode { get; set; }
        public string rcode { get; set; }
        public string status { get; set; }
        public string imgurl { get; set; }
    }


    public class ReturnHRMessage
    {
        public int totalMessage { get; set; }
        public int totalRead { get; set; }
        public int totalUnread { get; set; }
        public List<HRCcfmessage> listMessages { get; set; }
    }

}
