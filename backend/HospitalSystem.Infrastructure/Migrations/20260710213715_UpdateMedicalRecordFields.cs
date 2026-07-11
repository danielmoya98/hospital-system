using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalRecordFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ClinicalNotes",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "RecordDate",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MedicalRecords");

            migrationBuilder.RenameColumn(
                name: "TreatmentPlan",
                table: "MedicalRecords",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "Prescriptions",
                table: "MedicalRecords",
                newName: "AttachmentUrl");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppointmentId",
                table: "MedicalRecords",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Treatment",
                table: "MedicalRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Treatment",
                table: "MedicalRecords");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "MedicalRecords",
                newName: "TreatmentPlan");

            migrationBuilder.RenameColumn(
                name: "AttachmentUrl",
                table: "MedicalRecords",
                newName: "Prescriptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppointmentId",
                table: "MedicalRecords",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "ClinicalNotes",
                table: "MedicalRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "MedicalRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RecordDate",
                table: "MedicalRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "MedicalRecords",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
