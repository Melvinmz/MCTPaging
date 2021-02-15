using System;
namespace MCTPaging.Model
{
    public class PagingData
    {
        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalRecords / RecordsPerPage);
        public string UrlParams { get; set; }
        public int LinksPerPage { get; set; }
    }
}
