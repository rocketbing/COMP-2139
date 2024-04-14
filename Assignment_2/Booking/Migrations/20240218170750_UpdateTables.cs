using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.RenameColumn(
                name: "FlightNumber",
                table: "Flights",
                newName: "FlightName");

            migrationBuilder.RenameColumn(
                name: "CarModel",
                table: "CarRentals",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Amenities",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Airline",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "CarRentals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modal",
                table: "CarRentals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amenities",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Airline",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "CarRentals");

            migrationBuilder.DropColumn(
                name: "Modal",
                table: "CarRentals");

            migrationBuilder.RenameColumn(
                name: "FlightName",
                table: "Flights",
                newName: "FlightNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CarRentals",
                newName: "CarModel");

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CarRentalId = table.Column<int>(type: "int", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_CarRentals_Id",
                        column: x => x.Id,
                        principalTable: "CarRentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Flights_Id",
                        column: x => x.Id,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Hotels_Id",
                        column: x => x.Id,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
