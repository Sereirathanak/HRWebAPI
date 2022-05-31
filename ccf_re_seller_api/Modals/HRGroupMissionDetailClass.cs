using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfgroupmissiondetaill")]
    public class HRGroupMissionDetailClass
    {
        private readonly HRContext _context;

        public HRGroupMissionDetailClass(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string gdid { get; set; }

        [ForeignKey("groupMissionRequest")]
        public string gmid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        public string com { get; set; }
        public string remark { get; set; }
        public DateTime createdate { get; set; }
        

        //public virtual ICollection<HRGroupMissionClass> groupMissionRequest { get; set; }
        public virtual HRGroupMissionClass groupMissionRequest { get; set; }

        public virtual HREmployee ccfpinfo { get; set; }

        public string employeename
        {
            get
            {
                var name = _context?.employee.SingleOrDefault(cur => cur.eid == this.eid);
                if (name != null)
                {
                    return name.dname;
                }
                return "";
            }
        }

        public string employeecard
        {
            get
            {
                var name = _context?.employee.SingleOrDefault(cur => cur.eid == this.eid);
                if (name != null)
                {
                    return name.ecard;
                }
                return "";
            }
        }

        public string employeeposition
        {
            get
            {
                var name = _context?.employeeJoinInfo.SingleOrDefault(cur => cur.eid == this.eid);
                if (name != null)
                {
                    return name.positionName;
                }
                return "";
            }
        }

        public String employeejoiningdate
        {
            get
            {
                var name = _context?.employeeJoinInfo.SingleOrDefault(cur => cur.eid == this.eid);
                if (name != null)
                {
                    return name.jdate.ToString();
                }
                return "";
            }
        }

        public string employeedepartment
        {
            get
            {
                var name = _context?.employeeJoinInfo.SingleOrDefault(cur => cur.eid == this.eid);
                if (name != null)
                {
                    return name.departmentName;
                }
                return "";
            }
        }


    }
}
