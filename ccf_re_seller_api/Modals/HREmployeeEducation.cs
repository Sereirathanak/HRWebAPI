﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ccf_re_seller_api.Models
{
    public class HREmployeeEducation
    {
        [Key]
        public string eduid { get; set; }
        [ForeignKey("pinfo")]
        public string eid { get; set; }
        public string inst { get; set; }
        public string sub { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public string certyfi { get; set; }
        public string remark { get; set; }

    }
}