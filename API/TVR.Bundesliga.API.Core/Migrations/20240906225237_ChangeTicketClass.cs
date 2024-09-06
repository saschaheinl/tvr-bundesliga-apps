using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVR.Bundesliga.API.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTicketClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Guest_GuestId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Guest_GuestId",
                table: "Tickets",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Guest_GuestId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "Tickets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Guest_GuestId",
                table: "Tickets",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id");
        }
    }
}
