using System.Text.Json.Serialization;

namespace ANF.Core.Commons
{
    public class IpInfor
    {
        public string Ip { get; set; } = null!;
        [JsonPropertyName("country")]
        public string Country { get; set; } = null!;
        [JsonPropertyName("isp")]
        public string Carrier { get; set; } = null!;
        [JsonPropertyName("proxy")]
        public bool? Proxy { get; set; }
    }
}
