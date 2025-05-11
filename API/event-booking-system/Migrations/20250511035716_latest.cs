using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace event_booking_system.Migrations
{
    /// <inheritdoc />
    public partial class latest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Bio", "ConcurrencyStamp", "DateJoined", "Email", "EmailConfirmed", "FirstName", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "Cairo, Egypt", null, "9a6f32ea-d89c-4098-badd-a4ad95fd31ed", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ahmedashraf@example.com", false, "Ahmed", null, "Ashraf", false, null, null, null, null, "01009970776", false, 1, "2b24c537-4d15-4eba-a2b7-0eb7eedc0eaf", false, "ahmedashraf" },
                    { "2", 0, "New York, USA", null, "253b12d9-54ea-47d9-abaa-39cec33041a0", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "johndoe@example.com", false, "John", null, "Doe", false, null, null, null, null, "01234567890", false, 0, "4cb67efd-9d90-49ff-9795-032b5e3a312b", false, "johndoe" },
                    { "3", 0, "London, UK", null, "3355fcc1-c994-4063-88e7-85c944c75e25", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "sarah.smith@example.com", false, "Sarah", null, "Smith", false, null, null, null, null, "01122334455", false, 1, "7b51fbc4-c070-4d32-866d-05dce4b77077", false, "sarahsmith" },
                    { "4", 0, "Los Angeles, USA", null, "e9317caf-ce39-48cc-8944-63106d004444", new DateTime(2023, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "emily.brown@example.com", false, "Emily", null, "Brown", false, null, null, null, null, "01658748391", false, 1, "e9767dd3-74f9-4952-bc96-2dfbe438de06", false, "emilybrown" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Music" },
                    { 2, "Sports" },
                    { 3, "Theater" },
                    { 4, "Comedy" },
                    { 5, "Conference" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "AdmissionPrice", "AdmissionTicketQty", "CategoryId", "CreatedById", "Date", "Description", "ImageUrl", "Location", "Title", "VipPrice", "VipTicketQty" },
                values: new object[,]
                {
                    { 1, 50m, 200, 1, "1", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling rock concert with top bands.", "uploads/Rock Concert.jpeg", "Cairo Arena", "Rock Concert", 150m, 100 },
                    { 2, 30m, 500, 2, "2", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling football match between the top teams.", "uploads/football.jpeg", "Football Stadium", "Football Match", 100m, 300 },
                    { 3, 40m, 150, 3, "3", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A stunning rendition of Shakespeare's Hamlet.", "uploads/Shakespeare Play.jpg", "London Theater", "Shakespeare Play", 120m, 50 },
                    { 4, 20m, 300, 4, "4", new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Laugh-out-loud comedy with top comedians.", "uploads/Stand-up Comedy Show.jpeg", "LA Comedy Club", "Stand-up Comedy Show", 80m, 100 },
                    { 5, 75m, 400, 5, "2", new DateTime(2024, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Learn the latest in technology from industry leaders.", "uploads/Tech Conference.jpeg", "Tech Convention Center", "Tech Conference", 200m, 150 },
                    { 6, 50m, 200, 1, "1", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling rock concert with top bands.", "uploads/Rock Concert.jpeg", "Cairo Arena", "Rock Concert", 150m, 100 },
                    { 7, 30m, 500, 2, "2", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling football match between the top teams.", "uploads/football.jpeg", "Football Stadium", "Football Match", 100m, 300 },
                    { 8, 40m, 150, 3, "3", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A stunning rendition of Shakespeare's Hamlet.", "uploads/Shakespeare Play.jpg", "London Theater", "Shakespeare Play", 120m, 50 },
                    { 9, 20m, 300, 4, "4", new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Laugh-out-loud comedy with top comedians.", "uploads/Stand-up Comedy Show.jpeg", "LA Comedy Club", "Stand-up Comedy Show", 80m, 100 },
                    { 10, 50m, 200, 1, "1", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling rock concert with top bands.", "uploads/Rock Concert.jpeg", "Cairo Arena", "Rock Concert", 150m, 100 },
                    { 11, 30m, 500, 2, "2", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling football match between the top teams.", "uploads/football.jpeg", "Football Stadium", "Football Match", 100m, 300 },
                    { 12, 40m, 150, 3, "3", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A stunning rendition of Shakespeare's Hamlet.", "uploads/Shakespeare Play.jpg", "London Theater", "Shakespeare Play", 120m, 50 },
                    { 13, 20m, 300, 4, "4", new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Laugh-out-loud comedy with top comedians.", "uploads/Stand-up Comedy Show.jpeg", "LA Comedy Club", "Stand-up Comedy Show", 80m, 100 },
                    { 14, 75m, 400, 5, "2", new DateTime(2024, 5, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Learn the latest in technology from industry leaders.", "uploads/Tech Conference.jpeg", "Tech Convention Center", "Tech Conference", 200m, 150 },
                    { 15, 50m, 200, 1, "1", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling rock concert with top bands.", "uploads/Rock Concert.jpeg", "Cairo Arena", "Rock Concert", 150m, 100 },
                    { 16, 30m, 500, 2, "2", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A thrilling football match between the top teams.", "uploads/football.jpeg", "Football Stadium", "Football Match", 100m, 300 },
                    { 17, 40m, 150, 3, "3", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A stunning rendition of Shakespeare's Hamlet.", "uploads/Shakespeare Play.jpg", "London Theater", "Shakespeare Play", 120m, 50 },
                    { 18, 20m, 300, 4, "4", new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Laugh-out-loud comedy with top comedians.", "uploads/Stand-up Comedy Show.jpeg", "LA Comedy Club", "Stand-up Comedy Show", 80m, 100 },
                    { 19, 40m, 150, 3, "3", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "A stunning rendition of Shakespeare's Hamlet.", "uploads/Shakespeare Play.jpg", "London Theater", "Shakespeare Play", 120m, 50 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "AdmissionTicketQty", "BookingDate", "EventId", "Status", "TicketType", "UserId", "VipTicketQty" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 0, 2, "1", 1 },
                    { 2, 4, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, 0, "2", 0 },
                    { 3, 1, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, 2, 0, "3", 0 },
                    { 4, 3, new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 4, 1, 0, "4", 0 },
                    { 5, 2, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 5, 1, 2, "1", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
