using EtcsServer.Controllers;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
using EtcsServer.Senders;
using EtcsServer.Senders.Contracts;
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

        public IDriverAppSender DriverAppSender { get; set; }

        public TestRequestSender(IServiceProvider serviceProvider)
        {
            this.driverAppController = serviceProvider.GetRequiredService<DriverAppController>();
            this.unityAppController = serviceProvider.GetRequiredService<UnityAppController>();
            this.securityManager = serviceProvider.GetRequiredService<ISecurityManager>();
            this.movementAuthorityValidator = serviceProvider.GetRequiredService<IMovementAuthorityValidator>();
            this.movementAuthorityProvider = serviceProvider.GetRequiredService<IMovementAuthorityProvider>();
            this.movementAuthorityTracker = serviceProvider.GetRequiredService<IMovementAuthorityTracker>();

            DriverAppSender = A.Fake<IDriverAppSender>();
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
                movementAuthorityValidator,
                movementAuthorityProvider,
                DriverAppSender
            ).Result;

            Assert.True(response is OkResult);
        }

        public void ImitateReceivingCrossingStateFromUnity(int crossingId, bool isFunctional)
        {
            ActionResult response = unityAppController.ChangeCrossingState(
                crossingId, isFunctional,
                movementAuthorityValidator,
                movementAuthorityProvider,
                DriverAppSender
            ).Result;

            Assert.True(response is OkResult);
        }

        public void ImitateReceivingRailwaySignalStateFromUnity(int railwaySignalId, bool isGoMessage)
        {
            ActionResult response = unityAppController.ChangeRailwaySignalState(
                new SignalStateChange() { SemaphoreId = railwaySignalId, Go = isGoMessage },
                movementAuthorityValidator,
                movementAuthorityProvider,
                DriverAppSender
            ).Result;

            Assert.True(response is OkResult);
        }
    }
}
