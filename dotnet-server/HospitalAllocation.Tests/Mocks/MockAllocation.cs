using System;
using Xunit;
using Moq;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HospitalAllocation.Providers.Allocation.Interface.Positions;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Providers.Allocation;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Data.Allocation;
using HospitalAllocation.Tests.Mocks;

namespace HospitalAllocation.Tests.Mocks
{
    /// <summary>
    /// Methods to get a mock implementation of IAllocationStore.
    /// </summary>
    public static class MockAllocation
    {
        /// <summary>
        /// Gets a mock allocation with a bunch of mock interfaces
        /// </summary>
        /// <value>A mock allocation store</value>
        public static IAllocationStore GetTestAllocation()
        {
            var mockAllocation = new Mock<IAllocationStore>();
            mockAllocation.Setup(foo => foo.PodA).Returns(MockPodA.GetTestPodA());
            mockAllocation.Setup(foo => foo.PodB).Returns(MockPodB.GetTestPodB());
            mockAllocation.Setup(foo => foo.PodC).Returns(MockPodC.GetTestPodC());
            mockAllocation.Setup(foo => foo.PodD).Returns(MockPodD.GetTestPodD());
            mockAllocation.Setup(foo => foo.SeniorTeam).Returns(MockSeniorTeam.GetTestSeniorTeam());
            var obj = mockAllocation.Object;
            return obj;
        }
    }
}
