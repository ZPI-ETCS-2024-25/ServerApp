using EtcsServer.Controllers;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
using EtcsServer.Senders;
using EtcsServer.Senders.Contracts;
using EtcsServer.UnityDto;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static EtcsServer.Controllers.UnityAppController;

namespace EtcsServerTests.Helpers
{
    public partial class TestRequestSender
    {
        private DriverAppController driverAppController;
        private UnityAppController unityAppController;

        private ISecurityManager securityManager;
        private IMovementAuthorityValidator movementAuthorityValidator;
        private IMovementAuthorityProvider movementAuthorityProvider;
        private IMovementAuthorityTracker movementAuthorityTracker;
        private readonly IServiceProvider serviceProvider;

        public TestRequestSender(IServiceProvider serviceProvider)
        {
            this.driverAppController = serviceProvider.GetRequiredService<DriverAppController>();
            this.unityAppController = serviceProvider.GetRequiredService<UnityAppController>();
            this.securityManager = serviceProvider.GetRequiredService<ISecurityManager>();
            this.movementAuthorityValidator = serviceProvider.GetRequiredService<IMovementAuthorityValidator>();
            this.movementAuthorityProvider = serviceProvider.GetRequiredService<IMovementAuthorityProvider>();
            this.movementAuthorityTracker = serviceProvider.GetRequiredService<IMovementAuthorityTracker>();
            this.serviceProvider = serviceProvider;
        }
        
        public MovementAuthority? PostValidMovementAuthorityRequest(MovementAuthorityRequest request)
        {
            ActionResult response = driverAppController.PostMovementAuthorityRequest(
                new EncryptedMessage() { Content = securityManager.Encrypt(request) },
                movementAuthorityValidator,
                movementAuthorityProvider,
                movementAuthorityTracker
            ).Result;

            if (response is ObjectResult objectResult && objectResult.Value is string serializedResponse)
            {
                if (serializedResponse != null)
                {
                    EncryptedMessage encryptedMessage = JsonSerializer.Deserialize<EncryptedMessage>(serializedResponse);
                    return securityManager.Decrypt<MovementAuthority>(encryptedMessage.Content);
                }

                return null;
            }
            return null;
        }

        public void ImitateReceivingSwitchStateFromUnity(int switchId, bool isGoingStraight)
        {
            ActionResult response = unityAppController.ChangeSwitchState(
                new JunctionStateChange() { JunctionId = switchId, Straight = isGoingStraight },
                serviceProvider.GetRequiredService<ISwitchStates>(),
                serviceProvider.GetRequiredService<ISwitchDirectionStates>()
            ).Result;

            Assert.True(response is OkResult);
        }

        public void ImitateReceivingCrossingStateFromUnity(int crossingId, bool isFunctional)
        {
            ActionResult response = unityAppController.ChangeCrossingState(
                crossingId, isFunctional,
                serviceProvider.GetRequiredService<ICrossingStates>()
            ).Result;

            Assert.True(response is OkResult);
        }

        public void ImitateReceivingRailwaySignalStateFromUnity(int railwaySignalId, bool isGoMessage)
        {
            ActionResult response = unityAppController.ChangeRailwaySignalState(
                new SignalStateChange() { SemaphoreId = railwaySignalId, Go = isGoMessage },
                serviceProvider.GetRequiredService<IRailwaySignalStates>()
            ).Result;

            Assert.True(response is OkResult);
        }
    }
}
