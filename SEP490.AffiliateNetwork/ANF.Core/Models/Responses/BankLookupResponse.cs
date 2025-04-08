using Newtonsoft.Json;

namespace ANF.Core.Models.Responses
{
    public class BankLookupResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public BankLookupData? Data { get; set; }

        [JsonProperty("msg")]
        public string? Message { get; set; }
    }

    public class BankLookupData
    {
        [JsonProperty("ownerName")]
        public string? OwnerName { get; set; }
    }
}
