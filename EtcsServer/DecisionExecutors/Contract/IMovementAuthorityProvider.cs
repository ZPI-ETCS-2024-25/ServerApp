using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;

namespace EtcsServer.DecisionExecutors.Contract
{
    public interface IMovementAuthorityProvider
    {
        MovementAuthority ProvideMovementAuthorityToEtcsBorder(string trainId);
        public MovementAuthority ProvideMovementAuthority(string trainId, RailwaySignal stopSignal);
    }
}
