using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// A <see cref="T:Microsoft.EntityFrameworkCore.DbContext"/> for the entities in the allocation database.
    /// </summary>
    public class AllocationContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HospitalAllocation.Providers.Allocation.Database.AllocationContext"/> class using
        /// the specified options.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public AllocationContext(DbContextOptions<AllocationContext> options)
            : base(options)
        { }

        /// <summary>
        /// The set of <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.TeamAllocation"/>
        /// database entries.
        /// </summary>
        public DbSet<TeamAllocation> TeamAllocations { get; set; }

        /// <summary>
        /// The set of <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.TeamTypeEntry"/>
        /// database entries.
        /// </summary>
        public DbSet<TeamTypeEntry> TeamTypes { get; set; }

        /// <summary>
        /// The set of <see cref="T:HospitalAllocation.Providers.Allocation.Database.Model.PositionTypeEntry"/>
        /// database entries.
        /// </summary>
        public DbSet<PositionTypeEntry> PositionTypes { get; set; }

        /// <summary>
        /// Staff member designations, denoting what kind of staff member they are
        /// </summary>
        public DbSet<Designation> Designations { get; set; }

        /// <summary>
        /// Photos of staff members
        /// </summary>
        public DbSet<Photo> Photos { get; set; }

        /// <summary>
        /// Skills that staff members may have
        /// </summary>
        public DbSet<Skill> Skills { get; set; }

        /// <summary>
        /// Notes that staff members may have
        /// </summary>
        public DbSet<Note> Notes { get; set; }

        /// <summary>
        /// The staff members of the ICU to be allocated to team positions
        /// </summary>
        public DbSet<StaffMember> StaffMembers { get; set; }

        /// <summary>
        /// The handovers sent to nurses after allocation
        /// </summary>
        /// <value>The handovers.</value>
        public DbSet<Handover> Handovers { get; set; }

        /// <summary>
        /// The issues information in the handovers
        /// </summary>
        /// <value>The handover issues.</value>
        public DbSet<HandoverIssue> HandoverIssues { get; set; }


        public DbSet<CSVFileStorage> CSVFileStorages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set alternate key for PodAllocation
            modelBuilder.Entity<TeamAllocation>()
                .HasAlternateKey(p => new { p.Type, p.Time });

            // Set alternate key for PositionTypeEntry
            modelBuilder.Entity<PositionTypeEntry>()
                .HasAlternateKey(p => p.Type);

            // Set alternate key for TeamTypeEntry
            modelBuilder.Entity<TeamTypeEntry>()
                .HasAlternateKey(p => p.Type);

            modelBuilder.Entity<Designation>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Skill>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<CSVFileStorage>()
                .HasIndex(p => p.CSVTimeStamp);

            // Make migration aware of these classes
            modelBuilder.Entity<BedPosition>();
            modelBuilder.Entity<UnknownStaffPosition>();

            modelBuilder.Entity<Handover>();
            modelBuilder.Entity<HandoverIssue>()
                        .HasOne(p => p.Handover)
                        .WithMany(b => b.HandoverIssues)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

        }

        /// <summary>
        /// Ensure that the database has the seeded values
        /// </summary>
        public void EnsureSeedData()
        {
            foreach (var type in Enum.GetValues(typeof(TeamType)).Cast<TeamType>())
            {
                var entry = TeamTypes.SingleOrDefault(t => t.Type == type);
                if (entry != null)
                {
                    // Database entry for enum value mismatch
                    if (entry.Name != Enum.GetName(typeof(TeamType), type))
                    {
                        throw new Exception("Database mapping to enum is incorrect");
                    }
                }
                else
                {
                    TeamTypes.Add(
                        new TeamTypeEntry
                        {
                            Type = type,
                            Name = Enum.GetName(typeof(TeamType), type)
                        });
                }
            }

            foreach (var type in Enum.GetValues(typeof(PositionType)).Cast<PositionType>())
            {
                var entry = PositionTypes.SingleOrDefault(t => t.Type == type);
                if (entry != null)
                {
                    // Database entry for enum value mismatch
                    if (entry.Name != Enum.GetName(typeof(PositionType), type))
                    {
                        throw new Exception("Database mapping to enum is incorrect");
                    }
                }
                else
                {
                    PositionTypes.Add(
                        new PositionTypeEntry
                        {
                            Type = type,
                            Name = Enum.GetName(typeof(PositionType), type)
                        });
                }
            }

            SaveChanges();
        }
    }
}
