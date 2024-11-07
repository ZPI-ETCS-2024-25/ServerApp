namespace EtcsServer.Database.Entity
{
    public class SwitchRoute
    {
        public int SwitchRouteId {  get; set; }
        public int SwitchId { get; set; }
        public Switch Switch { get; set; }
        public int TrackFromId { get; set; }
        public TrackageElement TrackFrom { get; set; }
        public int TrackToId { get; set; }
        public TrackageElement TrackTo { get; set; }
    }
}
