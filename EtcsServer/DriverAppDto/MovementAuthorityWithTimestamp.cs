namespace EtcsServer.DriverAppDto
{
    public class MovementAuthorityWithTimestamp : MovementAuthority
    {

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public MovementAuthorityWithTimestamp(MovementAuthority movementAuthority)
        {
            this.Speeds = movementAuthority.Speeds;
            this.SpeedDistances = movementAuthority.SpeedDistances;
            this.Gradients = movementAuthority.Gradients;
            this.GradientsDistances = movementAuthority.GradientsDistances;
            this.Lines = movementAuthority.Lines;
            this.LinesDistances = movementAuthority.LinesDistances;
            this.Messages = movementAuthority.Messages;
            this.MessagesDistances = movementAuthority.MessagesDistances;
            this.ServerPosition = movementAuthority.ServerPosition;
        }
    }
}
