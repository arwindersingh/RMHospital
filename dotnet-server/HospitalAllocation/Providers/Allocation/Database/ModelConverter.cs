using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HospitalAllocation.Data.Allocation.StaffGroups;
using HospitalAllocation.Model;
using DataPositions = HospitalAllocation.Data.Allocation.Positions;

namespace HospitalAllocation.Providers.Allocation.Database
{
    /// <summary>
    /// Converter between the Data model and the Database model.
    /// </summary>
    public class ModelConverter
    {
        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.TeamAllocation"/>
        /// to a <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.Pod"/>.
        /// </summary>
        /// <param name="allocation">The allocation to convert.</param>
        /// <returns>The converted allocation.</returns>
        public static Pod PodFromModel(TeamAllocation allocation)
        {
            var consultant = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.Consultant);
            var teamLeader = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.TeamLeader);
            var registrar = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.Registrar);
            var resident = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.Resident);
            var podCa = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.PodCa);
            var caCleaner = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.CaCleaner);

            var beds = new Dictionary<int, DataPositions.Position>();
            foreach (BedPosition bed in allocation.Positions.OfType<BedPosition>())
            {
                beds.Add(bed.Position, PositionFromModel(bed));
            }

            return new Pod(
                TeamTypeFromModel(allocation.Type),
                beds,
                PositionFromModel(consultant),
                PositionFromModel(teamLeader),
                PositionFromModel(registrar),
                PositionFromModel(resident),
                PositionFromModel(podCa),
                PositionFromModel(caCleaner));
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.TeamAllocation"/>
        /// to a <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.SeniorTeam"/>.
        /// </summary>
        /// <param name="allocation">The allocation to convert.</param>
        /// <returns>The converted allocation.</returns>
        public static SeniorTeam SeniorTeamFromModel(TeamAllocation allocation)
        {
            var accessCoordinator = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.AccessCoordinator);
            var tech = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.Tech);
            var mern = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.Mern);
            var caSupport = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.CaSupport);
            var wardOnCallConsultant = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.WardOnCallConsultant);
            var transportRegistrar = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.TransportRegistrar);
            var donationCoordinator = allocation.Positions
                .FirstOrDefault(p => p.Type == PositionType.DonationCoordinator);

            var cnm = new List<DataPositions.Position>();
            foreach (var pos in allocation.Positions.Where(p => p.Type == PositionType.Cnm))
            {
                cnm.Add(PositionFromModel(pos));
            }

            var cnc = new List<DataPositions.Position>();
            foreach (var pos in allocation.Positions.Where(p => p.Type == PositionType.Cnc))
            {
                cnc.Add(PositionFromModel(pos));
            }

            var resource = new List<DataPositions.Position>();
            foreach (var pos in allocation.Positions.Where(p => p.Type == PositionType.Resource))
            {
                resource.Add(PositionFromModel(pos));
            }

            var internalRegistrar = new List<DataPositions.Position>();
            foreach (var pos in allocation.Positions.Where(p => p.Type == PositionType.InternalRegistrar))
            {
                internalRegistrar.Add(PositionFromModel(pos));
            }

            var externalRegistrar = new List<DataPositions.Position>();
            foreach (var pos in allocation.Positions.Where(p => p.Type == PositionType.ExternalRegistrar))
            {
                externalRegistrar.Add(PositionFromModel(pos));
            }

            var educator = new List<DataPositions.Position>();
            foreach (var pos in allocation.Positions.Where(p => p.Type == PositionType.Educator))
            {
                educator.Add(PositionFromModel(pos));
            }

            return new SeniorTeam(
                PositionFromModel(accessCoordinator),
                PositionFromModel(tech),
                PositionFromModel(mern),
                PositionFromModel(caSupport),
                PositionFromModel(wardOnCallConsultant),
                PositionFromModel(transportRegistrar),
                PositionFromModel(donationCoordinator),
                cnm,
                cnc,
                resource,
                internalRegistrar,
                externalRegistrar,
                educator);
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.Position"/>
        /// to a <see cref="T:HospitalAllocation.Data.Allocation.Positions.Position"/>.
        /// </summary>
        /// <param name="position">The position to convert.</param>
        /// <returns>The converted position.</returns>
        public static DataPositions.Position PositionFromModel(Position position)
        {
            if (position.StaffPosition is KnownStaffPosition knownPos)
            {
                int staffId = knownPos.StaffMemberId ?? 0;
                return new DataPositions.IdentifiedPosition(staffId, shiftType: ShiftTypeFromModel(position.ShiftType));
            }
            else if (position.StaffPosition is UnknownStaffPosition unknownPos)
            {
                return new DataPositions.UnidentifiedPosition(unknownPos.StaffName ?? String.Empty, ShiftTypeFromModel(position.ShiftType));
            }
            else
            {
                return DataPositions.UnidentifiedPosition.Empty;
            }
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.ShiftType"/>
        /// to a <see cref="T:HospitalAllocation.Data.Allocation.Positions.ShiftType"/>.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The converted type.</returns>
        public static DataPositions.ShiftType ShiftTypeFromModel(ShiftType type)
        {
            switch (type)
            {
                case ShiftType.EightHour:
                    return DataPositions.ShiftType.EightHour;
                case ShiftType.TwelveHour:
                    return DataPositions.ShiftType.TwelveHour;
                case ShiftType.Closed:
                    return DataPositions.ShiftType.Closed;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.TeamType"/>
        /// to a <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.TeamType"/>.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The converted type.</returns>
        public static Data.Allocation.StaffGroups.TeamType TeamTypeFromModel(Model.TeamType? type)
        {
            switch (type)
            {
                case Model.TeamType.A:
                    return Data.Allocation.StaffGroups.TeamType.A;
                case Model.TeamType.B:
                    return Data.Allocation.StaffGroups.TeamType.B;
                case Model.TeamType.C:
                    return Data.Allocation.StaffGroups.TeamType.C;
                case Model.TeamType.D:
                    return Data.Allocation.StaffGroups.TeamType.D;
                case Model.TeamType.Senior:
                    return Data.Allocation.StaffGroups.TeamType.Senior;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Data.Allocation.Positions.ShiftType"/>
        /// to a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.ShiftType"/>.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The converted type.</returns>
        public static ShiftType ShiftTypeToModel(DataPositions.ShiftType type)
        {
            switch (type)
            {
                case DataPositions.ShiftType.EightHour:
                    return ShiftType.EightHour;
                case DataPositions.ShiftType.TwelveHour:
                    return ShiftType.TwelveHour;
                case DataPositions.ShiftType.Closed:
                    return ShiftType.Closed;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.TeamType"/>
        /// to a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.TeamType"/>.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The converted type.</returns>
        public static Model.TeamType TeamTypeToModel(Data.Allocation.StaffGroups.TeamType type)
        {
            switch (type)
            {
                case Data.Allocation.StaffGroups.TeamType.A:
                    return Model.TeamType.A;
                case Data.Allocation.StaffGroups.TeamType.B:
                    return Model.TeamType.B;
                case Data.Allocation.StaffGroups.TeamType.C:
                    return Model.TeamType.C;
                case Data.Allocation.StaffGroups.TeamType.D:
                    return Model.TeamType.D;
                case Data.Allocation.StaffGroups.TeamType.Senior:
                    return Model.TeamType.Senior;
                default:
                    throw new ArgumentException();
            }
        }

        public static ModelConverter FromDb(DbContextOptions<AllocationContext> dbOptions)
        {
            using (var dbContext = new AllocationContext(dbOptions))
            {
                IDictionary<int, StaffMember> staff = dbContext.StaffMembers
                    .ToDictionary(sm => sm.StaffMemberId, sm => sm);

                return new ModelConverter(staff);
            }
        }

        private readonly IDictionary<int, StaffMember> _staff;

        public ModelConverter(IDictionary<int, StaffMember> staff)
        {
            _staff = staff;
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.Pod"/>
        /// to a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.PodAllocation"/>.
        /// </summary>
        /// <param name="allocation">The allocation to convert.</param>
        /// <returns>The converted allocation.</returns>
        public TeamAllocation PodToModel(Pod allocation)
        {
            var positions = new List<Position>();
            foreach (var bed in allocation.Beds)
            {
                positions.Add(BedToModel(bed.Key, bed.Value));
            }

            positions.Add(PositionToModel(allocation.Consultant, PositionType.Consultant));
            positions.Add(PositionToModel(allocation.TeamLeader, PositionType.TeamLeader));
            positions.Add(PositionToModel(allocation.Registrar, PositionType.Registrar));
            positions.Add(PositionToModel(allocation.Resident, PositionType.Resident));
            positions.Add(PositionToModel(allocation.PodCa, PositionType.PodCa));
            positions.Add(PositionToModel(allocation.CaCleaner, PositionType.CaCleaner));

            return new TeamAllocation
            {
                Type = TeamTypeToModel(allocation.TeamType),
                Positions = positions,
            };
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Data.Allocation.StaffGroups.SeniorTeam"/>
        /// to a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.SeniorStaffAllocation"/>.
        /// </summary>
        /// <param name="allocation">The allocation to convert.</param>
        /// <returns>The converted allocation.</returns>
        public TeamAllocation SeniorTeamToModel(SeniorTeam allocation)
        {
            var positions = new List<Position>();
            foreach (var pos in allocation.Cnm)
            {
                positions.Add(PositionToModel(pos, PositionType.Cnm));
            }

            var cnc = new List<Position>();
            foreach (var pos in allocation.Cnc)
            {
                positions.Add(PositionToModel(pos, PositionType.Cnc));
            }

            var resource = new List<Position>();
            foreach (var pos in allocation.Resource)
            {
                positions.Add(PositionToModel(pos, PositionType.Resource));
            }

            var internalRegistrar = new List<Position>();
            foreach (var pos in allocation.InternalRegistrar)
            {
                positions.Add(PositionToModel(pos, PositionType.InternalRegistrar));
            }

            var externalRegistrar = new List<Position>();
            foreach (var pos in allocation.ExternalRegistrar)
            {
                positions.Add(PositionToModel(pos, PositionType.ExternalRegistrar));
            }

            var educator = new List<Position>();
            foreach (var pos in allocation.Educator)
            {
                positions.Add(PositionToModel(pos, PositionType.Educator));
            }

            positions.Add(PositionToModel(allocation.AccessCoordinator, PositionType.AccessCoordinator));
            positions.Add(PositionToModel(allocation.Tech, PositionType.Tech));
            positions.Add(PositionToModel(allocation.Mern, PositionType.Mern));
            positions.Add(PositionToModel(allocation.CaSupport, PositionType.CaSupport));
            positions.Add(PositionToModel(allocation.WardOnCallConsultant, PositionType.WardOnCallConsultant));
            positions.Add(PositionToModel(allocation.TransportRegistrar, PositionType.TransportRegistrar));
            positions.Add(PositionToModel(allocation.DonationCoordinator, PositionType.DonationCoordinator));

            return new TeamAllocation
            {
                Type = Model.TeamType.Senior,
                Positions = positions,
            };
        }

        /// <summary>
        /// Converts a bed <see cref="T:HospitalAllocation.Data.Allocation.Positions.Position"/>
        /// to a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.BedPosition"/>.
        /// </summary>
        /// <param name="bedId">The id of the bed.</param>
        /// <param name="position">The bed.</param>
        /// <returns>The converted bed.</returns>
        public BedPosition BedToModel(int bedId, DataPositions.Position position)
        {
            if (position is DataPositions.IdentifiedPosition)
            {
                int? staffId = LookupModelStaffId((position as DataPositions.IdentifiedPosition).StaffId);
                if (staffId != null)
                {
                    return new BedPosition
                    {
                        Type = PositionType.Bed,
                        Position = bedId,
                        ShiftType = ShiftTypeToModel(position.ShiftType),
                        StaffPosition = new KnownStaffPosition
                        {
                            StaffMemberId = staffId
                        },
                    };
                }
            }
            else if (position is DataPositions.UnidentifiedPosition)
            {
                var staffName = (position as DataPositions.UnidentifiedPosition).StaffName;
                return new BedPosition()
                {
                    Type = PositionType.Bed,
                    Position = bedId,
                    ShiftType = ShiftTypeToModel(position.ShiftType),
                    StaffPosition = new UnknownStaffPosition
                    {
                        StaffName = staffName
                    },
                };
            }
            throw new ArgumentException("Invalid Staff position supplied");
        }

        /// <summary>
        /// Converts a <see cref="T:HospitalAllocation.Data.Allocation.Positions.Position"/>
        /// to a <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.Position"/>.
        /// </summary>
        /// <param name="position">The position to convert.</param>
        /// <param name="type">The type of the position.</param>
        /// <returns>The converted position.</returns>
        public Position PositionToModel(DataPositions.Position position, PositionType type)
        {
            if (position is DataPositions.IdentifiedPosition)
            {
                int? staffId = LookupModelStaffId((position as DataPositions.IdentifiedPosition).StaffId);

                if (staffId != null)
                {
                    return new Position()
                    {
                        Type = type,
                        ShiftType = ShiftTypeToModel(position.ShiftType),
                        StaffPosition = new KnownStaffPosition
                        {
                            StaffMemberId = staffId
                        },
                    };
                }
                else
                {
                    throw new ArgumentException("Position data object could not be parsed to Position model object, illegal staffId supplied.");
                }
            }
            return new Position
            {
                Type = type,
                ShiftType = ShiftTypeToModel(position.ShiftType),
                StaffPosition = new UnknownStaffPosition
                {
                    StaffName = (position as DataPositions.UnidentifiedPosition).StaffName
                },
            };
        }

        /// <summary>
        /// Take a data passing staff member value and create a corresponding model object
        /// </summary>
        /// <param name="staffId">the id of the staff member to lookup</param>
        /// <returns>the model staff member value</returns>
        private int? LookupModelStaffId(int staffId)
        {
            // First, try and find the staff member in our list.
            // If we don't have them, return null
            if (!_staff.TryGetValue(staffId, out StaffMember staffMember))
            {
                return null;
            }

            return staffMember.StaffMemberId;
        }
    }
}
