using System;
using Xunit;
using Moq;
using System.Linq;
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
    /// Methods to get a mock Pod B
    /// </summary>
    public static class MockPodB
    {
        /// <summary>
        /// Gets a mock Pod B store with a bunch of mock interfaces
        /// </summary>
        /// <value>A mock Pod B store</value>
        public static IPodStore GetTestPodB()
        {
            var mockPod = new Mock<IPodStore>();
            mockPod.Setup(foo => foo.TeamLeader).Returns(GetTestTeamLeaderPosition());
            mockPod.Setup(foo => foo.Consultant).Returns(GetTestConsultantPosition());
            mockPod.Setup(foo => foo.Registrar).Returns(GetTestRegistrarPosition());
            mockPod.Setup(foo => foo.Resident).Returns(GetTestResidentPosition());
            mockPod.Setup(foo => foo.PodCa).Returns(GetTestPodCaPosition());
            mockPod.Setup(foo => foo.CaCleaner).Returns(GetTestCaCleanerPosition());
            mockPod.Setup(foo => foo.BedSet).Returns(GetTestBedSet());
            mockPod.Setup(foo => foo.AsPod).Returns(
                new Pod(TeamType.B,
                        GetTestBedSet().Beds,
                        GetTestConsultantPosition().AsPosition,
                        GetTestTeamLeaderPosition().AsPosition,
                        GetTestRegistrarPosition().AsPosition,
                        GetTestResidentPosition().AsPosition,
                        GetTestPodCaPosition().AsPosition,
                        GetTestCaCleanerPosition().AsPosition));
            var obj = mockPod.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock TeamLeader position store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestTeamLeaderPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST TeamLeader B");
            mockPosition.Setup(foo => foo.AsPosition).Returns(new Position("TEST TeamLeader B", ShiftType.EightHour));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Consultant position store for test
        /// </summary>
        /// <value>A mock position store</value>
        private static IPositionStore GetTestConsultantPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST Consultant B");
            mockPosition.Setup(foo => foo.AsPosition).Returns(new Position("TEST Consultant B", ShiftType.EightHour));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Registrar position store for test
        /// </summary>
        /// <value>A mock position store</value>
        private static IPositionStore GetTestRegistrarPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST Registrar B");
            mockPosition.Setup(foo => foo.AsPosition).Returns(new Position("TEST Registrar B", ShiftType.EightHour));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Resident position store for test
        /// </summary>
        /// <value>A mock position store</value>
        private static IPositionStore GetTestResidentPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST Resident B");
            mockPosition.Setup(foo => foo.AsPosition).Returns(new Position("TEST Resident B", ShiftType.EightHour));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock PodCa position store for test
        /// </summary>
        /// <value>A mock position store</value>
        private static IPositionStore GetTestPodCaPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST PodCa B");
            mockPosition.Setup(foo => foo.AsPosition).Returns(new Position("TEST PodCa B", ShiftType.EightHour));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock CaCleaner position store for test
        /// </summary>
        /// <value>A mock position store</value>
        private static IPositionStore GetTestCaCleanerPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST CaCleaner B");
            mockPosition.Setup(foo => foo.AsPosition).Returns(new Position("TEST CaCleaner B", ShiftType.EightHour));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Bed Position store for creating test bed set
        /// </summary>
        /// <value>A mock position store</value>
        private static IPositionStore GetTestBedPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST Bed B");
            mockPosition.Setup(foo => foo.AsPosition).Returns(new Position("TEST Bed B", ShiftType.EightHour));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock bed set for test
        /// </summary>
        /// <value>A mock Bed set store</value>
        private static IBedSetStore GetTestBedSet()
        {
            var mockBedSet = new Mock<IBedSetStore>();
            var _bedDict = new Dictionary<int, IPositionStore>();
            var bedCapacity = 10;

            for (int i = 1; i <= bedCapacity; i++)
            {
                _bedDict.Add(i, GetTestBedPosition());
            }
            for (int j = 1; j <= bedCapacity; j++)
            {
                mockBedSet.Setup(foo => foo[j]).Returns(_bedDict[j]);
            }
            mockBedSet.Setup(foo => foo.Beds).Returns(_bedDict.ToDictionary(kv => kv.Key, kv => kv.Value.AsPosition));
            var obj = mockBedSet.Object;
            return obj;
        }
    }
}
