using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInvigilatorFromExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvigilatorId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_InvigilatorId",
                table: "Rooms",
                column: "InvigilatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Users_InvigilatorId",
                table: "Rooms",
                column: "InvigilatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Users_InvigilatorId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_InvigilatorId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "InvigilatorId",
                table: "Rooms");
        }
    }
}
