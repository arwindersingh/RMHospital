using System;
using Xunit;
using Moq;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HospitalAllocation.Controllers;
using HospitalAllocation.Providers.Allocation.Interface.Positions;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Providers.Allocation;
using HospitalAllocation.Messages.Requests.Allocation;
using HospitalAllocation.Messages.Responses;
using HospitalAllocation.Messages.Responses.Allocation;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Tests.Mocks;
using HospitalAllocation.Tests;

namespace HospitalAllocation.Tests.Controllers
{
    /// <summary>
    /// Test suite for <see cref="T:HospitalAllocation.Controllers.TeamController"/>.
    /// </summary>
    public class TeamControllerTests
    {
        public class GetTeamListMethod
        {
            [Fact]
            public void ValidList_Success()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                // When
                var res = fooController.GetTeamList();
                // Then
                var jsonResult = Assert.IsType<JsonResult>(res);
                var teamListResponse = Assert.IsType<TeamListResponse>(jsonResult.Value);
                Assert.Equal(ResponseStatus.Success, teamListResponse.Status);
            }
        }

        public class GetTeamMethod
        {
            [Fact]
            public void SeniorTeam_Success()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                // When
                var seniorRes = fooController.GetTeam(TeamType.Senior);
                // Then
                var jsonSeniorRes = Assert.IsType<JsonResult>(seniorRes);
                var seniorSummaryResponse = Assert.IsType<TeamSummaryResponse>(jsonSeniorRes.Value);
                Assert.Equal(ResponseStatus.Success, seniorSummaryResponse.Status);
            }

            [Fact]
            public void Pod_Success()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                // When
                var podRes = fooController.GetTeam(TeamType.A);
                // Then
                var jsonPodRes = Assert.IsType<JsonResult>(podRes);
                var podSummaryResponse = Assert.IsType<TeamSummaryResponse>(jsonPodRes.Value);
                Assert.Equal(ResponseStatus.Success, podSummaryResponse.Status);
            }

            [Fact]
            private void NullTeam_BadRequest()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                // When
                var nullRes = fooController.GetTeam(null);
                // Then
                Assert.IsType<BadRequestObjectResult>(nullRes);
            }
        }

        public class AddAllocationToSeniorTeamMethod
        {
            [Fact]
            private void ValidAllocation_Ok()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                var fooNewSeniorAllocation = new SeniorAllocationRequest(MockSeniorTeam.GetMutantSeniorTeam().AsSeniorTeam);
                // When
                var seniorRes = fooController.AddAllocationToSeniorTeam(fooNewSeniorAllocation);
                // Then
                Assert.IsType<OkObjectResult>(seniorRes);
            }

            [Fact]
            private void NullAllocation_BadRequest()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                // When
                var nullRes = fooController.AddAllocationToSeniorTeam(null);
                // Then
                Assert.IsType<BadRequestObjectResult>(nullRes);
            }
        }

        public class AddAllocationToPodMethod
        {
            [Fact]
            private void NullAllocationToValidPod_BadRequest()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                // When
                var nullAllocRes = fooController.AddAllocationToPod(TeamType.A, null);
                // Then
                Assert.IsType<BadRequestObjectResult>(nullAllocRes);
            }

            [Fact]
            private void ValidAllocationToNullPod_BadRequest()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                var fooNewAllocation = new PodAllocationRequest(MockPodD.GetTestPodD().AsPod);
                // When
                var nullTypeRes = fooController.AddAllocationToPod(null, fooNewAllocation);
                // Then
                Assert.IsType<BadRequestObjectResult>(nullTypeRes);
            }

            [Fact]
            private void ValidAllocationToValidPod_Success()
            {
                // Given
                var fooAllocation = MockAllocation.GetTestAllocation();
                var fooProvider = new AllocationProvider(fooAllocation);
                var fooController = new TeamController(fooProvider);
                var fooNewAllocation = new PodAllocationRequest(MockPodD.GetTestPodD().AsPod);
                // When
                var successRes = fooController.AddAllocationToPod(TeamType.A, fooNewAllocation);
                // Then
                Assert.IsType<JsonResult>(successRes);
            }
        }
    }
}
