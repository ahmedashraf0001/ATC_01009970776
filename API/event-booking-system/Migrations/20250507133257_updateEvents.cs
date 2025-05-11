using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_booking_system.Migrations
{
    /// <inheritdoc />
    public partial class updateEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdmissionTicketQty",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VipTicketQty",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdmissionTicketQty",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VipTicketQty",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionTicketQty",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "VipTicketQty",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdmissionTicketQty",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "VipTicketQty",
                table: "Bookings");
        }
    }
}
