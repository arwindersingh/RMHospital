using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HospitalAllocation.Providers.Allocation.Interface.Positions;
using HospitalAllocation.Providers.Allocation.Interface;
using HospitalAllocation.Data.Allocation.Positions;
using HospitalAllocation.Data.Allocation.StaffGroups;

namespace HospitalAllocation.Tests
{
    /// <summary>
    /// Stores the equal methods for IPositionStore, IPodStore, ISeniorStore, IBedSet, ISeniorListStore
    /// </summary>
    public static class EqualMethods
    {
        /// <summary>
        /// Compare two instances of IPositionStore, if they have same staff member and ShiftType, then return true
        /// </summary>
        /// <param name="positionX">Position Store.</param>
        /// <param name="positionY">Position Store.</param>
        /// <value>Is Equal.</value>
        public static bool PositionStoreEqual(IPositionStore positionX, IPositionStore positionY)
        {
            if (positionX == null && positionY == null)
            {
                return true;
            }
            if (positionX?.StaffMember == positionY?.StaffMember && positionX?.ShiftType == positionY?.ShiftType)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compare two positions, if they have same staff member and ShiftType, then return true
        /// </summary>
        /// <param name="positionX">Position.</param>
        /// <param name="positionY">Position.</param>
        /// <value>Is Equal.</value>
        public static bool PositionEqual(Position positionX, Position positionY)
        {
            if (positionX == null && positionY == null)
            {
                return true;
            }
            if (positionX?.StaffMember == positionY?.StaffMember && positionX?.ShiftType == positionY?.ShiftType)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compare two instances of ISeniorListStore, if they have same field, then return true
        /// </summary>
        /// <param name="seniorListX">Senior List Store.</param>
        /// <param name="seniorListY">Senior List Store.</param>
        /// <value>Is Equal.</value>
        public static bool SeniorListEqual(ISeniorListStore seniorListX, ISeniorListStore seniorListY)
        {

            if (seniorListX.Array == null && seniorListY.Array == null)
            {
                return true;
            }
            else if (seniorListX.Array == null || seniorListY.Array == null)
            {
                return false;
            }
            else if (seniorListX.Array != null && seniorListY.Array != null)
            {
                if (seniorListX.Array.Length != seniorListY.Array.Length)
                {
                    return false;
                }

                for (int i = 0; i < seniorListX.Array.Length; i++)
                {
                    if (!PositionEqual(seniorListX.Array[i], seniorListY.Array[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Compare two instances of IBedSetStore, if they have same fields, then return true
        /// </summary>
        /// <param name="bedSetX">Bed Set Store.</param>
        /// <param name="bedSetY">Bed Set Store.</param>
        /// <value>Is Equal.</value>
        public static bool BedSetStoreEqual(IBedSetStore bedSetX, IBedSetStore bedSetY)
        {
            if (bedSetX == null && bedSetY == null)
            {
                return true;
            }
            else if (bedSetX == null || bedSetY == null)
            {
                return false;
            }
            else if (bedSetX != null && bedSetY != null)
            {
                foreach (KeyValuePair<int, Position> bed in bedSetX.Beds)
                {
                    if (!PositionEqual(bedSetY[bed.Key].AsPosition, bed.Value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Compare two instances of IPodStore, if they have same fields, then return true
        /// </summary>
        /// <param name="podX">Pod Store.</param>
        /// <param name="podY">Pod Store.</param>
        /// <value>Is Equal.</value>
        public static bool PodStoreEqual(IPodStore podX, IPodStore podY)
        {
            return PositionStoreEqual(podX.TeamLeader, podY.TeamLeader) &&
                PositionStoreEqual(podX.Consultant, podY.Consultant) &&
                PositionStoreEqual(podX.Registrar, podY.Registrar) &&
                PositionStoreEqual(podX.Resident, podY.Resident) &&
                PositionStoreEqual(podX.PodCa, podY.PodCa) &&
                PositionStoreEqual(podX.CaCleaner, podY.CaCleaner) &&
                BedSetStoreEqual(podX.BedSet, podY.BedSet);
        }

        /// <summary>
        /// Compare two instances of ISeniorTeamStore, if they have same fields, then return true
        /// </summary>
        /// <param name="seniorTeamX">Senior store.</param>
        /// <param name="seniorTeamY">Senior store.</param>
        /// <value>Is Equal.</value>
        public static bool SeniorTeamStoreEqual(ISeniorTeamStore seniorTeamX, ISeniorTeamStore seniorTeamY)
        {
            return PositionStoreEqual(seniorTeamX.AccessCoordinator, seniorTeamY.AccessCoordinator) &&
                PositionStoreEqual(seniorTeamX.Tech, seniorTeamY.Tech) &&
                PositionStoreEqual(seniorTeamX.Mern, seniorTeamY.Mern) &&
                PositionStoreEqual(seniorTeamX.CaSupport, seniorTeamY.CaSupport) &&
                PositionStoreEqual(seniorTeamX.WardOnCallConsultant, seniorTeamY.WardOnCallConsultant) &&
                PositionStoreEqual(seniorTeamX.TransportRegistrar, seniorTeamY.TransportRegistrar) &&
                PositionStoreEqual(seniorTeamX.DonationCoordinator, seniorTeamY.DonationCoordinator) &&
                SeniorListEqual(seniorTeamX.Cnm, seniorTeamY.Cnm) &&
                SeniorListEqual(seniorTeamX.Cnc, seniorTeamY.Cnc) &&
                SeniorListEqual(seniorTeamX.Resource, seniorTeamY.Resource) &&
                SeniorListEqual(seniorTeamX.InternalRegistrar, seniorTeamY.InternalRegistrar) &&
                SeniorListEqual(seniorTeamX.ExternalRegistrar, seniorTeamY.ExternalRegistrar) &&
                SeniorListEqual(seniorTeamX.Educator, seniorTeamY.Educator);
        }
    }
}
