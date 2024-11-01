namespace EtcsServer.DecisionMakers.Contract
{
    public interface IMovementAuthorityValidator
    {
        MovementAuthorityValidationOutcome IsTrainValidForMovementAuthority(string trainId);
    }
}
