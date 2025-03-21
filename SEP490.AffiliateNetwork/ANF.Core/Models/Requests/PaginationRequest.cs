using Microsoft.AspNetCore.Mvc;

namespace ANF.Core.Models.Requests
{
    public class PaginationRequest
    {
        const int maxPageSize = 100;

        //[BindProperty("pageNumber")]
        public int pageNumber { get; set; } = 1;

        private int _pageSize = 10;

        //[BindProperty("pageSize")]
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
