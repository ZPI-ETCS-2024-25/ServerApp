using EtcsServer.InMemoryData;

namespace EtcsServer.ExtensionMethods
{
    public static class TrackEndExtensions
    {
        public static TrackEnd GetOppositeEnd(this TrackEnd trackEnd)
        {
            return trackEnd == TrackEnd.RIGHT ? TrackEnd.LEFT : TrackEnd.RIGHT;
        }
    }
}
