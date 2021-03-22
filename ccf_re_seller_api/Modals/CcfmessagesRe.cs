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
        public string bcode { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int? mstatus { get; set; }
        public string startdate { get; set; }
        public string res { get; set; }
        public string des { get; set; }
        public string date { get; set; }
        public string imgurl { get; set; }
    }
}
