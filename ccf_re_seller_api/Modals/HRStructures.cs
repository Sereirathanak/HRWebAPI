using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfstructures")]
    public class HRStructures
    {
        [Key]
        public string id { get; set; }
        public string title { get; set; }
        public bool expaned { get; set; }
        public string children { get; set; }
        public string iddepartment { get; set; }
        public string idemployee { get; set; }
        public string level { get; set; }

    }

    public class ReturnHRStructures    {
        public string id { get; set; }
        public string title { get; set; }
        public bool expaned { get; set; }
        public string children { get; set; }
        public string iddepartment { get; set; }
        public string idemployee { get; set; }
        public string level { get; set; }
        public List<HRStructures> listStructures { get; set; }
    }

    public class ListHRStructures
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool expaned { get; set; }
        public string children { get; set; }
        public string iddepartment { get; set; }
        public string idemployee { get; set; }
        public string level { get; set; }
    }
}
