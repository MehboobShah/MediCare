using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class ModifiedPatientReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNo",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Age",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "BedNo",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "CollectedOn",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "MedicalRecordNo",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "MedicalReportNo",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Receipt",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "ReferredBy",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "ReportedOn",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "RequestedOn",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "SpecimenNo",
                schema: "Users",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Ward",
                schema: "Users",
                table: "Patient");

            migrationBuilder.AddColumn<string>(
                name: "AccountNo",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Age",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BedNo",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CollectedOn",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalRecordNo",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalReportNo",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferredBy",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportedOn",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedOn",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecimenNo",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                schema: "Report",
                table: "PatientReport",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNo",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "Age",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "BedNo",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "CollectedOn",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "MedicalRecordNo",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "MedicalReportNo",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "Receipt",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "ReferredBy",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "ReportedOn",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "RequestedOn",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "SpecimenNo",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.DropColumn(
                name: "Ward",
                schema: "Report",
                table: "PatientReport");

            migrationBuilder.AddColumn<string>(
                name: "AccountNo",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Age",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BedNo",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CollectedOn",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalRecordNo",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalReportNo",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferredBy",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportedOn",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedOn",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecimenNo",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                schema: "Users",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
