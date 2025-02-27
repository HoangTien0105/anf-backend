using System.Text.Json.Serialization;

namespace ANF.Core.Models.Requests
{
    public class PaginationRequest
    {
        const int maxPageSize = 100;

        //TODO: Fix the display property, using this to follow camelCase in query parameter
        public int pageNumber { get; set; } = 1;

        private int _pageSize = 10;

        //TODO: Fix the display property, using this to follow camelCase in query parameter
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
