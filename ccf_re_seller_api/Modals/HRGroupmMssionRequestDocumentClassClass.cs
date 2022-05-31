using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfgroupmissionrequestdocument")]
    public class HRGroupmMssionRequestDocumentClassClass
    {
        [Key]
        public string groupmisreqdocid { get; set; }
        public string groupmissionrequestid { get; set; }
        public byte[] file { get; set; }
    }
}
