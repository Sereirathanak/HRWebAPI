using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfovertimerequestdocument")]
    public class HROvertimeRequestDocument
    {
        [Key]
        public string overtimereqdocid { get; set; }
        public string overtimerequestid { get; set; }
        public string eid { get; set; }
        public byte[] file { get; set; }
    }
}
