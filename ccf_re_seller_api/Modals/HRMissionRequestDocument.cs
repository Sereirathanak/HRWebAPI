using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfmissionrequestdocument")]
    public class HRMissionRequestDocument
    {
        
            [Key]
            public string misreqdocid { get; set; }
            public string missionrequestid { get; set; }
            public string eid { get; set; }
            public byte[] file { get; set; }
    }
}
