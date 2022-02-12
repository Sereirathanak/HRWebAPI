using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfleaverequestdocument")]
    public class HRLeaveRequestDocument
    {
        [Key]
        public string leavereqdocid { get; set; }
        public string leaverequestid { get; set; }
        public string eid { get; set; }
        public byte[] file { get; set; }
    }
}
