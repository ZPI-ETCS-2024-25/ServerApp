namespace EtcsServer.Database.Entity
{
    public class SwitchRoute
    {
        public int SwitchRouteId {  get; set; }
        public int SwitchId { get; set; }
        public Switch Switch { get; set; }
        public int TrackFromId { get; set; }
        public Track TrackFrom { get; set; }
        public int TrackToId { get; set; }
        public Track TrackTo { get; set; }

        public int MaxSpeedMps { get; set; }
    }
}
