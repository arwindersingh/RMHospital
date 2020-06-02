using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HospitalAllocation.Migrations
{
    public partial class AddCsvAndHandover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NEW_StaffMembers",
                columns: table => new
                {
                    StaffMemberId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(type: "TEXT", nullable: true),
                    DesignationId = table.Column<int>(type: "INTEGER", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastDouble = table.Column<long>(type: "INTEGER", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    PhotoId = table.Column<int>(type: "INTEGER", nullable: true),
                    RosterOnId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.StaffMemberId);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "DesignationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                });
            
            migrationBuilder.Sql("INSERT INTO NEW_StaffMembers (FirstName, LastName, StaffMemberId, DesignationId, LastDouble, PhotoId, RosterOnId) "
                                + "SELECT substr(Name || ' ', 1, pos-1), CASE WHEN trim(substr(Name || ' ', pos+1)) == '' THEN null "
                                + "ELSE trim(substr(Name || ' ', pos+1)) END, StaffMemberId, DesignationId, LastDouble, PhotoId, RosterOnId FROM "
                                + "(SELECT *, instr(Name || ' ',' ') AS pos FROM StaffMembers);");
            migrationBuilder.Sql("PRAGMA foreign_keys=\"0\"", true);
            migrationBuilder.Sql("DROP TABLE StaffMembers", true);
            migrationBuilder.Sql("ALTER TABLE NEW_StaffMembers RENAME TO StaffMembers", true);
            migrationBuilder.Sql("PRAGMA foreign_keys=\"1\"", true);

            migrationBuilder.CreateTable(
                name: "CSVFileStorages",
                columns: table => new
                {
                    CSVFileID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CSVFileData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    CSVFileFormat = table.Column<int>(type: "INTEGER", nullable: false),
                    CSVTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSVFileStorages", x => x.CSVFileID);
                });

            migrationBuilder.CreateTable(
                name: "Handovers",
                columns: table => new
                {
                    HandoverID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdmissionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AdmissionUnit = table.Column<string>(type: "TEXT", nullable: false),
                    Alerts = table.Column<string>(type: "TEXT", nullable: false),
                    BedNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Isolation = table.Column<string>(type: "TEXT", nullable: true),
                    NurseName = table.Column<string>(type: "TEXT", nullable: true),
                    PastMedicalHistory = table.Column<string>(type: "TEXT", nullable: true),
                    PatientId = table.Column<string>(type: "TEXT", nullable: false),
                    PatientName = table.Column<string>(type: "TEXT", nullable: false),
                    PresentingComplaint = table.Column<string>(type: "TEXT", nullable: true),
                    SignificantEvents = table.Column<string>(type: "TEXT", nullable: true),
                    StudentName = table.Column<string>(type: "TEXT", nullable: true),
                    SwabSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    SwabSentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Handovers", x => x.HandoverID);
                });

            migrationBuilder.CreateTable(
                name: "HandoverIssues",
                columns: table => new
                {
                    HandoverIssueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentIssue = table.Column<string>(type: "TEXT", nullable: true),
                    FollowUp = table.Column<string>(type: "TEXT", nullable: true),
                    HandoverID = table.Column<int>(type: "INTEGER", nullable: false),
                    IssueNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Management = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandoverIssues", x => x.HandoverIssueId);
                    table.ForeignKey(
                        name: "FK_HandoverIssues_Handovers_HandoverID",
                        column: x => x.HandoverID,
                        principalTable: "Handovers",
                        principalColumn: "HandoverID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CSVFileStorages_CSVTimeStamp",
                table: "CSVFileStorages",
                column: "CSVTimeStamp");

            migrationBuilder.CreateIndex(
                name: "IX_HandoverIssues_HandoverID",
                table: "HandoverIssues",
                column: "HandoverID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSVFileStorages");

            migrationBuilder.DropTable(
                name: "HandoverIssues");

            migrationBuilder.DropTable(
                name: "Handovers");

            migrationBuilder.CreateTable(
                name: "NEW_StaffMembers",
                columns: table => new
                {
                    StaffMemberId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DesignationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, defaultValue: ""),
                    LastDouble = table.Column<long>(type: "INTEGER", nullable: true),
                    PhotoId = table.Column<int>(type: "INTEGER", nullable: true),
                    RosterOnId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.StaffMemberId);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "DesignationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffMembers_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                });
            
            migrationBuilder.Sql("INSERT INTO NEW_StaffMembers (StaffMemberId, DesignationId, Name, LastDouble, PhotoId, RosterOnId) "
                                + "SELECT StaffMemberId, DesignationId, CASE WHEN LastName IS NULL THEN FirstName ELSE trim(FirstName || ' ' || LastName) END, "
                                + "LastDouble, PhotoId, RosterOnId FROM StaffMembers;");
            migrationBuilder.Sql("PRAGMA foreign_keys=\"0\"", true);
            migrationBuilder.Sql("DROP TABLE StaffMembers", true);
            migrationBuilder.Sql("ALTER TABLE NEW_StaffMembers RENAME TO StaffMembers", true);
            migrationBuilder.Sql("PRAGMA foreign_keys=\"1\"", true);
        }
    }
}
