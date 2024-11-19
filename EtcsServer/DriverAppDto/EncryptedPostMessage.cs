using System.Text.Json.Serialization;

namespace EtcsServer.DriverAppDto
{
    public class EncryptedPostMessage(string content)
    {
        [JsonPropertyName("source")]
        public string Source { get; set; } = "SERVER";
        public string Content { get; set; } = content;
    }
}
