using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Models
{
    [Table("edoc")]
    public class HREmployeeDocument
    {
        [Key]
        public string docid { get; set; }
        [ForeignKey("pinfo")]
        public string eid { get; set; }
        public string doctype { get; set; }
        public string docnum { get; set; }
        public DateTime edate { get; set; }
        public byte[] docatt { get; set; }
        public string rmark { get; set; }
        public virtual HREmployee pinfo { get; set; }

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
