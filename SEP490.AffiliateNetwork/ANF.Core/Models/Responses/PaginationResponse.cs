namespace ANF.Core.Models.Responses
{
    public class PaginationResponse<T>
    {
        public int PageNumber { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalPages { get; set; }
        
        public int TotalRecords { get; set; }
        
        public List<T> Data { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public PaginationResponse(List<T> data, int count, int pageNumber, int pageSize)
        {
            Data = data;
            TotalRecords = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            // Calculate navigation properties
            HasNextPage = pageNumber < TotalPages;
            HasPreviousPage = pageNumber > 1;
        }
    }
}
