using System;
using Xunit;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
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
    /// Methods to get a mock senior team.
    /// </summary>
    public static class MockSeniorTeam
    {
        /// <summary>
        /// Gets a mock Senior Team Store with a bunch of mock interfaces
        /// </summary>
        /// <value>A mock Senior Team Store</value>
        public static ISeniorTeamStore GetTestSeniorTeam()
        {
            var mockSenior = new Mock<ISeniorTeamStore>();
            // Positions
            mockSenior.Setup(foo => foo.AccessCoordinator).Returns(GetTestACPosition());
            mockSenior.Setup(foo => foo.Tech).Returns(GetTestTechPosition());
            mockSenior.Setup(foo => foo.Mern).Returns(GetTestMernPosition());
            mockSenior.Setup(foo => foo.CaSupport).Returns(GetTestCaSupportPosition());
            mockSenior.Setup(foo => foo.WardOnCallConsultant).Returns(GetTestWardOnCallConsultantSupportPosition());
            mockSenior.Setup(foo => foo.TransportRegistrar).Returns(GetTestTransportRegistrarPosition());
            mockSenior.Setup(foo => foo.DonationCoordinator).Returns(GetTestDonationCoordinatorSupportPosition());

            // Lists
            mockSenior.Setup(foo => foo.Cnm).Returns(GetTestCnmList());
            mockSenior.Setup(foo => foo.Cnc).Returns(GetTestCncList());
            mockSenior.Setup(foo => foo.Resource).Returns(GetTestResourceList());
            mockSenior.Setup(foo => foo.InternalRegistrar).Returns(GetTestInternalRegistrarList());
            mockSenior.Setup(foo => foo.ExternalRegistrar).Returns(GetTestExternalRegistrarList());
            mockSenior.Setup(foo => foo.Educator).Returns(GetTestEducatorList());
            mockSenior.Setup(foo => foo.AsSeniorTeam).Returns(
                new SeniorTeam(
                    GetTestACPosition().AsPosition,
                    GetTestTechPosition().AsPosition,
                    GetTestMernPosition().AsPosition,
                    GetTestCaSupportPosition().AsPosition,
                    GetTestWardOnCallConsultantSupportPosition().AsPosition,
                    GetTestTransportRegistrarPosition().AsPosition,
                    GetTestDonationCoordinatorSupportPosition().AsPosition,
                    GetTestCnmList().Array,
                    GetTestCncList().Array,
                    GetTestResourceList().Array,
                    GetTestInternalRegistrarList().Array,
                    GetTestExternalRegistrarList().Array,
                    GetTestEducatorList().Array));
            var obj = mockSenior.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Senior Team Store with a mutant from the test pod A
        /// </summary>
        /// <value>A mock Senior Team Store</value>
        public static ISeniorTeamStore GetMutantSeniorTeam()
        {
            var mockSenior = new Mock<ISeniorTeamStore>();
            // Positions
            mockSenior.Setup(foo => foo.AccessCoordinator).Returns(GetMutantPosition()); // Mutant
            mockSenior.Setup(foo => foo.Tech).Returns(GetTestTechPosition());
            mockSenior.Setup(foo => foo.Mern).Returns(GetTestMernPosition());
            mockSenior.Setup(foo => foo.CaSupport).Returns(GetTestCaSupportPosition());
            mockSenior.Setup(foo => foo.WardOnCallConsultant).Returns(GetTestWardOnCallConsultantSupportPosition());
            mockSenior.Setup(foo => foo.TransportRegistrar).Returns(GetTestTransportRegistrarPosition());
            mockSenior.Setup(foo => foo.DonationCoordinator).Returns(GetTestDonationCoordinatorSupportPosition());

            // Lists
            mockSenior.Setup(foo => foo.Cnm).Returns(GetTestCnmList());
            mockSenior.Setup(foo => foo.Cnc).Returns(GetTestCncList());
            mockSenior.Setup(foo => foo.Resource).Returns(GetTestResourceList());
            mockSenior.Setup(foo => foo.InternalRegistrar).Returns(GetTestInternalRegistrarList());
            mockSenior.Setup(foo => foo.ExternalRegistrar).Returns(GetTestExternalRegistrarList());
            mockSenior.Setup(foo => foo.Educator).Returns(GetTestEducatorList());
            mockSenior.Setup(foo => foo.AsSeniorTeam).Returns(
                new SeniorTeam(
                    GetMutantPosition().AsPosition, // Mutant
                    GetTestTechPosition().AsPosition,
                    GetTestMernPosition().AsPosition,
                    GetTestCaSupportPosition().AsPosition,
                    GetTestWardOnCallConsultantSupportPosition().AsPosition,
                    GetTestTransportRegistrarPosition().AsPosition,
                    GetTestDonationCoordinatorSupportPosition().AsPosition,
                    GetTestCnmList().Array,
                    GetTestCncList().Array,
                    GetTestResourceList().Array,
                    GetTestInternalRegistrarList().Array,
                    GetTestExternalRegistrarList().Array,
                    GetTestEducatorList().Array));
            var obj = mockSenior.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Position Store as a mutant
        /// </summary>
        /// <value>A mock Position Store</value>
        public static IPositionStore GetMutantPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.TwelveHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "MUTATION");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Senior List Store for test
        /// </summary>
        /// <value>A mock Senior List Store</value>
        private static ISeniorListStore GetTestSeniorList()
        {
            var mockSeniorList = new Mock<ISeniorListStore>();
            mockSeniorList.Setup(foo => foo.Array).Returns(new Position[0]);
            var obj = mockSeniorList.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock AC Position Store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestACPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST AC");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }


        /// <summary>
        /// Gets a mock Tech Position Store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestTechPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST Tech");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Mern Position Store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestMernPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST Mern");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock CaSupport Position Store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestCaSupportPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST CaSupport");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock WardOnCallConsultantSupport Position Store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestWardOnCallConsultantSupportPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST WardOnCallConsultant");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock TransportRegistrar Position Store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestTransportRegistrarPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST TransportRegistrar");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock TestDonationCoordinatorSupport Position Store for test
        /// </summary>
        /// <value>A mock position Store</value>
        private static IPositionStore GetTestDonationCoordinatorSupportPosition()
        {
            var mockPosition = new Mock<IPositionStore>();
            mockPosition.SetupProperty(foo => foo.ShiftType, ShiftType.EightHour);
            mockPosition.SetupProperty(foo => foo.StaffMember, "TEST DonationCoordinator");
            mockPosition.SetupGet(foo => foo.AsPosition).Returns(() => new Position(mockPosition.Object.StaffMember, mockPosition.Object.ShiftType));
            var obj = mockPosition.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Cnm Senior List Store for test
        /// </summary>
        /// <value>A mock Senior List Store</value>
        private static ISeniorListStore GetTestCnmList()
        {
            var mockSeniorList = new Mock<ISeniorListStore>();
            var cnm = new Position[2];
            cnm[0] = new Position("Test Cnm1", ShiftType.EightHour);
            cnm[1] = new Position("Test Cnm2", ShiftType.EightHour);
            mockSeniorList.SetupProperty(foo => foo.Array, cnm);
            var obj = mockSeniorList.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Cnc Senior List Store for test
        /// </summary>
        /// <value>A mock Senior List Store</value>
        private static ISeniorListStore GetTestCncList()
        {
            var mockSeniorList = new Mock<ISeniorListStore>();
            var cnc = new Position[2];
            cnc[0] = new Position("Test Cnc1", ShiftType.EightHour);
            cnc[1] = new Position("Test Cnc2", ShiftType.EightHour);
            mockSeniorList.SetupProperty(foo => foo.Array, cnc);
            var obj = mockSeniorList.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Resource List Store for test
        /// </summary>
        /// <value>A mock Senior List Store</value>
        private static ISeniorListStore GetTestResourceList()
        {
            var mockSeniorList = new Mock<ISeniorListStore>();
            var resource = new Position[2];
            resource[0] = new Position("Test Resource1", ShiftType.EightHour);
            resource[1] = new Position("Test Resource2", ShiftType.EightHour);
            mockSeniorList.SetupProperty(foo => foo.Array, resource);
            var obj = mockSeniorList.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock InternalRegistrar Senior List Store for test
        /// </summary>
        /// <value>A mock Senior List Store</value>
        private static ISeniorListStore GetTestInternalRegistrarList()
        {
            var mockSeniorList = new Mock<ISeniorListStore>();
            var internalRegistrar = new Position[2];
            internalRegistrar[0] = new Position("Test InternalRegistrar1", ShiftType.EightHour);
            internalRegistrar[1] = new Position("Test InternalRegistrar2", ShiftType.EightHour);
            mockSeniorList.SetupProperty(foo => foo.Array, internalRegistrar);
            var obj = mockSeniorList.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock ExternalRegistrar Senior List Store for test
        /// </summary>
        /// <value>A mock Senior List Store</value>
        private static ISeniorListStore GetTestExternalRegistrarList()
        {
            var mockSeniorList = new Mock<ISeniorListStore>();
            var externalRegistrar = new Position[2];
            externalRegistrar[0] = new Position("Test ExternalRegistrar1", ShiftType.EightHour);
            externalRegistrar[1] = new Position("Test ExternalRegistrar2", ShiftType.EightHour);
            mockSeniorList.SetupProperty(foo => foo.Array, externalRegistrar);
            var obj = mockSeniorList.Object;
            return obj;
        }

        /// <summary>
        /// Gets a mock Educator Senior List Store for test
        /// </summary>
        /// <value>A mock Senior List Store</value>
        private static ISeniorListStore GetTestEducatorList()
        {
            var mockSeniorList = new Mock<ISeniorListStore>();
            var educator = new Position[2];
            educator[0] = new Position("Test Educator1", ShiftType.EightHour);
            educator[1] = new Position("Test Educator2", ShiftType.EightHour);
            mockSeniorList.SetupProperty(foo => foo.Array, educator);
            var obj = mockSeniorList.Object;
            return obj;
        }
    }
}
