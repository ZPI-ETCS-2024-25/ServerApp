namespace EtcsServer.DriverAppDto
{
    public class EncryptedResponse(string content) : ServerResponse
    {
        public string EncryptedContent { get; set; } = content;
    }
}
