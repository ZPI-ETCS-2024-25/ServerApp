using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DecisionMakers;
using EtcsServer.DriverAppDto;
using EtcsServer.Senders;
using static EtcsServer.DecisionMakers.Contract.MovementAuthorityValidationOutcome;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.Senders.Contracts;
using EtcsServer.UnityDto;
using Microsoft.AspNetCore.Mvc;
using EtcsServer.Helpers.Contract;

namespace EtcsServer.Helpers
{
    public class DriverAppSenderHelper : IDriverAppSenderHelper
    {
        private readonly IMovementAuthorityValidator movementAuthorityValidator;
        private readonly IMovementAuthorityProvider movementAuthorityProvider;
        private readonly IDriverAppSender driverAppSender;

        public DriverAppSenderHelper(
            [FromServices] IMovementAuthorityValidator movementAuthorityValidator,
            [FromServices] IMovementAuthorityProvider movementAuthorityProvider,
            [FromServices] IDriverAppSender driverAppSender
            )
        {
            this.movementAuthorityValidator = movementAuthorityValidator;
            this.movementAuthorityProvider = movementAuthorityProvider;
            this.driverAppSender = driverAppSender;
        }

        public void SendUpdatedMaToEachImpactedTrain(List<string> impactedTrains)
        {
            foreach (string trainId in impactedTrains)
            {
                MovementAuthorityValidationOutcome validationOutcome = movementAuthorityValidator.IsTrainValidForMovementAuthority(trainId);
                if (validationOutcome.Result == MovementAuthorityValidationResult.OK)
                {
                    MovementAuthority newMovementAuthority = validationOutcome.NextStopSignal == null ?
                        movementAuthorityProvider.ProvideMovementAuthorityToEtcsBorder(trainId) :
                        movementAuthorityProvider.ProvideMovementAuthority(trainId, validationOutcome.NextStopSignal!);

                    driverAppSender.SendNewMovementAuthority(trainId, newMovementAuthority);
                }
            }
        }
    }
}
