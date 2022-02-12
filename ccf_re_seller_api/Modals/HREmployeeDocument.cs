using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Modals;

namespace ccf_re_seller_api.Models
{
    [Table("ccfedoc")]
    public class HREmployeeDocument
    {
        [Key]
        public string docid { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public string doctype { get; set; }
        public string docnum { get; set; }
        public DateTime edate { get; set; }
        public byte[] docatt { get; set; }
        public string rmark { get; set; }
        public virtual HRTimeLogClass ccfpinfo { get; set; }

    }

    public class HRValidateEmployeeDocument
    {
        [Key]
        public string docid { get; set; }

        public string eid { get; set; }

        public string doctype { get; set; }

        public string docnum { get; set; }

        public DateTime edate { get; set; }

        //[Required]
        public byte[] docatt { get; set; }

        public string rmark { get; set; }

    }

    public class ReturnDocumentByEmployee
    {
        public string docid { get; set; }
        public string doctype { get; set; }
        public string eid { get; set; }
        public string docnum { get; set; }
        public string filepath { get; set; }
    }
}
