namespace EtcsServer.DriverAppDto
{
    public class RegisterTrainResponse : ServerResponse
    {
        public string MessageType { get; set; } = "RE";
        public bool RegisterSuccess { get; set; } = true;

    }
}
