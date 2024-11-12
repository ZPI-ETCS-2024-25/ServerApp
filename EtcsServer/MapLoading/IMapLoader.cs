using EtcsServer.Database;
using EtcsServer.Database.Entity;

namespace EtcsServer.MapLoading
{
    public interface IMapLoader
    {
        void LoadMapIntoDatabase(EtcsDbContext context, ITrackageMap map);
    }
}
