namespace EtcsServer.InMemoryData.Contract
{
    public class SwitchFromTo(int trackFromId, int trackToId)
    {
        public int TrackIdFrom { get; set; } = trackFromId;
        public int TrackIdTo { get; set; } = trackToId;
    }

}
