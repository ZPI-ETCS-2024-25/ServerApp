namespace EtcsServer.Helpers.Contract
{
    public interface IDriverAppSenderHelper
    {
        void SendUpdatedMaToEachImpactedTrain(List<string> impactedTrains);
    }
}
