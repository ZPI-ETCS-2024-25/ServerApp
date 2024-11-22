namespace EtcsServer.Database.Entity
{
    public class SwitchDirection
    {
        public int SwitchDirectionId { get; set; }
        public int TrackFromId { get; set; }
        public int TrackToIdGoingStraight { get; set; }
        public int TrackToIdTurning { get; set; }
    }
}
