using System;
namespace ccf_re_seller_api.Models
{
    public class HRCustomerFilter
    {
        public string? ucode { get; set; }
        public string? btlcode { get; set; }
        public string? sdate { get; set; }
        public string? edate { get; set; }
        public string? bcode { get; set; }
        public string? status { get; set; }
        public string? code { get; set; }
        public string? pcode { get; set; }
        public string? aby { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? dateOf { get; set; }
        public string? isDownload { get; set; }
        public string? timeClock { get; set; }
        public string? statusClock { get; set; }
        public string? eid { get; set; }
        public string? department { get; set; }
        public string? position { get; set; }
        public string? incharge { get; set; }
        public string? search { get; set; }


        public bool? checkBranch { get; set; }
        public bool? checkDepartment{ get; set; }
        public bool? checkPosition { get; set; }
        public bool? checkIncharge { get; set; }
        public bool? checkEcard { get; set; }
        public bool? listall { get; set; }



    }
    public class HRClockInOut
    {
        public string? ecard { get; set; }
        public string? dateClock { get; set; }
        public string? branchClock { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? timeClock { get; set; }
        public string? statusClock { get; set; }
        public string? sdate { get; set; }
        public string? edate { get; set; }
        public string? status { get; set; }
        public string? eid { get; set; }
        public int? level { get; set; }
        public string? search{ get; set; }
        public bool? listall { get; set; }


    }
}
