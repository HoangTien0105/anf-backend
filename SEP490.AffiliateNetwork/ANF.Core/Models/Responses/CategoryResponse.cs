namespace ANF.Core.Models.Responses
{
    public class CategoryResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
