using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccfmessages_re")]
    public partial class CcfmessagesRe
    {
        [Key]
        public string id { get; set; }
        public string ucode { get; set; }
        public string cid { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int? mstatus { get; set; }
        public DateTime startdate { get; set; }
        public string cname { get; set; }
        public string lamount { get; set; }
        public string date { get; set; }
        public string phone { get; set; }
        public string imgurl { get; set; }
        public string postion { get; set; }

    }

    public class ReturnMessage
    {
        public int totalMessage { get; set; }
        public int totalRead { get; set; }
        public int totalUnread { get; set; }
        public List<CcfmessagesRe> listMessages { get; set; }
    }
}
