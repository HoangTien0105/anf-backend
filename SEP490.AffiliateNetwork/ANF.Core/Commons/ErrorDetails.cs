using System.Text.Json;

namespace ANF.Core.Commons
{
    /// <summary>
    /// Represents the details of an error including status code, message, and optional details.
    /// </summary>
    public class ErrorDetail
    {
        public bool IsSuccess { get; set; } = false;

        public int StatusCode { get; set; }

        public string Message { get; set; } = null!;

        public string? Details { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
