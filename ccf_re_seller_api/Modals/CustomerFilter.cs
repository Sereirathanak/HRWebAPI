using System;
namespace ccf_re_seller_api.Modals
{
    public class CustomerFilter
    {
        
        public string? uid { get; set; }
        public string? sdate { get; set; }
        public string? edate { get; set; }
        public string? status { get; set; }
        public string? code { get; set; }
        public string? aby { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string dateOf { get; set; }
        public string? isDownload { get; set; }
    }
}
