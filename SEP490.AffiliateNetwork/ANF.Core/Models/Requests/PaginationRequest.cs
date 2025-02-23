using System.Text.Json.Serialization;

namespace ANF.Core.Models.Requests
{
    public class PaginationRequest
    {
        const int maxPageSize = 100;

        public int pageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
