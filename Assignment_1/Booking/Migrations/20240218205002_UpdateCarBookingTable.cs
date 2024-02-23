using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "CarBookings");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "CarBookings",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "CarBookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "CarBookings");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CarBookings",
                newName: "ToDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "CarBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
