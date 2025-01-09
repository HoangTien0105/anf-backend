namespace ANF.Core.Commons
{
    /// <summary>
    /// Represents a response to the client for Create, Update, Delete, or Get operations.
    /// </summary>
    /// <typeparam name="T">The type of the value being returned.</typeparam>
    public record ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets the status code of the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message of the response.
        /// </summary>
        public string Message { get; set; } = null!;

        /// <summary>
        /// Gets or sets the value of the response.
        /// </summary>
        public T? Value { get; set; }
    }
}
