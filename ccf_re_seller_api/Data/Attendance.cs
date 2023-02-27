using System;
namespace ccf_re_seller_api.Data
{
    public class Attendance
    {
        public Attendance()
        {
            
    }
        public string EID { get; set; }
        public string Dates { get; set; }
        public string ClockIn { get; set; }
        public string ClockOut { get; set; }
        public string Dept { get; set; }
    }
}

