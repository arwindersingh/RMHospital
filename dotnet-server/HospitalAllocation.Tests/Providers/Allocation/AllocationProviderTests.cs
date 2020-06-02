using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Providers.Allocation;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Providers.Allocation.Interface.Positions;
using HospitalAllocation.Tests.Mocks;
using HospitalAllocation.Tests;

namespace HospitalAllocation.Tests.Providers.Allocation
{
    /// <summary>
    /// Test Suite for <see cref="T:HospitalAllocation.Providers.Allocation.AllocationProvider"/>.
    /// </summary>
    public class AllocationProviderTests
    {
        public class SetSeniorTeamAllocationMethods
        {
            [Fact]
            public void MutantAllocationOverwritesOriginal_ResultEqualsMutant()
            {
                // Given
                var mutation = MockSeniorTeam.GetMutantSeniorTeam();
                var fooAllocation = MockAllocation.GetTestAllocation();
                // When
                AllocationProvider.SetSeniorTeamAllocation(fooAllocation.SeniorTeam, mutation.AsSeniorTeam);
                // Then
                Assert.True(EqualMethods.SeniorTeamStoreEqual(mutation, fooAllocation.SeniorTeam));
            }
        }

        public class SetPodAllocationMethods
        {
            [Fact]
            public void MutantAllocationOverwritesOriginal_ResultEqualsMutant()
            {
                // Given
                var mutation = MockPodA.GetMutantPodA();
                var fooAllocation = MockAllocation.GetTestAllocation();
                // When 
                AllocationProvider.SetPodAllocation(fooAllocation.PodA, mutation.AsPod);
                // Then
                Assert.True(EqualMethods.PodStoreEqual(mutation, fooAllocation.PodA));
            }
        }
    }
}
