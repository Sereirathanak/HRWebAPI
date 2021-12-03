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
    }
}
