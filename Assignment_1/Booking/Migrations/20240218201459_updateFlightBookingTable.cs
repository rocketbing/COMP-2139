using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Migrations
{
    /// <inheritdoc />
    public partial class updateFlightBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "FlightBooking");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "FlightBooking",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "HotelBookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "FlightBooking",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "HotelBookings");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "FlightBooking");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "FlightBooking",
                newName: "ToDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "FlightBooking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
